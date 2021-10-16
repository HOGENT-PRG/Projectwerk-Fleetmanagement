﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLaag.Interfaces;

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
            throw new NotImplementedException();
        }

        public IEnumerable<Tankkaart> geefTankkaarten()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> geefTankkaartProperties()
        {
            throw new NotImplementedException();
        }

        public void updateTankkaart(Tankkaart tankkaart)
        {
            throw new NotImplementedException();
        }

        public void verwijderTankkaart(Tankkaart tankkaart)
        {
            throw new NotImplementedException();
        }

        public void voegTankkaartToe(Tankkaart tankkaart)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Tankkaart> zoekTankkaarten()
        {
            throw new NotImplementedException();
        }
    }
}
