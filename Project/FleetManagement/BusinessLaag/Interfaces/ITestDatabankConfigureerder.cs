using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLaag.Interfaces
{
    public interface ITestDatabankConfigureerder : IDatabankConfigureerder
    {
        // Inherited:
        //   geefTabellen();
        void maakTabellenAan(Dictionary<string, string> tabellen);
        void truncateTabellen(List<string> tabellen);
        void verwijderTabellen(List<string> tabellen);
        void voerDataIn(Dictionary<string, object> data);
        IEnumerable geefInitialisatieParameters();
    }
}
