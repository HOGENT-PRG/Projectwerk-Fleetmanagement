using System;
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

        public VoertuigManager(FleetManager fleetmanager)
        {
            _fleetManager = fleetmanager;
        }

        public Voertuig fetchVoertuigDetail(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Voertuig> fetchVoertuigen()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> fetchVoertuigProperties()
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
