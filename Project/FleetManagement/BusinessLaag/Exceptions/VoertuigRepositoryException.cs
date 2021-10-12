using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLaag.Exceptions
{
    public class VoertuigRepositoryException : Exception
    {
        public VoertuigRepositoryException(string msg) : base(msg) { }
    }
}
