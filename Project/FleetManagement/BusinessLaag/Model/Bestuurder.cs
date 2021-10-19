using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessLaag.Exceptions;
using BusinessLaag.Helpers;
using BusinessLaag.Model.Attributes;
using System.Threading.Tasks;
using BusinessLaag.Model;
namespace BusinessLaag
{
#nullable enable
    public class Bestuurder
    {
        private Dictionary<Bestuurder, Voertuig> bestuurderVoertuig = new Dictionary<Bestuurder, Voertuig>();
        private Dictionary<Bestuurder, Tankkaart> bestuurderTankkaart = new Dictionary<Bestuurder, Tankkaart>();
        public int? Id { get; private set; }
        public string Naam { get; private set; }
        public string Voornaam { get; private set; }
        public Adres? Adres { get; private set; } // TODO -- aparte klasse
        public long GeboorteDatum { get; private set; }

        public string RijksRegisterNummer { get; private set; }

        public RijbewijsSoort RijbewijsSoort { get; private set; }

        public Voertuig Voertuig { get; private set; }

        public Tankkaart Tankkaart { get; private set; }

        public Bestuurder(int? id, string naam, string voornaam, Adres? adres, long geboortedatum, 
            string rijksregisternummer, RijbewijsSoort rijbewijssoort, Voertuig voertuig, Tankkaart tankkaart)
        {
            zetId(id);
            zetNaam(naam);
            zetVoornaam(voornaam);
            zetAdres(adres);
            zetGeboortedatum(geboortedatum);
            zetRijksregisternummer(rijksregisternummer);
            zetRijbewijs(rijbewijssoort);
            zetVoertuig(voertuig);
            zetTankkaart(tankkaart);
            /*  Id = id;
            Naam = naam.Length > 1 ? naam : throw new BestuurderException("Naam moet bestaan uit minstens 2 karakters");
            Voornaam = voornaam.Length > 1 ? voornaam : throw new BestuurderException("Voornaam moet bestaan uit minstens 2 karakters");
            Adres = adres;
            GeboorteDatum = geboortedatum > -2208988800 ? geboortedatum : throw new BestuurderException("Geboortejaar moet na 1900 zijn");
            RijksRegisterNummer = (new RRNValideerder().Valideer(rijksregisternummer)) ? rijksregisternummer : throw new BestuurderException("Rijksregisternummer is ongeldig");
            RijbewijsSoort = rijbewijssoort;
            Voertuig = voertuig;
            Tankkaart = tankkaart;*/
        }

#nullable disable
        public void zetId(int id)
        {
            if (id <= 0)
            {
                throw new BestuurderException("Uw bestuurder id mag niet gelijk of kleiner dan nul zijn ");
            }
            else if (id.GetType() != typeof(int))
            {
                throw new BestuurderException("Wat u heeft ingevuld is geen numeriek getal");
            }
            Id = id;
        }
        public void zetNaam(string naam) {
            if (naam.GetType() != typeof(string))
            {
                throw new BestuurderException("wat u heeft ingegeven is niet het juiste datatype voor een naam");
            }else if (naam.Length > 1)
            {
                throw new BestuurderException("Naam moet bestaan uit minstens 2 karakters");
            }
            Naam = naam;
        }
        public void zetVoornaam(string voornaam)
        {
            if (voornaam.GetType() != typeof(string))
            {
                throw new BestuurderException("wat u heeft ingegeven is niet het juiste datatype voor een naam");
            }
            else if (voornaam.Length > 1)
            {
                throw new BestuurderException("Naam moet bestaan uit minstens 2 karakters");
            }
            Voornaam = voornaam;
        }
        public void zetAdres(Adres adres) { Adres = adres; }
        public void zetGeboortedatum(long geboortedatum) { 
            if (geboortedatum > -2208988800)
            {
                throw new BestuurderException("Geboortejaar moet na 1900 zijn");
            }
            GeboorteDatum = geboortedatum;
        }
        public void zetRijksregisternummer(string rijksregisternummer) {
            if (new RRNValideerder().Valideer(rijksregisternummer)==false) {
                throw new BestuurderException("Rijksregisternummer is ongeldig");
            }
            RijksRegisterNummer = rijksregisternummer;
            }
        public void zetRijbewijs(RijbewijsSoort rijbewijs) { RijbewijsSoort = rijbewijs; }

        public void zetTankkaart(Tankkaart tankkaart)
        {
            if (bestuurderTankkaart.Keys.Contains(tankkaart.Bestuurder))
            {
                throw new BestuurderException("Tankkaart hoort al bij een bestuurder");
            }
            else
            {
                bestuurderTankkaart.Add(tankkaart.Bestuurder, tankkaart);
            }
            Tankkaart = tankkaart;
        }
        public void zetVoertuig(Voertuig voertuig)
        {
            if (bestuurderVoertuig.Keys.Contains(Voertuig.Bestuurder))
            {
                throw new BestuurderException("Bestuurder hoort al bij een wagen");
            }
            else
            {
                bestuurderVoertuig.Add(voertuig.Bestuurder, voertuig);
            }
            Voertuig = voertuig;
        }
    }

    }

