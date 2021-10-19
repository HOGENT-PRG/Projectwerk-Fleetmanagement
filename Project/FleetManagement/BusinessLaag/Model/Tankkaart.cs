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
        private Dictionary<Bestuurder, Tankkaart> bestuurderTankkaart = new Dictionary<Bestuurder, Tankkaart>();
        private List<Brandstof> brandstoffen = new List<Brandstof>();
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
        public void zetId(int id) { }
        public void zetKaartnummer(string kaartnummer)
        {
            if (kaartnummer.Length > 5 && kaartnummer.Length < 900)
            {
                throw new TankkaartException("Het nummer van de kaart moet minstens 5 karakters lang zijn.");
            }
            Kaartnummer = kaartnummer;
        }
        public void zetVervaldatum(long vervaldatum)
        {
            if (DateTimeOffset.Now.ToUnixTimeSeconds() < vervaldatum)
            {
                throw new TankkaartException("De vervaldatum van de kaart moet zich in de toekomst bevinden.");
                
            }
            Vervaldatum = vervaldatum;
        }
        public void zetPincode(string pincode)
        {
            if (pincode.Length > 4)
            {
                throw new TankkaartException("Pincode mag maar 4 cijfers bevatten");
            }
        }
        public void VoegBrandstofToe(Brandstof brandstof) 
        {
            if (brandstoffen.Contains(brandstof))
            {
                throw new TankkaartException("Brandstof zit al in het lijstje met de brandstoffen");
            }
            else
            {
                brandstoffen.Add(brandstof);
            }
        }
        public void VerwijderBrandstof(Brandstof brandstof)
        {
            if (brandstoffen.Contains(brandstof))
            {
                brandstoffen.Remove(brandstof);
            }
            else
            {
                throw new TankkaartException("Deze brandstof bestaat niet in het lijstje vooraleer je hem kunt verwijderen moet je het eerst hebben toegevoegd");
            }
        }
        public void zetBestuurder(Bestuurder bestuurder)
        {
            try
            {
                if (bestuurderTankkaart.Keys.Contains(bestuurder))
                {
                    throw new VoertuigException("Bestuurder hoort al bij een tankkaart");
                }
                else
                {
                    bestuurderTankkaart.Add(bestuurder, bestuurder.Tankkaart);
                }
            }
            catch (Exception ex)
            {
                throw new VoertuigException("tankkaart", ex);
            }
        }
    }
#nullable disable
}
