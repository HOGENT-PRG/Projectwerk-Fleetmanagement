﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLaag.Interfaces;

namespace BusinessLaag.Managers
{
    public class VoertuigManager : IVoertuigManager
    {
        private static FleetManager _fleetManager;
        private IVoertuigOpslag _opslag;

        public VoertuigManager(FleetManager fleetmanager, IVoertuigOpslag repository)
        {
            _fleetManager = fleetmanager;
            _opslag = repository;
        }

        public Voertuig geefVoertuigDetail(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Voertuig> geefVoertuigen()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> geefVoertuigProperties()
        {
            throw new NotImplementedException();
        }

        public void updateVoertuig(Voertuig voertuig)
        {
            throw new NotImplementedException();
        }

        public void verwijderVoertuig(Voertuig voertuig)
        {
            throw new NotImplementedException();
        }

        public void voegVoertuigToe(Voertuig voertuig)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Voertuig> zoekVoertuig()
        {
            throw new NotImplementedException();
        }
    }
}
