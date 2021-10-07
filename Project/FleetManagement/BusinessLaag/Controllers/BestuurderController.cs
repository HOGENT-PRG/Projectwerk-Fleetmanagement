using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLaag.Interfaces;

namespace BusinessLaag.Controllers
{
    public class BestuurderController
    {
        private FleetManager FleetManager;
        public BestuurderController(FleetManager fleetmanager)
        {
            FleetManager = fleetmanager;
        }
    }
}
