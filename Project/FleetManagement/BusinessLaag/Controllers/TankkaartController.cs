using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLaag.Interfaces;

namespace BusinessLaag.Controllers
{
    public class TankkaartController: ITankkaartController
    {
        private FleetManager FleetManager;
        public TankkaartController(FleetManager fleetmanager)
        {
            FleetManager = fleetmanager;
        }

        public Tankkaart fetchTankkaartDetail(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Tankkaart> fetchTankkaarten()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> fetchTankkaartProperties()
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
