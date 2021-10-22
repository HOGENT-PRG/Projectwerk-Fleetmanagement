using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFApp.Exceptions {
    internal class ExterneBronParserException : Exception {
        public ExterneBronParserException(string message) : base(message) {
        }

        public ExterneBronParserException(string message, Exception innerException) : base(message, innerException) {
        }
    }
}
