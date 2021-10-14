using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLaag.Interfaces
{
    public interface IStartupSequence
    {
        public bool HasRan { get; }
        public bool ConnectionSuccessful { get;  }
        public bool TruncatedTables { get; }
        public bool InsertedMockData { get; }

        public void Execute(string connectionString, bool truncateTablesOnStartup=false, bool insertMockData=false);
    }
}
