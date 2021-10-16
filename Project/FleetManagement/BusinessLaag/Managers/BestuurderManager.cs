using System;
using System.Collections.Generic;
using BusinessLaag.Interfaces;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLaag.Managers
{
    public class BestuurderManager : IBestuurderManager
    {
        private static FleetManager _fleetManager;
        private IBestuurderOpslag _opslag;

        public BestuurderManager(FleetManager fleetmanager, IBestuurderOpslag repository)
        {
            _fleetManager = fleetmanager;
            _opslag = repository;
        }

        public Bestuurder geefBestuurderDetail(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Bestuurder> geefBestuurders()
        {
            throw new NotImplementedException();
        }

        public void updateBestuurder(Bestuurder bestuurder)
        {
            throw new NotImplementedException();
        }

        public void verwijderBestuurder(Bestuurder bestuurder)
        {
            throw new NotImplementedException();
        }

        public void voegBestuurderToe(Bestuurder bestuurder)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Bestuurder> zoekBestuurders()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> geefBestuurderProperties()
        {
            throw new NotImplementedException();
        }
    }
}
