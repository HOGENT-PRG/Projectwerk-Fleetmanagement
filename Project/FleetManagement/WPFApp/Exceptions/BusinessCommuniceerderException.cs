using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFApp.Exceptions {
    public class BusinessCommuniceerderException : Exception {
        public BusinessCommuniceerderException(string message) : base(message) {
        }

        public BusinessCommuniceerderException(string message, Exception innerException) : base(message, innerException) {
        }
    }
}
