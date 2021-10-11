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
        public VoertuigController VoertuigController { get; private set; }
        public BestuurderController BestuurderController { get; private set; }
        public TankkaartController TankkaartController { get; private set; }
        public DataManager DataManager { get; private set; }
        public FleetManager()
        {
            VoertuigController = new VoertuigController(this);
            BestuurderController = new BestuurderController(this);
            TankkaartController = new TankkaartController(this);
            DataManager = new DataManager(truncateTablesOnStartup:true,insertMockData:true);
        }
    }
}
