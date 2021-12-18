using BusinessLaag.Interfaces;
using BusinessLaag.Managers;

namespace BusinessLaag
{
    public class FleetManager
    {
        // Checkt connectie met databank, maakt indien nodig databank en tabellen aan
        // Is public want bevat informatie (connectie succesvol etc.) voor de Presentatielaag
        public IDatabankConfigureerder DatabankConfigureerder { get; private set; }

        /* Managers uit domeinlaag */
        /* Deze roepen functies aan van de ...Repository klassen en van andere Managers en handhaven de domeinregels */
        public VoertuigManager VoertuigManager { get; private set; }
        public BestuurderManager BestuurderManager { get; private set; }
        public TankkaartManager TankkaartManager { get; private set; }


        public FleetManager(IVoertuigOpslag voertuigRepo, IBestuurderOpslag bestuurderRepo, ITankkaartOpslag tankkaartRepo, IDatabankConfigureerder databankConfig)
        {
            // Stelt db info (voor weergave door presentatie laag) en connection string ter beschikking
            DatabankConfigureerder = databankConfig;

            /** Repositories van DAL
                    Stelt functies van de data laag beschikbaar aan de Managers. 
                    Lokale property is niet nodig, elke Manager
                    krijgt bij instantiering de correcte repository mee en kan dus niet
                    per ongeluk de taken van een andere Manager overnemen.
                                                                            **/

            // De SqlConnection voor gebruik in productie wordt aangemaakt door de repos dmv connstring
            voertuigRepo.ZetConnectionString(DatabankConfigureerder.ProductieConnectieString);
            bestuurderRepo.ZetConnectionString(DatabankConfigureerder.ProductieConnectieString);
            tankkaartRepo.ZetConnectionString(DatabankConfigureerder.ProductieConnectieString);

            /* De fleetmanager wordt meegegeven zodat zij elkaar niet tig maal hoeven te importeren en
                het geeft de presentatielaag toegang tot alle Managers op 1 centraal punt.
                Daarnaast wordt de correcte repository meegegeven welke binnen de klasse nodig is.
            */
            VoertuigManager = new(this, voertuigRepo);
            BestuurderManager = new(this, bestuurderRepo);
            TankkaartManager = new(this, tankkaartRepo);

        }
    }
}
