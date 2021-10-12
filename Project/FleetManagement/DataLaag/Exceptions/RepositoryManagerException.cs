using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLaag.Exceptions
{
    public class RepositoryManagerException : Exception
    {
        public RepositoryManagerException(string msg): base(msg){}
    }
}
