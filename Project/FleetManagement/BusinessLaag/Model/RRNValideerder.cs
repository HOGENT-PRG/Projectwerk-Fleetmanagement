using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLaag
{
    public class RRNValideerder
    {
        public string valideer(string ongevalideerdRijksregisternummer)
        {
            // indien successvol:
            // GevalideerdRRN = teValiderenRRN
            // indien niet successvol:
            throw new ArgumentException("Het opgegeven rijksregisternummer is ongeldig.");
        }
    }
}
