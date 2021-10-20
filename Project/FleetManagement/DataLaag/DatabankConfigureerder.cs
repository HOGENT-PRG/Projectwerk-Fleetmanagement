using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using BusinessLaag.Exceptions;
using System.Linq;
using System.Data;

using BusinessLaag.Interfaces;
using System.Net;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;

namespace DataLaag
{
#nullable enable
    public class DatabankConfigureerder : IDatabankConfigureerder
    {
        // Het gebruik van een master connectie is enkel intern vereist
        private string MasterConnectieString { get; set; }

        // Wordt gebruikt om SqlConnections aan te maken in de Repositories
        public string ProductieConnectieString { get; private set; }

        // SqlConnections zijn private omdat ze anders aanroepbaar worden door managers,
        // een SqlConnection maken daar kunnen ze niet dus enkel de connectionstrings worden ge-exposed
        private SqlConnection MasterConnectie { get; set; }
        private SqlConnection ProductieConnectie { get; set; } 
        
        // Volgende zaken zijn van belang voor interne werking (moeten er db/tables aangemaakt worden?)
        // en laat de presentatielaag weten wat de stand van zaken is bij initialisatie
        public bool ConnectieSuccesvol { get; private set; }
        public bool DatabaseBestaat { get; private set; }
        public bool AlleTabellenBestaan { get; private set; }
        public int AantalTabellen { get; private set; }
        public bool SequentieDoorlopen { get; private set; }

        private List<string> _ontbrekendeTabellen = new List<string>();

        public DatabankConfigureerder(Dictionary<string, string> tabellen,
                                     string databanknaam = "FleetManager",
                                     string dataSource = @".\SQLEXPRESS",
                                     bool integratedSecurity = true)
        {
            // Stelt de SqlConnections en connectionstrings in
            _zetConnecties(databanknaam, dataSource, integratedSecurity);

            // Doorloopt de configuratiesequentie, stelt overige properties in
            _doorloopSequentie(databanknaam, tabellen);  
        }

