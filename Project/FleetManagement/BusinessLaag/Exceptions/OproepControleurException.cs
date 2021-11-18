using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLaag.Exceptions {
	public class OproepControleurException : Exception {
		public OproepControleurException(string message) : base(message) {
		}

		public OproepControleurException(string message, Exception innerException) : base(message, innerException) {
		}
	}
}
