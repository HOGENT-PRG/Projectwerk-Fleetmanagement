using BusinessLaag.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xUnitTesting
{
    public class TestDatabankConfigureerder : ITestDatabankConfigureerder
    {
        private SqlConnection MasterConnectie { get; set; }
        private SqlConnection TestConnectie { get; set; }
        private string MasterConnectieString { get; set; }
        public string ProductieConnectieString { get; private set; }

        private Dictionary<string, object> _initialisatieParameters;

        public bool ConnectieSuccesvol { get; private set; }
        public bool DatabaseBestaat { get; private set; }
        public bool AlleTabellenBestaan { get; private set; }
        public int AantalTabellen { get; private set; }
        public bool SequentieDoorlopen { get; private set; }

        public TestDatabankConfigureerder(Dictionary<string, string> tabellen,
                                     string databanknaam = "FleetManagerTESTING",
                                     string dataSource = @".\SQLEXPRESS",
                                     bool integratedSecurity = true)
        {

            // evt toevoegen relatief pad voor testdata 
            _initialisatieParameters = new Dictionary<string, object>()
            {
                {"databanknaam", databanknaam},
                {"dataSource", dataSource},
                {"integratedSecurity", integratedSecurity},
                { "tabellen", tabellen }
            };
        }
            // Toe te voegen:
            // constructor logica
            // private methodes
            // ---------- haal deze uit DatabankConfigureerder als de bugs er uit zijn, en wanneer testen van de managers
            // ---------- nodig is (na database design en tijdens/na ontwikkelen managers om functionaliteiten te testen)

        public IEnumerable geefInitialisatieParameters()
        {
            return _initialisatieParameters;
        }

        public IList<string> geefTabellen()
        {
            throw new NotImplementedException();
        }

        public void maakTabellenAan()           //hetzelfde pad gebruiken als de reguliere dbconfigureerder waar de 
        {                                       //create table statements zich bevinden: "/DataLaag/_SQL/"
            throw new NotImplementedException();
        }

        public void truncateTabellen()
        {
            throw new NotImplementedException();
        }

        public void verwijderTabellen()
        {
            throw new NotImplementedException();
        }
    }
}
