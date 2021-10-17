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
        // In te vullen - de namen van de bestanden waar de create table
        // statements in staan, zonder .sql extensie, zie:
        // ./DataLaag/_SQL
        public static List<string> tabelfilenamen = new List<string>() { };

        public static TestDatabankConfigureerder TestDatabankConfigureerder = new TestDatabankConfigureerder(tabelfilenamen);
        public static FleetManager _testFleetManager = new(new VoertuigOpslag(), new BestuurderOpslag(), 
                                                          new TankkaartOpslag(), TestDatabankConfigureerder);
    }
}
