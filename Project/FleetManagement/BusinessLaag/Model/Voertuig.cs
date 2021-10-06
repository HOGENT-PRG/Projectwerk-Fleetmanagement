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

    }
}
