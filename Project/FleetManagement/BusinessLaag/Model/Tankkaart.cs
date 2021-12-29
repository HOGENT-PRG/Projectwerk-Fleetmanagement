using System;
using System.Collections.Generic;
using System.Linq;
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
        public Bestuurder Bestuurder { get; private set; }

        public Tankkaart(int? id, string kaartnummer, DateTime vervaldatum, 
            string pincode, List<TankkaartBrandstof>? geldigvoorbrandstoffen, Bestuurder bestuurder)
        {
            ZetId(id);
            ZetKaartnummer(kaartnummer);
            ZetVervaldatum(vervaldatum);
            ZetPincode(pincode);
            ZetBestuurder(bestuurder);
            GeldigVoorBrandstoffen = geldigvoorbrandstoffen?.Distinct()?.ToList() ?? new();
       }

        public void ZetId(int? id) {
            if (id is not null) {
                if (id <= 0) {
                    throw new TankkaartException("Indien een Id opgegeven wordt mag deze niet kleiner zijn dan 0.");
                }
            }
            Id = id; // nullable toelaten
        }
        public void ZetKaartnummer(string kaartnummer)
        {
            if (kaartnummer.Length < 5 || kaartnummer.Length > 60) {
                throw new TankkaartException("Het nummer van de kaart moet minstens 5, maximum 60 karakters lang zijn.");
            }
            Kaartnummer = kaartnummer;
        }
        public void ZetVervaldatum(DateTime vervaldatum)
        {
            DateTime minimumdate = DateTime.Now.AddHours(24 - DateTime.Now.Hour + 1);

            if (DateTime.Compare(minimumdate, vervaldatum) < 0) {
                Vervaldatum = vervaldatum;
            } else throw new TankkaartException("De vervaldatum van de kaart moet zich in de toekomst bevinden en minimum 1 dag geldig zijn.");            
        }
        public void ZetPincode(string pincode)
        {
            if (pincode.Length is not 4 || pincode.ToCharArray().Any(c => !Char.IsDigit(c))) {
                throw new TankkaartException("Pincode moet exact 4 karakters lang zijn en enkel bestaan uit cijfers.");
            }

            Pincode = pincode;
        }
        public void VoegBrandstofToe(TankkaartBrandstof brandstof) 
        {
            if (GeldigVoorBrandstoffen.Contains(brandstof)) {
                throw new TankkaartException("Brandstof zit reeds in de lijst met brandstoffen.");
            }

            GeldigVoorBrandstoffen.Add(brandstof);
        }
        public void VerwijderBrandstof(TankkaartBrandstof brandstof)
        {
            if (!GeldigVoorBrandstoffen.Contains(brandstof)) {
                throw new TankkaartException("Deze brandstof bestaat niet in de lijst en kan dus niet verwijdert worden.");
            }

            GeldigVoorBrandstoffen.Remove(brandstof);
        }
        public void ZetBestuurder(Bestuurder bestuurder)
        {
            if (Bestuurder == bestuurder && bestuurder is not null) {
                throw new TankkaartException("Bestuurder hoort al bij deze tankkaart");
            } else {
                Bestuurder = bestuurder;
            }
        }
    }
#nullable disable
}
