using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLaag.Exceptions;

namespace BusinessLaag
{
#nullable enable
    public class Tankkaart
    {
        public int? Id { get; private set; }
        public string Kaartnummer { get; private set; }
        public long Vervaldatum { get; private set; }
        public int? Pincode { get; private set; }
        public List<Brandstof>? GeldigVoorBrandstoffen { get; private set; }
        public Bestuurder? Bestuurder { get; private set; }

        public Tankkaart(int? id, string kaartnummer, long vervaldatum, int? pincode, 
            List<Brandstof>? geldigvoorbrandstoffen, Bestuurder? bestuurder)
        {
            Id = id;
            Kaartnummer = kaartnummer.Length > 5 && kaartnummer.Length < 900 ? kaartnummer : throw new TankkaartException("Het nummer van de kaart moet minstens 5 karakters lang zijn.");
            Vervaldatum = DateTimeOffset.Now.ToUnixTimeSeconds() < vervaldatum ? vervaldatum : throw new TankkaartException("De vervaldatum van de kaart moet zich in de toekomst bevinden.");
            Pincode = pincode;
            GeldigVoorBrandstoffen = geldigvoorbrandstoffen;
            Bestuurder = bestuurder;
       }
    }
#nullable disable
}
