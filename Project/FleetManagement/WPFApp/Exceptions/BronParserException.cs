using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFApp.Exceptions {
    internal class BronParserException : Exception {
        public BronParserException(string message) : base(message) {
        }

        public BronParserException(string message, Exception innerException) : base(message, innerException) {
        }
    }
}
