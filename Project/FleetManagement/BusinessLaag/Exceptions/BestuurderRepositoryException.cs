using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLaag.Exceptions
{
    public class BestuurderRepositoryException : Exception
    {
        public BestuurderRepositoryException(string msg) : base(msg) { }
    }
}
