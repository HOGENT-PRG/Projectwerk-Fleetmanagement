using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLaag.Controllers
{
    public class TankkaartController
    {
        private FleetManager FleetManager;
        public TankkaartController(FleetManager fleetmanager)
        {
            FleetManager = fleetmanager;
        }
    }
}
