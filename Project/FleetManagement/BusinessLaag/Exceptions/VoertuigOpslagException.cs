using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLaag.Exceptions
{
    public class VoertuigOpslagException : Exception
    {
        public VoertuigOpslagException(string msg) : base(msg) { }
    }
}
