using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DataLaag.Exceptions;
using BusinessLaag.Model;
using BusinessLaag;
using BusinessLaag.Interfaces;
using BusinessLaag.Exceptions;


namespace DataLaag.Repositories
{
    public class VoertuigRepository : IVoertuigRepository
    {
        private string _connectionString { get;  set; }

        public void ZetConnectionString(string connectionString)
        {
            _connectionString = connectionString.Length > 5 ? connectionString : throw new VoertuigException("Connection string moet langer zijn dan 5 karakters");
        }

        public Voertuig fetchVoertuigDetail(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Voertuig> fetchVoertuigen()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> fetchVoertuigProperties()
        {
            throw new NotImplementedException();
        }

        public void updateVoertuig(Voertuig voertuig)
        {
            throw new NotImplementedException();
        }

        public void verwijderVoertuig(Voertuig voertuig)
        {
            throw new NotImplementedException();
        }

        public void voegVoertuigToe(Voertuig voertuig)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Voertuig> zoekVoertuig()
        {
            throw new NotImplementedException();
        }
    }
}
