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
        /* Startup */
        /** Laat toe om tijdens het opstarten van de applicatie een sequentie te doorlopen, waaronder db connectie test, legen van tabellen en toevoegen van fake data aan tabellen (conditionele executie) */
        public IStartupSequence StartupSequence { get; private set; }

        /* Repositories van DAL */
        /** Stelt functies van de data laag beschikbaar aan de Managers. 
            De visibiliteit van de repositories staat op private, elke Manager
            krijgt bij instantiering de correcte repository mee en kan dus niet
            per ongeluk de taken van een andere Manager overnemen.
         */
        private IVoertuigRepository VoertuigRepository { get; set; }
        private IBestuurderRepository BestuurderRepository { get; set; }
        private ITankkaartRepository TankkaartRepository { get; set; }

        public IDatabankConfigureerder DatabankConfigureerder { get; private set; }

        /* Managers uit domeinlaag */
        /** Deze roepen functies aan van de ...Repository klassen en handhaven de domeinregels */
        public VoertuigManager VoertuigManager { get; private set; }
        public BestuurderManager BestuurderManager { get; private set; }
        public TankkaartManager TankkaartManager { get; private set; }

        public FleetManager(IVoertuigRepository voertuigRepo, IBestuurderRepository bestuurderRepo, ITankkaartRepository tankkaartRepo, IDatabankConfigureerder databankConfig, string connectionString)
        {
            DatabankConfigureerder = databankConfig;

            VoertuigRepository = voertuigRepo;
            BestuurderRepository = bestuurderRepo;
            TankkaartRepository = tankkaartRepo;

            VoertuigRepository.ZetConnectionString(connectionString);
            BestuurderRepository.ZetConnectionString(connectionString);
            TankkaartRepository.ZetConnectionString(connectionString);

            /* De fleetmanager wordt meegegeven zodat zij elkaar niet tig maal hoeven te importeren.
                Tevens geeft het de presentatielaag toegang tot alle Managers op 1 centraal punt.
                Daarnaast wordt de correcte repository meegegeven welke binnen de klasse nodig is.
            */
            VoertuigManager = new(this, VoertuigRepository);
            BestuurderManager = new(this, BestuurderRepository);
            TankkaartManager = new(this, TankkaartRepository);

        }
    }
}
