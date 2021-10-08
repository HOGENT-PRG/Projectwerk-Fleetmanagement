using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLaag.Exceptions;

namespace BusinessLaag.Helpers
{
    public class RRNValideerder
    {
        public string valideer(string ongevalideerdRijksregisternummer)
        {
            // indien successvol:
            // return GevalideerdRRN
            throw new NotImplementedException();

            // indien niet successvol:
            // throw new BestuurderException("Het opgegeven rijksregisternummer is ongeldig.");
        }
    }
}
