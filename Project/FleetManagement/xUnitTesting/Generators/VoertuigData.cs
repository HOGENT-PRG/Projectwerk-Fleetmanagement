using BusinessLaag.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Werd niet uitgewerkt, zie hoofdstuk Unit testing in documentatie
namespace xUnitTesting.Generators {
    internal class VoertuigData {
        // Beheert relatie met Bestuurder

        private List<Voertuig> _voertuigenZonderRelaties = new();
        private List<Voertuig> _voertuigenMetRelaties = new();
    }
}
