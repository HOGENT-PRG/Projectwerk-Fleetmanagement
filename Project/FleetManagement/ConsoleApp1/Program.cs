using System;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DataLaag.Repositories;
using BusinessLaag.Model;

namespace ConsoleApp1
{
   public class Program
    {
        static void Main(string[] args)
        {
            VoertuigOpslag vrtp = new VoertuigOpslag();
            vrtp.ZetConnectionString(@"Data Source=LAPTOP-9HCGFJ8O;Initial Catalog=FleetManager;Integrated Security=True");
            Voertuig v = new Voertuig(2, BusinessLaag.Model.Enum.Merk.Bentley, "Corvette", "1xae153", BusinessLaag.Model.Enum.VoertuigBrandstof.Elektrisch, BusinessLaag.Model.Enum.Voertuigsoort.bestelwagen, "F2E15D48-9A38-411", "Groen", 5);
            vrtp.UpdateVoertuig(v);
        }
    }
}
