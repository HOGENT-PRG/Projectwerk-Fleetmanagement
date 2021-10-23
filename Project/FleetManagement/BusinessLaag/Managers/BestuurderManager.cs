using System;
using System.Collections.Generic;
using BusinessLaag.Interfaces;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BusinessLaag.Model;

namespace BusinessLaag.Managers
{
    public class BestuurderManager : IBestuurderManager {
        private static FleetManager _fleetManager;
        private IBestuurderOpslag _opslag;

        public BestuurderManager(FleetManager fleetmanager, IBestuurderOpslag repository) {
            _fleetManager = fleetmanager;
            _opslag = repository;
        }

        public Bestuurder geefBestuurderDetail(int id) {
            // indien een adres hem refereert
            // indien een voertuig hem refereert
            // indien een tankkaart hem refereert
            // refereert hij ze? (allebei checken voor redundantie)
            // opvragen en obj toevoegen
            // return bestuurder
            throw new NotImplementedException();
        }

        public IEnumerable<Bestuurder> geefBestuurders() {
            // ontvang bestuurders
            // voor elke bestuurder
            // indien een adres hem refereert
            // indien een voertuig hem refereert
            // indien een tankkaart hem refereert
            // refereert hij ze tevens ook? (allebei checken voor redundantie) -> indien mismatch conflict oplossen
            // opvragen refs en obj toevoegen
            // return bestuurder
            throw new NotImplementedException();
        }

        public bool updateBestuurder(Bestuurder bestuurder) {
            // indien adres wijzigt: verwijder oud adres, insert new adres if geen adres.id
            // indien voertuig/tankkaart wijzigt: referenties wijzigen
            // 
            throw new NotImplementedException();
        }

        public bool verwijderBestuurder(Bestuurder bestuurder) {
            // referenties checken en behandelen (lees bovenstaanden), indien ok verwijderen
            throw new NotImplementedException();
        }

        public bool voegBestuurderToe(Bestuurder bestuurder) {
            //Bestuurder Bestuurder = (Bestuurder)bestuurder;
            // indien rijksreg reeds bestaat: throw
            // eventueel inserten van adres,voertuig,tankkaart en deze opnemen in bestuurder
            // doorgaan met insert
            throw new NotImplementedException();
        }

        public IEnumerable<Bestuurder> zoekBestuurders() {
            throw new NotImplementedException();
        }

        public IEnumerable<string> geefBestuurderProperties() {
            throw new NotImplementedException(); //GetProperties
        }
    }
}
