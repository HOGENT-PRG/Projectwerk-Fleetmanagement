using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLaag.Exceptions;

namespace BusinessLaag
{
   public class Voertuig
    {
        public Voertuig(int id,Merk merk,string nummerplaat,Brandstof b , Voertuigsoort vrtgSoort,VoertuigKleur vrtgKleur,int aantalDeuren,Bestuurder bestuurd,string chasisnummer)
        {
            setId( id);
            Mer = merk;
            Nummerplaat = nummerplaat;
            Bra = b;
            VrtgSoort = vrtgSoort;
            VrtgKleur = vrtgKleur;
            AantalDeuren = aantalDeuren;
            Best = bestuurd;
            Chasisnummer = chasisnummer;
        }
        public string Chasisnummer { get; set; }
        public int Id { get; private set; }
        public Merk Mer { get; private set; }
        public string Nummerplaat { get; set; }
        public Brandstof Bra { get; set; }
        public Voertuigsoort VrtgSoort { get; set; }
        public VoertuigKleur VrtgKleur { get; set; }
        public int AantalDeuren { get; set; }
        public Bestuurder Best { get; set; }
        public void setId(int id)
        {
            if (id <= 0)
            {
                throw new VoertuigException("Uw klant id mag niet gelijk of kleiner dan nul zijn ");
            }
            else if (id.GetType() != typeof(int))
            {
                throw new VoertuigException("Wat u heeft ingevuld is geen numeriek getal");
            }
            Id = id;
        }
        
    }
}
