using BusinessLaag.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Werd niet uitgewerkt, zie hoofdstuk Unit testing in documentatie
namespace xUnitTesting.Generators {
    internal class TankkaartData {
        // Beheert relatie met Bestuurder

        private List<Tankkaart> _tankkaartenZonderRelaties = new();
        private List<Tankkaart> _tankkaartenMetRelaties = new();
    }
}
