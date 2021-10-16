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
    public class VoertuigOpslag : IVoertuigOpslag
    {
        private SqlConnection _connector { get; set; }

        public void ZetConnectionString(string connString)
        {
            _connector = connString.Length > 5 ? new SqlConnection(connString) : throw new VoertuigOpslagException("Connectiestring moet langer zijn dan 5 karakters.");
        }

        public Voertuig geefVoertuigDetail(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Voertuig> geefVoertuigen()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> geefVoertuigProperties()
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
