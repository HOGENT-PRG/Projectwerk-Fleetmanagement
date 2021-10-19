using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLaag.Exceptions;
using BusinessLaag.Model;
namespace BusinessLaag
{
   public class Voertuig
    {
#nullable enable
        public int? Id { get; private set; }
        public Merk Merk { get; private set; }
        public string Nummerplaat { get; private set; }
        public Brandstof Brandstof { get; private set; }
        public Voertuigsoort Soort { get; private set; }
        public string? Kleur { get; private set; }
        public int? AantalDeuren { get; private set; }
        public Bestuurder? Bestuurder { get; private set; }
        public string Chassisnummer { get; private set; }

        public Voertuig(int? id, Merk merk, string nummerplaat, Brandstof brandstof , 
            Voertuigsoort soort, string? kleur, int? aantalDeuren, Bestuurder? bestuurder , string chassisnummer)
        {
            zetId(id);
            zetMerk(merk);
            zetNummerplaat(nummerplaat);
            zetBrandstof(brandstof);
            zetVoertuigSoort(soort);
            zetKleur(kleur);
            zetAantalDeuren(aantalDeuren);
            zetBestuurder(bestuurder);
            zetChasisnummer(chassisnummer);
    
         
        }
        public void zetId(int? id)
        {
            if (id is not null)
                if (id <= 0)
                    throw new TankkaartException("Uw bestuurder id mag niet gelijk of kleiner dan nul zijn ");

            Id = id; // nullable toelaten
        }
        public void zetMerk(Merk merk) {  Merk = merk; }
        public void zetNummerplaat(string nummerplaat) { Nummerplaat = nummerplaat.Length > 0 ? nummerplaat : throw new VoertuigException("Gelieve een nummerplaat op te geven"); }
        public void zetChasisnummer(string chasisnummer) { Chassisnummer = chasisnummer.Length == 17 ? chasisnummer : throw new VoertuigException("Een chassisnummer moet bestaan uit 17 karakters"); }
        //https://nl.wikipedia.org/wiki/Framenummer

        public void zetBrandstof(Brandstof brandstof) { Brandstof = brandstof; }
        public void zetVoertuigSoort(Voertuigsoort voertuigsoort) { Soort = voertuigsoort; }
        public void zetKleur(string kleur) {
            Kleur = kleur.Length > 0 ? kleur : throw new VoertuigException("Gelieve een kleur op te geven");
        }
        public void zetAantalDeuren(int? aantal) {
            if(aantal is not null)
                AantalDeuren = aantal > 0 ? aantal : throw new VoertuigException("Minimum aantal deuren is 1");

            AantalDeuren = aantal; //nullable toelaten
        }
#nullable disable
        public void zetBestuurder(Bestuurder? bestuurder)
        {
            if (Bestuurder == bestuurder)
                throw new VoertuigException("Bestuurder hoort al bij deze wagen");

            Bestuurder = bestuurder; //nullable toelaten
        }


    }
}
