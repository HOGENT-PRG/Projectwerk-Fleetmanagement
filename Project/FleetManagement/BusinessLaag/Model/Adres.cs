using System.Linq;
using BusinessLaag.Exceptions;

namespace BusinessLaag.Model
{
   public class Adres
    {
        public int? Id { get; private set; }
        public string Straatnaam { get; private set; }
        public string Huisnummer { get; private set; }
        public string Postcode { get; private set; }
        public string Plaatsnaam { get; private set; }
        public string Provincie { get; private set; }
        public string Land { get; private set; }

        public Adres(int? id, string straatnaam,string huisnummer,
            string postcode,string plaatsnaam,string provincie, string land)
        {
            ZetId(id);
            ZetStraatnaam(straatnaam);
            ZetHuisnummer(huisnummer);
            ZetPostcode(postcode);
            ZetPlaatsnaam(plaatsnaam);
            ZetProvincie(provincie);
            ZetLand(land);
        }

        public void ZetId(int? id) 
        {
            if (id is not null || id.HasValue) {
                if (id <= 0) {
                    throw new AdresException("Adres id mag niet gelijk of kleiner dan nul zijn ");
                }
			}
            
            Id = id; // nullable toelaten
        }
        public void ZetStraatnaam(string straatnaam)
        {
            if (string.IsNullOrEmpty(straatnaam) || straatnaam.Any(char.IsDigit) || straatnaam.Length > 150) {
                throw new AdresException("Straat moet ingevuld worden en mag geen cijfers bevatten, straat mag max 150 karakters lang zijn.");
            }
            Straatnaam = straatnaam;
        }    
        public void ZetHuisnummer(string huisnummer)
        {
            if (string.IsNullOrEmpty(huisnummer) || !(huisnummer.Any(char.IsDigit)) || huisnummer.Length > 50) {
                throw new AdresException("Huisnummer moet ingevuld worden en ten minste 1 cijfer bevatten, max 50 karakters.");
            }
            Huisnummer = huisnummer;
        }
        public void ZetPostcode(string postcode)
        {
            if (string.IsNullOrEmpty(postcode) || !postcode.Any(char.IsDigit)) {
                throw new AdresException("Postcode moet ingevuld worden en cijfer(s) bevatten");
            }
            Postcode = postcode.Length >= 4 && postcode.Length < 50 ? postcode : throw new AdresException("Postcode moet op zijn minst bestaan uit 4 karakters, maximum 50 karakters.");
        }  
        public void ZetPlaatsnaam(string plaatsnaam)
        {
            if (string.IsNullOrEmpty(plaatsnaam) || plaatsnaam.Any(char.IsDigit) || plaatsnaam.Length > 150) {
                throw new AdresException("Plaatsnaam moet ingevuld worden en mag geen cijfers bevatten, tevens heeft het een maximum van 150 karakters.");
            }
            Plaatsnaam = plaatsnaam;
        } 
        public void ZetProvincie(string provincie)
        {
            if (string.IsNullOrEmpty(provincie) || provincie.Any(char.IsDigit) || provincie.Length > 150) {
                throw new AdresException("Provincie moet ingevuld worden en mag geen cijfers bevatten, mag maximum 150 karakters lang zijn.");
            }
            Provincie = provincie;
        }
        public void ZetLand(string land)
        {
            if (string.IsNullOrEmpty(land) || land.Any(char.IsDigit) || land.Length > 100) {
                throw new AdresException("Land moet ingevuld worden en mag geen cijfers bevatten. Land mag maximaal bestaan uit 100 karakters.");
			}
            Land = land;
        }

    }
}
