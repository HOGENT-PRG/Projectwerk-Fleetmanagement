using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFApp.Exceptions {
    public class ApiCommuniceerdeerException : Exception {
        public ApiCommuniceerdeerException(string message) : base(message) {
        }

        public ApiCommuniceerdeerException(string message, Exception innerException) : base(message, innerException) {
        }
    }
}
