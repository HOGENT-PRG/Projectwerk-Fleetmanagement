using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLaag
{
    public class Rijksregisternummer
    {
        public string GevalideerdRRN { get; private set; }
        public Rijksregisternummer(string ongevalideerdRijksregisternummer)
        {
            _valideer(ongevalideerdRijksregisternummer);
        }

        private void _valideer(string teValiderenRRN)
        {
            // indien successvol:
            // GevalideerdRRN = teValiderenRRN
            // indien niet successvol:
            throw new ArgumentException("Het opgegeven rijksregisternummer is ongeldig.");
        }

        public override string ToString()
        {
            return GevalideerdRRN;
        }
    }
}
