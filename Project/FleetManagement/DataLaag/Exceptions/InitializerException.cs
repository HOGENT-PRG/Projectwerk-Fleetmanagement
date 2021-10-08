using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLaag.Exceptions
{
    public class InitializerException : Exception
    {
        public InitializerException(string msg) : base(msg) { }
    }
}
