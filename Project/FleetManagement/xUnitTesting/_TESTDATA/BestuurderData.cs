using BusinessLaag.Model;
using BusinessLaag.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xUnitTesting._TESTDATA {
    internal static class BestuurderData {
        // Beheert relaties met Tankkaart, Voertuig en Adres

        private static Random random = new Random();

        // private List<Dictionary<string, object>> _bestuurdersMetRelaties = new();

        private static string _randString(int minLengte=3, int maxLengte=69, bool bevatNummers=false) {
            string chars = " ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            if (bevatNummers) chars += "0123456789";
            int lengte = random.Next(minLengte, maxLengte);
            return new string(Enumerable.Repeat(chars, lengte) .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private static Dictionary<string, object> _bestuurderNaarDict(Bestuurder b) {
            return new Dictionary<string, object>() {
                {"Naam", b.Naam},
                {"Voornaam", b.Voornaam},
                {"Geboortedatum", b.GeboorteDatum},
                {"AdresId", b.Adres?.Id },
                {"Rijksregisternummer", b.RijksRegisterNummer},
                {"Rijbewijssoort", b.RijbewijsSoort.ToString()},
                {"VoertuigId", b.Voertuig?.Id },
                {"TankkaartId", b.Tankkaart?.Id }
            };
        }

        private static Bestuurder _genereerBestuurderZonderRelaties() {
            return new Bestuurder(null, _randString(), _randString(), null, new DateTime(random.Next(DateTime.Now.Year - 70, DateTime.Now.Year - 1), 1, 1), "90020199902", RijbewijsSoort.B, null, null);
        }

        // ---------------------

        public static List<Dictionary<string, object>> geefBestuurdersZonderRelaties(int aantal) {
            List<Dictionary<string, object>> res = new();
            foreach(var i in Enumerable.Range(1, aantal)) {
                res.Add(_bestuurderNaarDict(_genereerBestuurderZonderRelaties()));
            }
            return res;
        }

    }
}
