using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLaag.Controllers
{
    public class VoertuigController
    {
        private FleetManager FleetManager;
        public VoertuigController(FleetManager fleetmanager)
        {
            FleetManager = fleetmanager;
        }
    }
}
