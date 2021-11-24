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
using DataLaag.Helpers;

namespace xUnitTesting
{
    public class TestDatabankConfigureerder : DatabankConfigureerder, ITestDatabankConfigureerder {

        internal SqlConnection TestMasterConnectie { get { return MasterConnectie; } }
        internal SqlConnection TestConnectie { get { return ProductieConnectie; } }

        public TestDatabankConfigureerder(SortedDictionary<string, string>? tabellen=null,
                                          string databanknaam = "FleetManagerTESTING",
                                          string dataSource = @".\SQLEXPRESS",
                                          bool integratedSecurity = true) 
                                          :base(tabellen, databanknaam, dataSource, integratedSecurity){}


        internal bool MaakTabellenAan() {
            _maakOntbrekendeTabellenAan((string)_initialisatieParameters["databanknaam"], TabellenDefault);
            _controleerBestaanTabellen(TabellenDefault.Keys.ToList());
            if (_ontbrekendeTabellen.Count == 0) return true;
            else return false;
        }
        internal bool TruncateTabellen(List<string> tabellen=null)
        {
            if(tabellen is null) { 
                tabellen = ((SortedDictionary<string, string>)InitialisatieParameters["tabellen"]).Keys.ToList() ?? new(); 
            }

            IList<string> bestaandeTabellen = geefTabellenLowercase();
            try {
                TestConnectie.Open();
                foreach (string table in tabellen) {
                    if (bestaandeTabellen.Contains(table.ToLower())) {
                        string query = "TRUNCATE TABLE " + table;
                        SqlCommand cmd = new SqlCommand(query, TestConnectie);
                        cmd.ExecuteNonQuery();
                    }
                }
            } catch { throw; } finally { TestConnectie.Close(); }

            return true;
        }

        Func<string, string> keyFormatted = p => String.Concat("@", p);

        // zonder relaties data inserten
        // data = lijst met dicts waarbij string=column name en object is insert value
        // elke dict hoort, aangezien het 1 tabel betreft, dezelfde keys te bevatten
        internal List<int> VoerDataIn(string tabelnaam, List<Dictionary<string, object>> data) {
            List<int> insertRowIds = new();

            IList<string> bestaandeTabellen = geefTabellenLowercase();
            try {
                TestConnectie.Open();

                if (bestaandeTabellen.Contains(tabelnaam.ToLower())) {

                    string query = string.Format(
                            "INSERT INTO {0} ({1}) VALUES ({2}); SELECT CAST(SCOPE_IDENTITY() AS INT) AS LAST_IDENTITY;",
                            tabelnaam,
                            string.Join(",", data[0].Keys.ToArray()),
                            string.Join(",", data[0].Keys.Select(keyFormatted).ToArray())
                        );

                    SqlCommand cmd = new SqlCommand(query, TestConnectie);

                    foreach(KeyValuePair<string,object?> row in data[0]) {
                        SqlParameter param = new();
                        param.ParameterName = keyFormatted(row.Key);
                        Type t;
                        try {
                            t = row.Value.GetType();
                        } catch {
                            t = new string(" ").GetType();
                        }
                        param.DbType = TypeConverteerder.GeefDbType(t);
                        cmd.Parameters.Add(param);
                    }

                    foreach (Dictionary<string, object> entry in data) {
                        foreach(KeyValuePair<string,object> item in entry) {
                            object val;
                            try {
                                val = item.Value ?? DBNull.Value;
                            } catch {
                                val = DBNull.Value;
                            }
                            cmd.Parameters[keyFormatted(item.Key)].Value = val;
                        }


                        var resultRowId = cmd.ExecuteScalar();

                        if (resultRowId != null && resultRowId != DBNull.Value) {
                            insertRowIds.Add((int)resultRowId);
                        } else {
                            throw new ArgumentNullException("");
                        }
                    }

                }

                return insertRowIds;

            } catch { throw; } finally { TestConnectie.Close();  }
        }
    }
}
