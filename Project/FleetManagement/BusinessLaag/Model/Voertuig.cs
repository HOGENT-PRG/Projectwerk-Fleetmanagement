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
        private Dictionary<Bestuurder, Voertuig> voertuigBestuurder = new Dictionary<Bestuurder, Voertuig>();

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
            Id = id;
            Merk = merk;
            Nummerplaat = nummerplaat;
            Brandstof = brandstof;
            Soort = soort;
            Kleur = kleur;
            AantalDeuren = aantalDeuren;
            Bestuurder = bestuurder;
            Chassisnummer = chassisnummer;
        }

#nullable disable
        public void zetBestuurder(Bestuurder bestuurder,Voertuig voertuig)
        {
            if (voertuigBestuurder.Keys.Contains(bestuurder))
            {
                throw new VoertuigException("Bestuurder hoort al bij een wagen");
            }
            else
            {
                voertuigBestuurder.Add(bestuurder, voertuig);
            }
        }


    }
}
