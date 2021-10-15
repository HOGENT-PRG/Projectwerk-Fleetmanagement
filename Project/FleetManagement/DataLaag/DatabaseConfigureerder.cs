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
    public class DatabaseConfigureerder : IDatabaseConfigureerder
    {
        public string MasterConnectieString { get; private set; }
        public string ProductieConnectieString { get; private set; }
        public SqlConnection MasterConnectie { get; private set; }
        public SqlConnection ProductieConnectie { get; private set; }
        public bool ConnectieSuccesvol { get; private set; }
        public bool DatabaseBestaat { get; private set; }
        public bool AlleTabellenBestaan { get; private set; }
        public bool SequentieDoorlopen { get; private set; }

        private List<string> _ontbrekendeTabellen = new List<string>();

        public DatabaseConfigureerder(List<string> tabellen,
                                     string databanknaam = "FleetManager",
                                     string dataSource = @".\SQLEXPRESS",
                                     bool integratedSecurity = true,
                                     string? sqlFolderPad = null)
        {

            if (sqlFolderPad == null)
            {
                // Niet toegelaten als default param in ctor signatuur, dus toekennen in body
                sqlFolderPad = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "/DataLaag/_SQL/");
            }

            // Aanmaken SqlConnections
            SqlConnectionStringBuilder masterBouwer = new SqlConnectionStringBuilder();
            masterBouwer.InitialCatalog = "Master";
            masterBouwer.DataSource = dataSource;
            masterBouwer.IntegratedSecurity = integratedSecurity;
            MasterConnectieString = masterBouwer.ConnectionString;
            MasterConnectie = new(MasterConnectieString);

            SqlConnectionStringBuilder productieBouwer = new SqlConnectionStringBuilder();
            productieBouwer.InitialCatalog = databanknaam;
            productieBouwer.DataSource = dataSource;
            productieBouwer.IntegratedSecurity = integratedSecurity;
            ProductieConnectieString = productieBouwer.ConnectionString;
            ProductieConnectie = new(ProductieConnectieString);

            // Kunnen we verbinden met sql server en bestaat de databank?
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
                }

                // Bestaan de tabellen?
                _controleerBestaanTabellen(tabellen);
                if (!AlleTabellenBestaan)
                {
                    _maakOntbrekendeTabellenAan(databanknaam, sqlFolderPad);
                }

            }

            SequentieDoorlopen = true;
        }

        public IList<string> geefTabellen()
        {
            List<string> tables = new List<string>();
            DataTable dt = ProductieConnectie.GetSchema("Tables");
            foreach (DataRow row in dt.Rows)
            {
                string tablename = (string)row[2];
                tables.Add(tablename);
            }
            return tables;
        }

        private void _connecteerMetDatabase(string databanknaam)
        {
            try
            {
                string query = "select count(*) from master.dbo.sysdatabases where name=@dbNaam;";

                ProductieConnectie.Open();
                SqlCommand command = ProductieConnectie.CreateCommand();

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
                ProductieConnectie.Close();
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
                string query = "CREATE DATABASE @dbNaam;";
                MasterConnectie.Open();
                SqlCommand command = MasterConnectie.CreateCommand();
                command.Parameters.Add("@dbNaam", SqlDbType.VarChar);
                command.CommandText = query;
                command.Parameters["@dbNaam"].Value = databanknaam;
                command.ExecuteScalar();

                _connecteerMetDatabase(databanknaam);
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
                        command.CommandText = data.Replace(@"\r", "").Replace(@"\n", ""); //eventueel ook quotes ' en " naar backtick ` hier
                        command.ExecuteNonQuery();

                        int tussenGetal = _geefAantalTabellenVoorDatabase(databanknaam);
                        tabelTeller++;
                        tussenGetal++;
                        if (!(tussenGetal > tabelTeller))
                        {
                            throw new Exception($"Tabel aanmaken mislukt! Voor: {tabelTeller - 1} Na: {tussenGetal - 1}");
                        }

                    }
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message); // testing purposes, anders continue
                }
                finally
                {
                    ProductieConnectie.Close();
                }
            }
        }


    }
#nullable disable
}