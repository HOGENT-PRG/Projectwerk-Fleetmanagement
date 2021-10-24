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
using DataLaag;

namespace xUnitTesting
{
    public class TestDatabankConfigureerder : DatabankConfigureerder, ITestDatabankConfigureerder {

        private SqlConnection TestMasterConnectie { get { return MasterConnectie; } }
        private SqlConnection TestConnectie { get { return ProductieConnectie; } }

        public TestDatabankConfigureerder(SortedDictionary<string, string>? tabellen=null,
                                          string databanknaam = "FleetManagerTESTING",
                                          string dataSource = @".\SQLEXPRESS",
                                          bool integratedSecurity = true) 
                                          :base(tabellen, databanknaam, dataSource, integratedSecurity){}

        // Toe te voegen:
        // methodes logica

        internal bool MaakTabellenAan() {
            _controleerBestaanTabellen(TabellenDefault.Keys.ToList());
            if(_ontbrekendeTabellen.Count > 0) {
                _maakOntbrekendeTabellenAan((string)_initialisatieParameters["databanknaam"], TabellenDefault);
                _controleerBestaanTabellen(TabellenDefault.Keys.ToList());
                if (_ontbrekendeTabellen.Count == 0) return true;
                else return false;
            } else {
                return true;
            }
        }
        internal bool TruncateTabellen(List<string> tabellen)
        {
            IList<string> bestaandeTabellen = geefTabellenLowercase();
            try {
                TestMasterConnectie.Open();
                foreach (string table in tabellen) {
                    if (bestaandeTabellen.Contains(table.ToLower())) {
                        string query = "TRUNCATE TABLE " + table;
                        SqlCommand cmd = new SqlCommand(query, TestMasterConnectie);
                        cmd.ExecuteNonQuery();
                    }
                }
            } catch { throw; } finally { TestMasterConnectie.Close(); }

            return true;
        }
        internal bool VerwijderTabellen(List<string> tabellen)
        {
            IList<string> bestaandeTabellen = geefTabellenLowercase();
            try {
                TestMasterConnectie.Open();
                foreach (string table in tabellen) {
                    if (bestaandeTabellen.Contains(table.ToLower())) {
                        string query = "DROP TABLE " + table;
                        SqlCommand cmd = new SqlCommand(query, TestMasterConnectie);
                        cmd.ExecuteNonQuery();
                    }
                }
            } catch { throw; } finally { TestMasterConnectie.Close(); }

            return true;
        }

        Func<string, string> keyFormatted = p => String.Concat("?", p);

        // data = lijst met dicts waarbij string=column name en object is insert value
        // elke dict hoort, aangezien het 1 tabel betreft, dezelfde keys te bevatten
        internal bool VoerDataIn(string tabelnaam, List<Dictionary<string, object>> data) {
            IList<string> bestaandeTabellen = geefTabellenLowercase();
            try {
                TestConnectie.Open();

                if (bestaandeTabellen.Contains(tabelnaam.ToLower())) {

                    string query = string.Format(
                            "INSERT INTO {0} ({1}) VALUES ({2})",
                            tabelnaam,
                            string.Join(",", data[0].Keys.ToArray()),
                            string.Join(",", data[0].Keys.Select(keyFormatted).ToArray())
                        );

                    SqlCommand cmd = new SqlCommand(query, TestConnectie);

                    foreach (KeyValuePair<string, object> item in data[0]) {
                        SqlParameter param = new SqlParameter(keyFormatted(item.Key), SqlHelper.GetDbType(item.Value.GetType()));
                        cmd.Parameters.Add(param);
                    }

                    foreach (Dictionary<string, object> entry in data) {
                        foreach(KeyValuePair<string,object> item in entry) {
                            cmd.Parameters[keyFormatted(item.Key)].Value = item.Value;
                        }

                        cmd.ExecuteNonQuery();
                    }

                } else {
                    return false;
                }

                return true;

            } catch { throw; } finally { TestConnectie.Close();  }
        }
    }
}
