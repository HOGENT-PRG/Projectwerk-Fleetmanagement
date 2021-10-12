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
        private IBestuurderRepository _repository;

        public BestuurderManager(FleetManager fleetmanager, IBestuurderRepository repository)
        {
            _fleetManager = fleetmanager;
            _repository = repository;
        }

        public Bestuurder fetchBestuurderDetail(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Bestuurder> fetchBestuurders()
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

        // eventueel gebruiken voor TableMap, indien het een goed idee is

        public IEnumerable<string> fetchBestuurderProperties()
        {
            throw new NotImplementedException();
        }
    }
}
