using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLaag.Interfaces;
using BusinessLaag.Managers;

namespace BusinessLaag
{
    public class FleetManager
    {
        // Checkt connectie met databank, maakt indien nodig databank en tabellen aan
        // Is public want bevat informatie (connectie succesvol etc.) voor de Presentatielaag
        public IDatabankConfigureerder DatabankConfigureerder { get; private set; }


        /* Repositories van DAL */
        /** Stelt functies van de data laag beschikbaar aan de Managers. 
            De visibiliteit van de repositories staat op private, elke Manager
            krijgt bij instantiering de correcte repository mee en kan dus niet
            per ongeluk de taken van een andere Manager overnemen.
         */

        private IVoertuigOpslag VoertuigOpslag { get; set; }
        private IBestuurderOpslag BestuurderOpslag { get; set; }
        private ITankkaartOpslag TankkaartOpslag { get; set; }


        /* Managers uit domeinlaag */
        /** Deze roepen functies aan van de ...Repository klassen en van andere Managers en handhaven de domeinregels */
        public VoertuigManager VoertuigManager { get; private set; }
        public BestuurderManager BestuurderManager { get; private set; }
        public TankkaartManager TankkaartManager { get; private set; }


        public FleetManager(IVoertuigOpslag voertuigRepo, IBestuurderOpslag bestuurderRepo, ITankkaartOpslag tankkaartRepo, IDatabankConfigureerder databankConfig)
        {
            DatabankConfigureerder = databankConfig;

            VoertuigOpslag = voertuigRepo;
            BestuurderOpslag = bestuurderRepo;
            TankkaartOpslag = tankkaartRepo;

            // De SqlConnection voor gebruik in productie wordt ingesteld
            VoertuigOpslag.ZetConnectionString(DatabankConfigureerder.ProductieConnectieString);
            BestuurderOpslag.ZetConnectionString(DatabankConfigureerder.ProductieConnectieString);
            TankkaartOpslag.ZetConnectionString(DatabankConfigureerder.ProductieConnectieString);

            /* De fleetmanager wordt meegegeven zodat zij elkaar niet tig maal hoeven te importeren.
                Tevens geeft het de presentatielaag toegang tot alle Managers op 1 centraal punt.
                Daarnaast wordt de correcte repository meegegeven welke binnen de klasse nodig is.
            */
            VoertuigManager = new(this, VoertuigOpslag);
            BestuurderManager = new(this, BestuurderOpslag);
            TankkaartManager = new(this, TankkaartOpslag);

        }
    }
}
