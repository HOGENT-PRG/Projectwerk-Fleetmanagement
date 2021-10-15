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
        //huisnummer stond niet in de klassendiagram maar da hoort bij adres
        public Adres(int? id, string straatnaam,string huisnummer,string postcode,string plaatsnaam,string provincie, string land)
        {
            zetId(id);
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

        public void zetId(int? id)
        {
            if (id == null)
                throw new AdresException("Nullable id is enkel toegelaten door gebruik van de constructor");
            else
                Id = id > 0 ? id : throw new AdresException("Id kan niet kleiner zijn dan 1");
        }
        public void zetStraatnaam(string straatnaam)
        {
            if (string.IsNullOrEmpty(straatnaam)) throw new AdresException("Straat moet worden ingevuld worden");
            Straatnaam = straatnaam;
        }    
        
        public void zetHuisnummer(string huisnummer)
        {
            if (string.IsNullOrEmpty(huisnummer)) throw new AdresException("huisnummer moet worden ingevuld worden");
            Huisnummer = huisnummer;
        }
          public void zetPostcode(string postcode)
        {
            if (string.IsNullOrEmpty(postcode)) throw new AdresException("Postcode moet worden ingevuld worden");
            Postcode = postcode.Length >= 4 ? postcode : throw new AdresException("Postcode moet op zijn minst bestaan uit 4 karakters");
        }  
        
        public void zetPlaatsnaam(string plaatsnaam)
        {
            if (string.IsNullOrEmpty(plaatsnaam)) throw new AdresException("Plaatsnaam moet worden ingevuld worden");
            Plaatsnaam = plaatsnaam;
        } 
        
        public void zetProvincie(string provincie)
        {
            if (string.IsNullOrEmpty(provincie)) throw new AdresException("Provincie moet worden ingevuld worden");
            Provincie = provincie;
        }

        public void zetLand(string land)
        {
            if (string.IsNullOrEmpty(land)) throw new AdresException("Land moet worden ingevuld worden");
            Land = land;
        }

    }
}
