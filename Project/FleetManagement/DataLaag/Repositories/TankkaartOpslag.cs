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
    public class TankkaartOpslag : ITankkaartOpslag
    {
        private SqlConnection _connector { get; set; }

        public void ZetConnectionString(string connString)
        {
            _connector = connString.Length > 5 ? new SqlConnection(connString) : throw new TankkaartOpslagException("Connectiestring moet langer zijn dan 5 karakters.");
        }

     

        public IEnumerable<string> geefTankkaartProperties()
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

        public List<KeyValuePair<int?, Tankkaart>> GeefTankkaarten()
        {
            throw new NotImplementedException();
        }

        public KeyValuePair<int?, Tankkaart> GeefTankkaartDetail(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Tankkaart> zoekTankkaarten(string kolom, string waarde)
        {
            throw new NotImplementedException();
        }

        public int voegTankkaartToe(int? id)
        {
            throw new NotImplementedException();
        }
    }
}
