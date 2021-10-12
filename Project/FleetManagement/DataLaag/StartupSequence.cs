using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLaag.Exceptions;
using BusinessLaag.Interfaces;

namespace DataLaag
{
    //niet getest
    // kan eventueel config bestand uitlezen zoals gevraagd in opdracht

    // het doel van deze klasse is om:
    // - de connectie te testen dmv de connection string (altijd als de applicatie start)
    // - de databank te legen bij het opstarten indien enabled
    // - mock data in de db te steken indien enabled
    public class StartupSequence : IStartupSequence
    {
        public bool HasRan { get; private set; }
        public bool ConnectionSuccessful { get; private set; }
        public bool TruncatedTables { get; private set; }
        public bool InsertedMockData { get; private set; }

        private string _connectionString { get; set; }

        public void Execute(string connectionString, bool truncateTablesOnStartup=false, bool insertMockData=false)
        {
            if (!HasRan)
            {
                _connectionString = connectionString.Length > 5 ? connectionString : throw new StartupSequenceException("Connection string moet minstens 5 karakters lang zijn.");

                _attemptDatabaseConnection();

                if (!(truncateTablesOnStartup is false)) _truncateTablesOnStartup();
                else TruncatedTables = false;
                if (!(insertMockData is false)) _insertMockData();
                else InsertedMockData = false;

                HasRan = true;
            } else
            {
                throw new StartupSequenceException("De startup sequentie heeft reeds plaatsgevonden, je kan deze slechts 1 keer per startup uitvoeren.");
            }
        }

        private void _attemptDatabaseConnection()
        {
            // indien succesvol
            ConnectionSuccessful = true;
            // indien connectie maken niet succesvol is
            ConnectionSuccessful = false;
        }

        private void _truncateTablesOnStartup()
        {
            // indien succesvol:
            TruncatedTables = true;
            // anders:
            TruncatedTables = false;
        }

        private void _insertMockData()
        {
            // indien succesvol:
            InsertedMockData = true;
            // anders: 
            InsertedMockData = false;
        }

    }
}
