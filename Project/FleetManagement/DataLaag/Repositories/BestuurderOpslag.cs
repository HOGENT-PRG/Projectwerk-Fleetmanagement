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

namespace DataLaag.Repositories
{
    public class BestuurderOpslag : IBestuurderOpslag
    {
        private SqlConnection _connector { get; set; }

        public void ZetConnectionString(string connString)
        {
            _connector = connString.Length > 5 ? new SqlConnection(connString) : throw new BestuurderOpslagException("Connectiestring moet langer zijn dan 5 karakters.");
        }

        public Bestuurder geefBestuurderDetail(int id)
        {
            throw new NotImplementedException(); //enum cast rijbewijssoort
        }

        public IEnumerable<Bestuurder> geefBestuurders()
        {
            List<Bestuurder> geselecteerdeBestuurders = new List<Bestuurder>();

            using (SqlCommand command = _connector.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Bestuurder;";
                _connector.Open();
                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        // TODO
                        // de objecten dienen hier aangemaakt te worden en in de lijst geselecteerdeBestuurders gestoken te worden
                    }

                    return geselecteerdeBestuurders;
                }
                catch (Exception e)
                {
                    throw new BestuurderOpslagException(e.Message);
                }
                finally
                {
                    _connector.Close();
                }
            }
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

        // eventueel gebruiken voor TableMap, indien het een goed idee is

        public IEnumerable<string> geefBestuurderProperties()
        {
            PropertyInfo[] myPropertyInfo = typeof(Voertuig).GetProperties();
            List<string> props = new List<string>();

            for (int i = 0; i < myPropertyInfo.Length; i++)
            {
                props.Add(myPropertyInfo[i].Name);
            }

            return props;
        }
    }
}
