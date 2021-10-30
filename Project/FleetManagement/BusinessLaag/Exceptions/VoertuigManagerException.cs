using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLaag.Exceptions {
    public class VoertuigManagerException : Exception {
        public VoertuigManagerException(string message) : base(message) {
        }

        public VoertuigManagerException(string message, Exception innerException) : base(message, innerException) {
        }
    }
}
