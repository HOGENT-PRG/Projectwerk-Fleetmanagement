using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using BusinessLaag.Model;
using BusinessLaag;
using BusinessLaag.Interfaces;
using BusinessLaag.Exceptions;

namespace DataLaag.Repositories
{
    public class TankkaartRepository : ITankkaartRepository
    {
        private string _connectionString { get; set; }

        public void ZetConnectionString(string connectionString)
        {
            _connectionString = connectionString.Length > 5 ? connectionString : throw new TankkaartRepositoryException("Connection string moet langer zijn dan 5 karakters");
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
