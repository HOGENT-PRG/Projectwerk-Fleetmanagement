using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessLaag.Exceptions;
using BusinessLaag.Helpers;
using BusinessLaag.Model.Attributes;
using System.Threading.Tasks;

namespace BusinessLaag
{
#nullable enable
    public class Bestuurder
    {
        public int? Id { get; private set; }
        public string Naam { get; private set; }
        public string Voornaam { get; private set; }
        public string? Adres { get; private set; } // TODO -- aparte klasse
        public long GeboorteDatum { get; private set; }

        public string RijksRegisterNummer { get; private set; }

        public RijbewijsSoort RijbewijsSoort { get; private set; }

        public Voertuig? Voertuig { get; private set; }

        public Tankkaart? Tankkaart { get; private set; }

        public Bestuurder(int? id, string naam, string voornaam, string? adres, long geboortedatum, 
            string rijksregisternummer, RijbewijsSoort rijbewijssoort, Voertuig? voertuig, Tankkaart? tankkaart)
        {
            Id = id;
            Naam = naam.Length > 1 ? naam : throw new BestuurderException("Naam moet bestaan uit minstens 2 karakters");
            Voornaam = voornaam.Length > 1 ? voornaam : throw new BestuurderException("Voornaam moet bestaan uit minstens 2 karakters");
            Adres = adres;
            GeboorteDatum = geboortedatum > -2208988800 ? geboortedatum : throw new BestuurderException("Geboortejaar moet na 1900 zijn");
            RijksRegisterNummer = new RRNValideerder().Valideer(rijksregisternummer);
            RijbewijsSoort = rijbewijssoort;
            Voertuig = voertuig;
            Tankkaart = tankkaart;
        }
#nullable disable
    }
}