        private void _doorloopSequentie(string databanknaam, Dictionary<string, string> gewensteTabellen)
        {
            List<string> gewensteTabellenNamen = gewensteTabellen.Keys.ToList();

            // Test verbinding, populeert ConnectieSuccesvol en DatabaseBestaat
            _connecteerMetDatabase(databanknaam);

            if (ConnectieSuccesvol)
            {
                if (!DatabaseBestaat)
                {
                    _maakOntbrekendeDatabank(databanknaam);
                }

                _controleerBestaanTabellen(gewensteTabellenNamen); // populeert AlleTabellenBestaan
                if (!AlleTabellenBestaan)
                {
                    _maakOntbrekendeTabellenAan(databanknaam, gewensteTabellen);
                }

                _connecteerMetDatabase(databanknaam); // populeert DatabaseBestaat
                _controleerBestaanTabellen(gewensteTabellenNamen); // populeert AlleTabellenBestaan
                _geefAantalTabellenVoorDatabase(databanknaam); // AantalTabellen populaten
            }

            SequentieDoorlopen = true;
        }
        private void _zetConnecties(string dbnaam, string datasource, bool integratedsecurity)
        {
            // Aanmaken SqlConnections
            SqlConnectionStringBuilder masterBouwer = new SqlConnectionStringBuilder();
            masterBouwer.InitialCatalog = "Master";
            masterBouwer.DataSource = datasource;
            masterBouwer.IntegratedSecurity = integratedsecurity;
            MasterConnectieString = masterBouwer.ConnectionString;
            MasterConnectie = new(MasterConnectieString);

            SqlConnectionStringBuilder productieBouwer = new SqlConnectionStringBuilder();
            productieBouwer.InitialCatalog = dbnaam;
            productieBouwer.DataSource = datasource;
            productieBouwer.IntegratedSecurity = integratedsecurity;
            ProductieConnectieString = productieBouwer.ConnectionString;
            ProductieConnectie = new(ProductieConnectieString);
        }
        private void _connecteerMetDatabase(string databanknaam)
        {
            try
            {
                string query = "select count(*) from master.dbo.sysdatabases where name=@dbNaam;";

                MasterConnectie.Open();
                SqlCommand command = MasterConnectie.CreateCommand();

                command.Parameters.Add("@dbNaam", SqlDbType.VarChar);
                command.CommandText = query;
                command.Parameters["@dbNaam"].Value = databanknaam;

                var res = command.ExecuteScalar();
                ConnectieSuccesvol = res != null && res != DBNull.Value;
                int castedResultaat = (bool)ConnectieSuccesvol ? Convert.ToInt32(res) : 0;

                DatabaseBestaat = castedResultaat == 1;
            }
            catch (Exception e)
            {
                ConnectieSuccesvol = false;
            }
            finally
            {
                MasterConnectie.Close();
            }
        }
        private void _controleerBestaanTabellen(List<string> verwachteTabellen)
        {
            IList<string> bestaandeTabellen = this.geefTabellen();
            foreach (string tabel in verwachteTabellen)
            {
                if (!bestaandeTabellen.Contains(tabel))
                {
                    _ontbrekendeTabellen.Add(tabel);
                }
            }

            if (_ontbrekendeTabellen.Count > 0)
            {
                AlleTabellenBestaan = false;
            }
            else
            {
                AlleTabellenBestaan = true;
            }
        }
        private void _maakOntbrekendeDatabank(string databanknaam)
        {
            try
            {
                string query = string.Format("CREATE DATABASE {0}", databanknaam);
                MasterConnectie.Open();
                SqlCommand command = MasterConnectie.CreateCommand();
                command.CommandText = query;
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                DatabaseBestaat = false;
            }
            finally
            {
                MasterConnectie.Close();
            }
        }
        private int _geefAantalTabellenVoorDatabase(string databanknaam)
        {
            string sql = "SELECT count(*) FROM information_schema.TABLES WHERE (TABLE_CATALOG=@dbNaam)";

            try
            {
                ProductieConnectie.Open();

                SqlCommand cmd = ProductieConnectie.CreateCommand();
                cmd.Parameters.Add("@dbNaam", SqlDbType.VarChar);
                cmd.CommandText = sql;
                cmd.Parameters["@dbNaam"].Value = databanknaam;

                var test = cmd.ExecuteScalar();
                int output;

                if (int.TryParse(test.ToString(), out output))
                    AantalTabellen = output;
                else AantalTabellen = 0;

                return output;
            }
            catch (Exception e)
            {
                AantalTabellen = -1;
                return -1;
            }
            finally
            {
                ProductieConnectie.Close();
            }
        }
        private void _maakOntbrekendeTabellenAan(string databanknaam, Dictionary<string, string> tabellen)
        {
            IList<string> bestaandeTabellen = geefTabellen();
            List<string> bronnenTeBehandelen = new List<string>();

            foreach (KeyValuePair<string, string> entry in tabellen)
            {
                if (!bestaandeTabellen.Contains(entry.Key))
                {
                    bronnenTeBehandelen.Add(entry.Value);
                }
            }

            foreach (string url in bronnenTeBehandelen)
            {
                try
                {
                    string data;
                    using (WebClient client = new WebClient())
                    {
                        data = client.DownloadString(url);
                    }

                    if (!string.IsNullOrEmpty(data))
                    {
                        ProductieConnectie.Open();

                        //laat toe om GO statements te gebruiken
                        Server server = new Server(new ServerConnection(ProductieConnectie));
                        server.ConnectionContext.ExecuteNonQuery(data);

                    }
                }
                catch (Exception e)
                {
                    throw new DatabankConfigureerderException(e.Message); // testing purposes, anders continue
                }
                finally
                {
                    ProductieConnectie.Close();
                }
            }
        }

        //--
        public IList<string> geefTabellen()
        {
            List<string> tables = new List<string>();
            try
            {
                ProductieConnectie.Open();
                DataTable dt = ProductieConnectie.GetSchema("Tables");
                foreach (DataRow row in dt.Rows)
                {
                    string tablename = (string)row[2];
                    tables.Add(tablename);
                }
                ProductieConnectie.Close();
                return tables;
            }
            catch (Exception e)
            {
                return tables;
            }
        }


    }
#nullable disable
}