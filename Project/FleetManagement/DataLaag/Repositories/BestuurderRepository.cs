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

namespace DataLaag.Repositories
{
    public class BestuurderRepository
    {

        public IEnumerable<Bestuurder> SelecteerBestuurders()
        {
            List<Bestuurder> geselecteerdeBestuurders = new();

            SqlConnection connection = new SqlConnection(ConnectionString);

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
                    throw new RepositoryManagerException(e.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
