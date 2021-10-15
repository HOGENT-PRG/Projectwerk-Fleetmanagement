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
    public class BestuurderRepository : IBestuurderRepository
    {
        private string _connectionString { get; set; }

        public void ZetConnectionString(string connectionString)
        {
            _connectionString = connectionString.Length > 5 ? connectionString : throw new BestuurderRepositoryException("Connection string moet langer zijn dan 5 karakters");
        }

        public Bestuurder fetchBestuurderDetail(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Bestuurder> fetchBestuurders()
        {
            List<Bestuurder> geselecteerdeBestuurders = new List<Bestuurder>();

            SqlConnection connection = new SqlConnection(_connectionString);

            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Bestuurder;";
                connection.Open();
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
                    throw new BestuurderRepositoryException(e.Message);
                }
                finally
                {
                    connection.Close();
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

        public IEnumerable<string> fetchBestuurderProperties()
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
