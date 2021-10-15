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
        public Adres(string straatnaam,string huisnummer,string postcode,string plaatsnaam,string provincie)
        {
            setStraatnaam(straatnaam);
            setHuisnummer(huisnummer);
            setPostcode(postcode);
            setPlaatsnaam(plaatsnaam);
            setProvincie(provincie);

        }
        public string Straatnaam { get; set; }
        public string Huisnummer { get; set; }
        public string Postcode { get; set; }
        public string Plaatsnaam { get; set; }
        public string Provincie { get; set; }
        public void setStraatnaam(string straatnaam)
        {
            if (string.IsNullOrEmpty(straatnaam)) throw new AdresException("Straat moet worden in gevuld worden");
            Straatnaam = straatnaam;
        }    public void setHuisnummer(string huisnummer)
        {
            if (string.IsNullOrEmpty(huisnummer)) throw new AdresException("huisnummer moet worden in gevuld worden");
            Huisnummer = huisnummer;
        }
          public void setPostcode(string postcode)
        {
            if (string.IsNullOrEmpty(postcode)) throw new AdresException("Postcode moet worden in gevuld worden");
            Postcode = postcode;
        }  public void setPlaatsnaam(string plaatsnaam)
        {
            if (string.IsNullOrEmpty(plaatsnaam)) throw new AdresException("Plaatsnaam moet worden in gevuld worden");
            Plaatsnaam = plaatsnaam;
        } public void setProvincie(string provincie)
        {
            if (string.IsNullOrEmpty(provincie)) throw new AdresException("Provincie moet worden in gevuld worden");
            Provincie = provincie;
        }

    }
}
