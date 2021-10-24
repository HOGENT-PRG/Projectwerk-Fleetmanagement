using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using xUnitTesting._TESTDATA;

namespace xUnitTesting.Datalaag {

    [Collection("DisableParallelTests")]
    public class TestDatabankConfigureerderTests {
        // Deze unit test klasse test zowel functies van DatabankConfigureerder als van de
        // overervende / uitbreidende inhouse klasse TestDatabankConfigureerder
        TestDatabankConfigureerder _beheerDatabank;

        public TestDatabankConfigureerderTests() {
            _beheerDatabank = Gemeenschappelijk.TestDatabankConfigureerder;
        }

        //Default db = FleetManagerTESTING

        #region Private helper functies
        private Int32 _geefAantalRowsVoorTabel(string tabelnaam) {
            try {
                _beheerDatabank.TestMasterConnectie.Open();

                string query = $"select count(*) from {(string)_beheerDatabank.InitialisatieParameters["databanknaam"]}.dbo.{tabelnaam}";
                SqlCommand cmd = new(query, _beheerDatabank.TestMasterConnectie);

                Int32 hoeveelheid = Convert.ToInt32(cmd.ExecuteScalar());

                return hoeveelheid;

            } catch { throw; } finally { _beheerDatabank.TestMasterConnectie.Close(); }
        }

        private Int32 _geefAantalTabellen() {
            try {
                _beheerDatabank.TestMasterConnectie.Open();

                string query = $"select count(*) from {(string)_beheerDatabank.InitialisatieParameters["databanknaam"]}.sys.tables";
                SqlCommand cmd = new(query, _beheerDatabank.TestMasterConnectie);

                return Convert.ToInt32(cmd.ExecuteScalar());
            } catch { throw; } finally { _beheerDatabank.TestMasterConnectie.Close(); }
        }
        #endregion

        #region Tests
        [Fact]
        public void Test_DatabaseBestaat() {
            try {
                _beheerDatabank.TestMasterConnectie.Open();
                string query = "select count(*) from master.dbo.sysdatabases where name=@database";
                SqlCommand cmd = new(query, _beheerDatabank.TestMasterConnectie);
                cmd.Parameters.Add("@database", System.Data.SqlDbType.NVarChar)
                    .Value = (string)_beheerDatabank.InitialisatieParameters["databanknaam"];

                var resultaat = cmd.ExecuteScalar();
                Assert.Equal(1, resultaat);
            } catch { throw; } finally { _beheerDatabank.TestMasterConnectie.Close(); }
        }

        [Fact]
        public void Test_TabellenBestaan() {
            List<string> bestaandeTabellen = new List<string>();
            try {
                _beheerDatabank.TestConnectie.Open();
                DataTable dt = _beheerDatabank.TestConnectie.GetSchema("Tables");
                foreach (DataRow row in dt.Rows) {
                    string tablename = (string)row[2];
                    bestaandeTabellen.Add(tablename);
                }

                List<string> verwachteBestaandeTabellen = ((SortedDictionary<string, string>)_beheerDatabank.InitialisatieParameters["tabellen"]).Keys.ToList();

                verwachteBestaandeTabellen = verwachteBestaandeTabellen
                                            .ConvertAll(q => q.ToLower())
                                            .OrderBy(q => q)
                                            .ToList();

                bestaandeTabellen = bestaandeTabellen
                                    .ConvertAll(q => q.ToLower())
                                    .OrderBy(q => q)
                                    .ToList();

                Assert.Equal(verwachteBestaandeTabellen, bestaandeTabellen);

            } catch { throw; } finally { _beheerDatabank.TestConnectie.Close(); }
        }

        [Fact]
        public void Test_InsertWerkt() {
            List<int> rowIds =_beheerDatabank.VoerDataIn("Bestuurder", BestuurderData.geefBestuurdersDict(10));

                try {
                    _beheerDatabank.TestMasterConnectie.Open();

                    foreach (int rowId in rowIds) {
                        string query = $"select count(*) from {(string)_beheerDatabank.InitialisatieParameters["databanknaam"]}.dbo.Bestuurder where Id=@Id";
                        SqlCommand cmd = new(query, _beheerDatabank.TestMasterConnectie);
                        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = rowId;

                        Int32 hoeveelheid = Convert.ToInt32(cmd.ExecuteScalar());

                        Assert.Equal(1, hoeveelheid);
                    }

                } catch { throw; } finally { _beheerDatabank.TestMasterConnectie.Close(); }
        }

        [Fact]
        public void Test_TruncateWerkt() {
            if (_geefAantalRowsVoorTabel("Bestuurder") == 0) {
                Test_InsertWerkt();
            }

            if (_geefAantalRowsVoorTabel("Bestuurder") == 0) {
                throw new ArgumentOutOfRangeException("Databank lijkt data niet op te slaan in tabel Bestuurder.");
            }

            _beheerDatabank.TruncateTabellen(new List<string>() { "Bestuurder" });

            Assert.Equal(0, _geefAantalRowsVoorTabel("Bestuurder"));
        }

        [Fact]
        public void Test_MaakTabellen() {
            SortedDictionary<string, string> n = new ((SortedDictionary<string, string>)_beheerDatabank.InitialisatieParameters["tabellen"]);
            IEnumerable<string> tabellen = n.Keys.Reverse();

            _beheerDatabank.MaakTabellenAan();

            Assert.Equal(tabellen.Count(), _geefAantalTabellen());
        }

        [Fact]
        public void Test_ParametersCorrect() {
            Assert.NotNull(_beheerDatabank.InitialisatieParameters["tabellen"]);
            Assert.NotNull(_beheerDatabank.InitialisatieParameters["databanknaam"]);
            Assert.NotNull(_beheerDatabank.InitialisatieParameters["dataSource"]);
            Assert.NotNull(_beheerDatabank.InitialisatieParameters["integratedSecurity"]);

            Assert.Equal(_geefAantalTabellen(), _beheerDatabank.AantalTabellen);
        }

        #endregion
    }
}
