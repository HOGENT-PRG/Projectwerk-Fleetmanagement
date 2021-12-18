using BusinessLaag.Exceptions;
using BusinessLaag.Model.Enum;

namespace BusinessLaag.Model
{
   public class Voertuig
    {
        public int? Id { get; private set; }
        public Merk Merk { get; private set; }
        public string Model { get; private set; }
        public string Nummerplaat { get; private set; }
        public VoertuigBrandstof Brandstof { get; private set; }
        public Voertuigsoort Voertuigsoort { get; private set; }
        public string? Kleur { get; private set; }
        public int? AantalDeuren { get; private set; }
        public Bestuurder Bestuurder { get; private set; }
        public string Chassisnummer { get; private set; }

        public Voertuig(int? id, Merk merk, string model, string nummerplaat, VoertuigBrandstof brandstof , 
            Voertuigsoort soort, Bestuurder bestuurder, string chassisnummer, string? kleur, int? aantalDeuren)
        {
            ZetId(id);
            ZetMerk(merk);
            ZetModel(model);
            ZetNummerplaat(nummerplaat);
            ZetBrandstof(brandstof);
            ZetVoertuigSoort(soort);
            ZetKleur(kleur);
            ZetAantalDeuren(aantalDeuren);
            ZetBestuurder(bestuurder);
            ZetChassisnummer(chassisnummer);
        }

        public void ZetId(int? id)
        {
            if (id is not null) {
                if (id <= 0) {
                    throw new VoertuigException("Uw bestuurder id mag niet gelijk of kleiner dan nul zijn ");
                }
            }
            Id = id; // nullable toelaten
        }

        public void ZetMerk(Merk merk) {  
            Merk = merk; 
        }

        public void ZetModel(string model) { 
            Model = (!string.IsNullOrEmpty(model) && !string.IsNullOrWhiteSpace(model) && model.Length < 20) 
                    ? model 
                    : throw new VoertuigException("Gelieve een model op te geven (niet leeg, max 20 char)"); 
        }

        public void ZetNummerplaat(string nummerplaat) { 
            Nummerplaat = (!string.IsNullOrEmpty(nummerplaat) && !string.IsNullOrWhiteSpace(nummerplaat) && nummerplaat.Length < 20) ? nummerplaat : throw new VoertuigException("Gelieve een geldige nummerplaat op te geven (niet leeg, max 20 char)"); 
        }


        public void ZetChassisnummer(string chassisnummer) {
            //https://nl.wikipedia.org/wiki/Framenummer

            Chassisnummer = (!string.IsNullOrEmpty(chassisnummer) && !string.IsNullOrWhiteSpace(chassisnummer) && chassisnummer.Length == 17) 
                            ? chassisnummer 
                            : throw new VoertuigException("Een chassisnummer moet bestaan uit 17 karakters"); 
        }

        public void ZetBrandstof(VoertuigBrandstof brandstof) { 
            Brandstof = brandstof; 
        
        }

        public void ZetVoertuigSoort(Voertuigsoort voertuigsoort) { 
            Voertuigsoort = voertuigsoort; 
        }

        public void ZetKleur(string? kleur) {
            if (kleur is not null) {
                Kleur = (!string.IsNullOrEmpty(kleur) && !string.IsNullOrWhiteSpace(kleur) && kleur.Length < 40) 
                        ? kleur 
                        : throw new VoertuigException("Gelieve een kleur op te geven (niet leeg, max 40 chars)");
            }

            Kleur = kleur;
        }
        public void ZetAantalDeuren(int? aantal) {
            if (aantal is not null) {
                AantalDeuren = aantal > 0 && aantal < 21 ? aantal : throw new VoertuigException("Het minimum aantal deuren is 1, max 20.");
            }

            AantalDeuren = aantal; //nullable toelaten
        }
        public void ZetBestuurder(Bestuurder? bestuurder)
        {
            if (Bestuurder == bestuurder && bestuurder is not null) {
                throw new VoertuigException("Bestuurder hoort al bij deze wagen");
            }

            Bestuurder = bestuurder; //nullable toelaten
        }

    }
}
