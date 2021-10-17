using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using BusinessLaag.Exceptions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

using BusinessLaag.Interfaces;
using System.IO;

namespace DataLaag
{
#nullable enable
    public class DatabankConfigureerder : IDatabankConfigureerder
    {
        // Het gebruik van een master connectie is enkel intern vereist, voor het aanmaken van een databank
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

        public DatabankConfigureerder(List<string> tabellen,
                                     string databanknaam = "FleetManager",
                                     string dataSource = @".\SQLEXPRESS",
                                     bool integratedSecurity = true,
                                     string? relatiefTovSolutionPad = null,  // bijv. "/DataLaag/_SQL/"
                                     string? volledigFolderPad = null)
        {

            //AppDomain.CurrentDomain.BaseDirectory niet toegelaten als default param in ctor signatuur, dus toekennen in body
            if (volledigFolderPad == null && relatiefTovSolutionPad == null)
            {
                volledigFolderPad = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "/DataLaag/_SQL/");
            }
            else if (relatiefTovSolutionPad != null && volledigFolderPad == null)
            {
                volledigFolderPad = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relatiefTovSolutionPad);
            }
            else if (relatiefTovSolutionPad != null && volledigFolderPad != null)
            {
                throw new DatabankConfigureerderException("Stel het relatief pad in OF het volledige pad, niet allebei tegelijk.");
            }
            else if (volledigFolderPad != null && relatiefTovSolutionPad == null)// volledigFolderPad != null && relatiefTovSolutionPad == null
            {
                if (volledigFolderPad.Length < 3)
                    throw new DatabankConfigureerderException("Volledig pad moet minimum 3 karakters lang zijn.");
                
                // Else: Er verandert niks.
            }


            // Stelt de SqlConnections en strings in
            _zetConnecties(databanknaam, dataSource, integratedSecurity); // CS8618


            _connecteerMetDatabase(databanknaam);

            if (!ConnectieSuccesvol)
            {
                SequentieDoorlopen = true;
            }
            else
            {
                if (!DatabaseBestaat)
                {
                    _maakOntbrekendeDatabank(databanknaam);
                    _connecteerMetDatabase(databanknaam);
                }

                _controleerBestaanTabellen(tabellen);
                if (!AlleTabellenBestaan)
                {
                    _maakOntbrekendeTabellenAan(databanknaam, volledigFolderPad);
                    _controleerBestaanTabellen(tabellen);
                }

                _ = _geefAantalTabellenVoorDatabase(databanknaam); // AantalTabellen populaten
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

        // TODO: db create functie testen (wat als db al bestaat)
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

        // TODO: tabel functie testen (is het pad juist?)
        // uitproberen met een bestand dat in human-readable format is gemaakt (met enters)
        // Het script gaat op zoek naar .sql files welke matchen met een entry uit _ontbrekendeTabellen

        // moeten backticks gebruikt worden? https://stackoverflow.com/questions/44819719/check-if-table-creation-was-successful
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
                int.TryParse(test.ToString(), out output);
                AantalTabellen = output;
                return output;
            }
            catch (Exception e)
            {
                return 0;
            }
            finally
            {
                ProductieConnectie.Close();
            }


        }
        private void _maakOntbrekendeTabellenAan(string databanknaam, string sqlpad)
        {
            IEnumerable<string> sqlBestandPaden = Directory.EnumerateFiles(sqlpad, "*.sql");
            int tabelTeller = _geefAantalTabellenVoorDatabase(databanknaam);

            foreach (string filePad in sqlBestandPaden)
            {
                try
                {
                    ProductieConnectie.Open();
                    string data = File.ReadAllText(filePad);

                    using (SqlCommand command = ProductieConnectie.CreateCommand())
                    {
                        command.CommandText = data.Replace(@"\r", "").Replace(@"\n", ""); 
                        //eventueel ook quotes ' en " naar backtick ` hier

                        command.ExecuteNonQuery();
                        ProductieConnectie.Close();
                        int tussenGetal = _geefAantalTabellenVoorDatabase(databanknaam);

                        if (!(tussenGetal > tabelTeller))
                        {
                            throw new DatabankConfigureerderException($"Tabel aanmaken mislukt! Voor: {tabelTeller - 1} Na: {tussenGetal-1}");
                        }

                        tabelTeller = tussenGetal;
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

        //Publieke methoden hieronder -----------------------------------------------------
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