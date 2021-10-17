using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLaag.Exceptions;

namespace BusinessLaag.Model
{
   public class Adres
    {

        public Adres(int? id, string straatnaam,string huisnummer,string postcode,string plaatsnaam,string provincie, string land)
        {
            if(id == null) { Id = id; } else { zetId((int)id); } // bij constr. null id toelaten
            zetStraatnaam(straatnaam);
            zetHuisnummer(huisnummer);
            zetPostcode(postcode);
            zetPlaatsnaam(plaatsnaam);
            zetProvincie(provincie);
            zetLand(land);
        }

        public int? Id { get; private set; }
        public string Straatnaam { get; private set; }
        public string Huisnummer { get; private set; }
        public string Postcode { get; private set; }
        public string Plaatsnaam { get; private set; }
        public string Provincie { get; private set; }
        public string Land { get; private set; }

        public void zetId(int id)
        {
            Id = id > 0 ? id : throw new AdresException("Id kan niet kleiner zijn dan 1");
        }
        public void zetStraatnaam(string straatnaam)
        {
            if (string.IsNullOrEmpty(straatnaam) || straatnaam.Any(char.IsDigit)) throw new AdresException("Straat moet ingevuld worden en mag geen cijfers bevatten");
            Straatnaam = straatnaam;
        }    
        
        public void zetHuisnummer(string huisnummer)
        {
            if (string.IsNullOrEmpty(huisnummer) || !(huisnummer.Any(char.IsDigit))) throw new AdresException("Huisnummer moet ingevuld worden en ten minste 1 cijfer bevatten");
            Huisnummer = huisnummer;
        }
          public void zetPostcode(string postcode)
        {
            if (string.IsNullOrEmpty(postcode) || !postcode.Any(char.IsDigit)) throw new AdresException("Postcode moet ingevuld worden en cijfer(s) bevatten");
            Postcode = postcode.Length >= 4 ? postcode : throw new AdresException("Postcode moet op zijn minst bestaan uit 4 karakters");
        }  
        
        public void zetPlaatsnaam(string plaatsnaam)
        {
            if (string.IsNullOrEmpty(plaatsnaam) || plaatsnaam.Any(char.IsDigit)) throw new AdresException("Plaatsnaam moet ingevuld worden en mag geen cijfers bevatten");
            Plaatsnaam = plaatsnaam;
        } 
        
        public void zetProvincie(string provincie)
        {
            if (string.IsNullOrEmpty(provincie) || provincie.Any(char.IsDigit)) throw new AdresException("Provincie moet ingevuld worden en mag geen cijfers bevatten");
            Provincie = provincie;
        }

        public void zetLand(string land)
        {
            if (string.IsNullOrEmpty(land) || land.Any(char.IsDigit)) throw new AdresException("Land moet ingevuld worden en mag geen cijfers bevatten");
            Land = land;
        }

    }
}
