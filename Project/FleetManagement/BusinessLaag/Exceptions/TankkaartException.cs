using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLaag.Exceptions
{
    public class TankkaartException:Exception
    {
        public TankkaartException(string boodschap) : base(boodschap) { }
        public TankkaartException(string boodschap, Exception ex) : base(boodschap, ex) { }
    }
}
