using BusinessLaag.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xUnitTesting._TESTDATA {
    internal class VoertuigData {
        // Beheert relatie met Bestuurder

        private List<Voertuig> _voertuigenZonderRelaties = new();
        private List<Voertuig> _voertuigenMetRelaties = new();
    }
}
