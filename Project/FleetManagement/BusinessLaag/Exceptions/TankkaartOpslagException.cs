using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLaag.Exceptions
{
    public class TankkaartOpslagException : Exception
    {
        public TankkaartOpslagException(string msg) : base(msg) { }

		public TankkaartOpslagException(string message, Exception innerException) : base(message, innerException) {
		}
	}
}
