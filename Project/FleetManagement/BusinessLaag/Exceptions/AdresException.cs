using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLaag.Exceptions
{
   public class AdresException:Exception
    {
        public AdresException(string boodschap) : base(boodschap) { }
        public AdresException(string boodschap, Exception ex) : base(boodschap, ex) { }
    }
}
