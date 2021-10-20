using BusinessLaag;
using BusinessLaag.Managers;
using BusinessLaag.Interfaces;
using DataLaag.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xUnitTesting
{
    public static class Gemeenschappelijk
    {
        // Tabelnaam : url , de tabelnaam aan te maken (case sensitive) en de databron die er voor gebruikt wordt
        // voorbeeld:
        // {"bestelling", "https://pastebin.com/raw/ZnbbC6vu" },
        // {"klant", "https://pastebin.com/raw/KMVXnGRG" }

        public static Dictionary<string, string> tabeldatasources = new Dictionary<string, string>{ 
            {"", ""}
        };

        public static TestDatabankConfigureerder TestDatabankConfigureerder = new TestDatabankConfigureerder(tabeldatasources);
        public static FleetManager _testFleetManager = new(new VoertuigOpslag(), new BestuurderOpslag(), 
                                                          new TankkaartOpslag(), TestDatabankConfigureerder);
    }
}
