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
using DataLaag.Repositories;

namespace DataLaag
{
    public class RepositoryManager
    {
        // wellicht is het zinvol om connection string / flags uit bestand te lezen (config file) -> zie opdracht
        // untested

        public string ConnectionString { get; init; }
        public bool ConnectionSuccessful { get; init; }

        public BestuurderRepository BestuurderRepository = new();
        public TankkaartRepository TankkaartRepository = new();
        public VoertuigRepository VoertuigRepository = new();

        public RepositoryManager(bool? truncateTablesOnStartup, bool? insertMockData, string connectionString=null)
        {
            ConnectionString = connectionString ?? @"Data Source =.\SQLEXPRESS;Initial Catalog = FleetManagement; Integrated Security = True";
            StartupSequence Initializer = new(truncateTablesOnStartup, insertMockData, ConnectionString);
            ConnectionSuccessful = Initializer.ConnectionSuccessful;
        }


    }
}
