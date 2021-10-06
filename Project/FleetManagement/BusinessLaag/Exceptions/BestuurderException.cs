using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLaag.Exceptions
{
    public class BestuurderException:Exception
    {
        public BestuurderException(string boodschap) : base(boodschap) { }
        public BestuurderException(string boodschap,Exception ex) : base(boodschap, ex) { }
    }
}
