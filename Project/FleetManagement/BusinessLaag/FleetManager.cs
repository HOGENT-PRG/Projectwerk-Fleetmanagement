using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLaag.Controllers;
using DataLaag;

namespace BusinessLaag
{
    public class FleetManager
    {
        public VoertuigController VoertuigController;
        public BestuurderController BestuurderController;
        public TankkaartController TankkaartController;
        public DataManager DataManager;
        public FleetManager()
        {
            VoertuigController = new VoertuigController(this);
            BestuurderController = new BestuurderController(this);
            TankkaartController = new TankkaartController(this);
            DataManager = new DataManager();
        }
    }
}
