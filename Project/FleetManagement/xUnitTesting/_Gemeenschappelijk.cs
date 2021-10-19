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

        public static Dictionary<string, string> tabeldatasources = new Dictionary<string, string>{ 
            {"", ""}
        };

        public static TestDatabankConfigureerder TestDatabankConfigureerder = new TestDatabankConfigureerder(tabeldatasources);
        public static FleetManager _testFleetManager = new(new VoertuigOpslag(), new BestuurderOpslag(), 
                                                          new TankkaartOpslag(), TestDatabankConfigureerder);
    }
}
