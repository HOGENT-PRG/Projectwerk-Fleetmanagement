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

        public void ZetConnectionString(string connString) {
            _connector = connString.Length > 5 ? new SqlConnection(connString) : throw new VoertuigOpslagException("Connectiestring moet langer zijn dan 5 karakters.");
        }

        // -- create
        public Voertuig VoegVoertuigToe(Voertuig voertuig) {
            throw new NotImplementedException();
        }

        // -- read
        public KeyValuePair<int?, Voertuig> GeefVoertuigDetail(int id) {
            throw new NotImplementedException();
        }

        public Dictionary<int?, Voertuig> GeefVoertuigen() {
            throw new NotImplementedException();
        }

        public Voertuig ZoekVoertuig(string kolomNaam, string chassisnummer) {
            throw new NotImplementedException();
        }

        // -- update
        public void UpdateVoertuig(Voertuig voertuig) {
            throw new NotImplementedException();
        }

        // -- delete
        public void VerwijderVoertuig(Voertuig voertuig) {
            throw new NotImplementedException();
        }

    }
}
