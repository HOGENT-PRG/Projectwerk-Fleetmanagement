using BusinessLaag;
using DataLaag;
using DataLaag.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFApp.Interfaces;

// De Businesscommuniceerder heeft als enigste klasse dependency op de business laag.
// In het geval dat er een API gebruikt wordt zal deze de verantwoordelijkheid voor het
// beheren van de dependency en het aanmaken van de FleetManager op zich moeten nemen.

namespace WPFApp.Model.Communiceerders {
    internal class BusinessCommuniceerder : ICommuniceer {
        private FleetManager _fleetManager;
        public BusinessCommuniceerder() {
            _fleetManager = new(new VoertuigOpslag(), new BestuurderOpslag(), new TankkaartOpslag(), new DatabankConfigureerder(null));
        }
    }
}
