using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLaag.Interfaces;
using BusinessLaag.Model;

namespace BusinessLaag.Managers
{
    public class TankkaartManager : ITankkaartManager
    {
        private static FleetManager _fleetManager;
        private ITankkaartOpslag _opslag;

        public TankkaartManager(FleetManager fleetmanager, ITankkaartOpslag repository)
        {
            _fleetManager = fleetmanager;
            _opslag = repository;
        }

        public Tankkaart geefTankkaartDetail(int id)
        {
            // krijgt tankkaart
            // vraagt bestuurder op (if any), voegt deze bij obj
            // return
            throw new NotImplementedException();
        }

        public IEnumerable<Tankkaart> geefTankkaarten()
        {
            // krijgt tankkaarten
            // vraagt bestuurders op en voegt toe bij elke kaart
            // return
            throw new NotImplementedException();
        }

        public bool updateTankkaart(Tankkaart tankkaart)
        {
            // vraagt huidige versie op
            // indien bestuurder wijzigt -> oude bestuurder referentie verwijderen -> continue
            // indien null bestuurder -> nieuwe bestuurder -> continue
            // indien bestuurder -> null bestuurder -> oude bestuurder ref weg -> cont
            // indien niet geupdate : throw
            throw new NotImplementedException();
        }

        public bool verwijderTankkaart(Tankkaart tankkaart)
        {
            // zijn er bestuurders? - vraag op
            // bestuurder referenties naar kaart verwijderen
            // kaart verwijderen
            // indien niet deleted : throw
            throw new NotImplementedException();
        }

        // nog een functie brandstof toevoegen/verwijderen, dmv tussentabel <-------------

        public bool voegTankkaartToe(Tankkaart tankkaart)
        {
            // if kaartnummer bestaat al (vraag tankkaart op met id en check)
            // throw
            // else insert
            // daarna, indien het een bestuurder bevat,
            // update deze bestuurder en tankkaart met referentie naar elkaar
            throw new NotImplementedException();
        }

        // functie veranderblokkeringsstatus

        public IEnumerable<Tankkaart> zoekTankkaarten()
        {
            throw new NotImplementedException();
        }
    }
}
