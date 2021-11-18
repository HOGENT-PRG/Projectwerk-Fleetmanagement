using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLaag.Exceptions {
	public class RRNValideerderException : Exception {
		public RRNValideerderException(string message) : base(message) {
		}

		public RRNValideerderException(string message, Exception innerException) : base(message, innerException) {
		}
	}
}
