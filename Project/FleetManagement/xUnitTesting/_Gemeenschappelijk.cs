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
        public static TestDatabankConfigureerder TestDatabankConfigureerder = new(null);
        public static FleetManager _testFleetManager = new(new VoertuigOpslag(), new BestuurderOpslag(), 
                                                          new TankkaartOpslag(), TestDatabankConfigureerder);
    }
}
