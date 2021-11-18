using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLaag.Model;
using BusinessLaag;
using BusinessLaag.Exceptions;
using BusinessLaag.Interfaces;
using System.Reflection;
using System.Data.SqlClient;
using DataLaag.Helpers;

namespace DataLaag.Repositories
{
    public sealed class BestuurderOpslag : IBestuurderOpslag
    {
        private SqlConnection _connector { get; set; }

        public void ZetConnectionString(string connString)
        {
            _connector = connString.Length > 5 ? new SqlConnection(connString) : throw new BestuurderOpslagException("Connectiestring moet langer zijn dan 5 karakters.");
        }

        // -- Create
        public int voegBestuurderToe(Bestuurder bestuurder) {
            OproepControleur.ControleerOproeperGemachtigd();

            throw new NotImplementedException();
        }

        // -- Read
        public List<Bestuurder> geefBestuurders() {
            throw new NotImplementedException();
        }

        public List<Bestuurder> zoekBestuurders() {
            throw new NotImplementedException();
        }

        public Bestuurder geefBestuurderDetail(int id) {
            throw new NotImplementedException(); //enum cast rijbewijssoort
        }

        // -- Update
        public void updateBestuurder(Bestuurder bestuurder) {
            OproepControleur.ControleerOproeperGemachtigd();


            throw new NotImplementedException();
        }

        // -- Delete

        public void verwijderBestuurder(Bestuurder bestuurder)
        {
            OproepControleur.ControleerOproeperGemachtigd();

            throw new NotImplementedException();
        }


	}
}
