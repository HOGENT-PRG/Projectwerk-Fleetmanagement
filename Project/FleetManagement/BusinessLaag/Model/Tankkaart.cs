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
        public List<Brandstof> GeldigVoorBrandstoffen { get; private set; }
        public Bestuurder? Bestuurder { get; private set; }

        public Tankkaart(int id, string kaartnummer, long vervaldatum, string pincode, List<Brandstof>? geldigvoorbrandstoffen, Bestuurder? bestuurder)
        {
            zetId(id);
            zetKaartnummer(kaartnummer);
            zetVervaldatum(vervaldatum);
            zetPincode(pincode);
            zetBestuurder(bestuurder);
            GeldigVoorBrandstoffen = geldigvoorbrandstoffen ?? new();
       }
        public void zetId(int? id) {
            if (id is not null)
                if (id <= 0)
                    throw new TankkaartException("Uw bestuurder id mag niet gelijk of kleiner dan nul zijn ");

            Id = id; // nullable toelaten
        }
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
            if (DateTimeOffset.Now.ToUnixTimeSeconds() > vervaldatum)
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
            if (GeldigVoorBrandstoffen.Contains(brandstof))
            {
                throw new TankkaartException("Brandstof zit al in het lijstje met de brandstoffen");
            }
            else
            {
                GeldigVoorBrandstoffen.Add(brandstof);
            }
        }
        public void VerwijderBrandstof(Brandstof brandstof)
        {
            if (GeldigVoorBrandstoffen.Contains(brandstof))
            {
                GeldigVoorBrandstoffen.Remove(brandstof);
            }
            else
            {
                throw new TankkaartException("Deze brandstof bestaat niet in het lijstje vooraleer je hem kunt verwijderen moet je het eerst hebben toegevoegd");
            }
        }
        public void zetBestuurder(Bestuurder bestuurder)
        {
            if (Bestuurder == bestuurder)
            {
                throw new TankkaartException("Bestuurder hoort al bij deze tankkaart");
            }
            else
            {
                Bestuurder = bestuurder;
            }
        }
    }
#nullable disable
}
