using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLaag.Exceptions
{
    class VoertuigException:Exception
    {
        public VoertuigException(string boodschap) : base(boodschap) { }
        public VoertuigException(string boodschap,Exception ex) : base(boodschap, ex) { }
    }
}
