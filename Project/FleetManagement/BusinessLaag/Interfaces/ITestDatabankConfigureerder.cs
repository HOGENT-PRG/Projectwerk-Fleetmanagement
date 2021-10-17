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
        void maakTabellenAan();
        void truncateTabellen();
        void verwijderTabellen();
        IEnumerable geefInitialisatieParameters();
    }
}
