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
    public class Initializer
    {
        public Initializer(bool? truncateTablesOnStartup, bool? insertMockData, string connectionString)
        {
            _attemptDatabaseConnection(connectionString);
            if (!(truncateTablesOnStartup is null or false)) _truncateTablesOnStartup();
            if (!(insertMockData is null or false)) _insertMockData();
        }

        private void _attemptDatabaseConnection(string connectionString)
        {
            // indien connectie maken niet succesvol is
            throw new InitializerException("Er kon geen verbinding gemaakt worden met de databank.\nControleer de connection string en databank status.");
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
