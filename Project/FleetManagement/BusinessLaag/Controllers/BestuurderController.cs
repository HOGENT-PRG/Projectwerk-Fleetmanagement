using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using BusinessLaag.Interfaces;

namespace BusinessLaag.Controllers
{
    public class BestuurderController : IBestuurderController
    {
        private static FleetManager FleetManager;
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
            //PropertyInfo[] myPropertyInfo = Type.GetType("BusinessLaag.Model.Voertuig,BusinessLaag").GetProperties();
            PropertyInfo[] myPropertyInfo = typeof(Voertuig).GetProperties();
            List<string> props = new List<string>();

            for (int i = 0; i < myPropertyInfo.Length; i++)
            {
                props.Add(myPropertyInfo[i].Name);
            }

            return props;
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
