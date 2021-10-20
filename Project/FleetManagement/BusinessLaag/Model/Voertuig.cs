using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLaag.Exceptions;
using BusinessLaag.Model;
using BusinessLaag.Model.Enum;

namespace BusinessLaag.Model
{
   public class Voertuig
    {
        public int? Id { get; private set; }
        public Merk Merk { get; private set; }
        public string Model { get; private set; }
        public string Nummerplaat { get; private set; }
        public Brandstof Brandstof { get; private set; }
        public Voertuigsoort Voertuigsoort { get; private set; }
        public string? Kleur { get; private set; }
        public int? AantalDeuren { get; private set; }
        public Bestuurder? Bestuurder { get; private set; }
        public string Chassisnummer { get; private set; }

        public Voertuig(int? id, Merk merk, string model, string nummerplaat, Brandstof brandstof , 
            Voertuigsoort soort, string? kleur, int? aantalDeuren, Bestuurder? bestuurder , string chassisnummer)
        {
            zetId(id);
            zetMerk(merk);
            zetModel(model);
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
                    throw new VoertuigException("Uw bestuurder id mag niet gelijk of kleiner dan nul zijn ");

            Id = id; // nullable toelaten
        }
        public void zetMerk(Merk merk) {  Merk = merk; }
        public void zetModel(string model) { 
            Model = (!string.IsNullOrEmpty(model) && !string.IsNullOrWhiteSpace(model) && model.Length < 20) ? model : throw new VoertuigException("Gelieve een model op te geven (niet leeg, max 20 char)"); 
        }
        public void zetNummerplaat(string nummerplaat) { 
            Nummerplaat = (!string.IsNullOrEmpty(nummerplaat) && !string.IsNullOrWhiteSpace(nummerplaat) && nummerplaat.Length < 20) ? nummerplaat : throw new VoertuigException("Gelieve een geldige nummerplaat op te geven (niet leeg, max 20 char)"); 
        }
        public void zetChasisnummer(string chasisnummer) {
            //https://nl.wikipedia.org/wiki/Framenummer
            Chassisnummer = (!string.IsNullOrEmpty(chasisnummer) && !string.IsNullOrWhiteSpace(chasisnummer) && chasisnummer.Length == 17) ? chasisnummer : throw new VoertuigException("Een chassisnummer moet bestaan uit 17 karakters"); 
        }
        public void zetBrandstof(Brandstof brandstof) { Brandstof = brandstof; }
        public void zetVoertuigSoort(Voertuigsoort voertuigsoort) { Voertuigsoort = voertuigsoort; }
        public void zetKleur(string? kleur) {
            if(kleur is not null)
                Kleur = (!string.IsNullOrEmpty(kleur) && !string.IsNullOrWhiteSpace(kleur) && kleur.Length < 40) ? kleur : throw new VoertuigException("Gelieve een kleur op te geven (niet leeg, max 40 chars)");

            Kleur = kleur;
        }
        public void zetAantalDeuren(int? aantal) {
            if(aantal is not null)
                AantalDeuren = aantal > 0 ? aantal : throw new VoertuigException("Het minimum aantal deuren is 1");

            AantalDeuren = aantal; //nullable toelaten
        }
        public void zetBestuurder(Bestuurder? bestuurder)
        {
            if (Bestuurder == bestuurder && bestuurder is not null)
                throw new VoertuigException("Bestuurder hoort al bij deze wagen");

            Bestuurder = bestuurder; //nullable toelaten
        }

    }
}
