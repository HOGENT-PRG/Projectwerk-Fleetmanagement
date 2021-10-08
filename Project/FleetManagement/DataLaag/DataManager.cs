﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DataLaag.Exceptions;

namespace DataLaag
{
    public class DataManager
    {
        // wellicht is het zinvol om connection string / flags uit bestand te lezen (config file) -> zie opdracht
        // niet getest, wellicht zinvol om onder te verdelen in 3 klasses in de trend van de businesslaag sinds functies vrij groot zijn
        private string _connectionString;
        public DataManager(bool? truncateTablesOnStartup, bool? insertMockData)
        {
            _connectionString = new Configuraties().ConnectionString;
            Initializer Initializer = new Initializer(truncateTablesOnStartup, insertMockData, _connectionString);
        }

        public List<Dictionary<string, object>> SelecteerBestuurders()
        {
            List<Dictionary<string, object>> geselecteerdeBestuurders = new List<Dictionary<string, object>>();

            SqlConnection connection = new SqlConnection(_connectionString);

            using(SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Bestuurder;";
                connection.Open();
                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        geselecteerdeBestuurders.Add(Enumerable.Range(0, reader.FieldCount).ToDictionary(reader.GetName, reader.GetValue));
                    }
                    return geselecteerdeBestuurders;
                } catch (Exception e)
                {
                    throw new DataManagerException(e.Message);
                } finally
                {
                    connection.Close();
                }
            }
        }
    }
}
