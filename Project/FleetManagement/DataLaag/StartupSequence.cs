using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLaag.Exceptions;

namespace DataLaag
{
    //niet getest

    // het doel van deze klasse is om:
    // - de connectie te testen dmv de connection string (altijd als de applicatie start)
    // - de databank te legen bij het opstarten indien enabled
    // - mock data in de db te steken indien enabled
    public class StartupSequence
    {
        public bool ConnectionSuccessful { get; init; }
        public bool TruncatedTables { get; private set; }
        public bool InsertedMockData { get; private set; }
        public StartupSequence(bool? truncateTablesOnStartup, bool? insertMockData, string connectionString)
        {
            ConnectionSuccessful = _attemptDatabaseConnection(connectionString);

            if (!(truncateTablesOnStartup is null or false)) _truncateTablesOnStartup();
            else TruncatedTables = false;
            if (!(insertMockData is null or false)) _insertMockData();
            else InsertedMockData = false;
        }

        private bool _attemptDatabaseConnection(string connectionString)
        {
            // indien succesvol
            return true;
            // indien connectie maken niet succesvol is
            return false;
        }

        private void _truncateTablesOnStartup()
        {
            throw new NotImplementedException();
        }

        private void _insertMockData()
        {
            throw new NotImplementedException();
        }

    }
}
