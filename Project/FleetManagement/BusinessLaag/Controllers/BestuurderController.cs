using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLaag.Interfaces;

namespace BusinessLaag.Controllers
{
    public class BestuurderController : IBestuurderController
    {
        private FleetManager FleetManager;
        public BestuurderController(FleetManager fleetmanager)
        {
            FleetManager = fleetmanager;
        }

        public Bestuurder fetchBestuurderDetail(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> fetchBestuurderProperties()
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
    }
}
