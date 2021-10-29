using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLaag.Exceptions;
using BusinessLaag.Model.Enum;

namespace BusinessLaag.Model
{
#nullable enable
    public class Tankkaart
    {
        public int? Id { get; private set; } // nullable toegelaten
        public string Kaartnummer { get; private set; }
        public DateTime Vervaldatum { get; private set; }
        public string? Pincode { get; private set; } // nullable toegelaten
        public List<TankkaartBrandstof> GeldigVoorBrandstoffen { get; private set; }
        public Bestuurder? Bestuurder { get; private set; }

        public Tankkaart(int? id, string kaartnummer, DateTime vervaldatum, 
            string pincode, List<TankkaartBrandstof>? geldigvoorbrandstoffen, Bestuurder? bestuurder)
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
            if (kaartnummer.Length < 5 || kaartnummer.Length > 50)
            {
                throw new TankkaartException("Het nummer van de kaart moet minstens 5 karakters lang zijn, en maximum 50 karakters lang.");
            }
            Kaartnummer = kaartnummer;
        }
        public void zetVervaldatum(DateTime vervaldatum)
        {
            DateTime minimumdate = DateTime.Now.AddHours(24 - DateTime.Now.Hour + 1);

            if (DateTime.Compare(minimumdate, vervaldatum) < 0)
                Vervaldatum = vervaldatum;
            else throw new TankkaartException("De vervaldatum van de kaart moet zich in de toekomst bevinden en minimum 1 dag geldig zijn.");            
        }
        public void zetPincode(string pincode)
        {
            if (pincode.Length is not 4 || pincode.ToCharArray().Any(c => !Char.IsDigit(c)))
            {
                throw new TankkaartException("Pincode moet 4 karakters bevatten, welke enkel cijfers mogen zijn.");
            }
            Pincode = pincode;
        }
        public void VoegBrandstofToe(TankkaartBrandstof brandstof) 
        {
            if (GeldigVoorBrandstoffen.Contains(brandstof))
                throw new TankkaartException("Brandstof zit al in het lijstje met de brandstoffen");

            GeldigVoorBrandstoffen.Add(brandstof);
        }
        public void VerwijderBrandstof(TankkaartBrandstof brandstof)
        {
            if (!GeldigVoorBrandstoffen.Contains(brandstof))
                throw new TankkaartException("Deze brandstof bestaat niet in het lijstje vooraleer je hem kunt verwijderen moet je het eerst hebben toegevoegd");

            GeldigVoorBrandstoffen.Remove(brandstof);
        }
        public void zetBestuurder(Bestuurder bestuurder)
        {
            if (Bestuurder == bestuurder && bestuurder is not null)
                throw new TankkaartException("Bestuurder hoort al bij deze tankkaart");
            else
                Bestuurder = bestuurder;
        }
    }
#nullable disable
}
