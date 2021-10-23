using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFApp.Interfaces;

namespace WPFApp.Model.Communiceerders {
    internal class ApiCommuniceerder : ICommuniceer {
        private readonly string API_BASIS_PAD;

        public ApiCommuniceerder(string aPI_BASIS_PAD) {
            this.API_BASIS_PAD = aPI_BASIS_PAD;
        }
    }
}
