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
        
        public int? Id { get; private set; }
        public string Naam { get; private set; }
        public string Voornaam { get; private set; }
        public Adres? Adres { get; private set; }
        public long GeboorteDatum { get; private set; }

        public string RijksRegisterNummer { get; private set; }

        public RijbewijsSoort RijbewijsSoort { get; private set; }

        private List<Voertuig> Voertuigen = new();
        private List<Tankkaart> Tankkaarten = new ();

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
        }

#nullable disable
        public void zetId(int? id)
        {
            if(id is not null)
                if (id <= 0)
                    throw new BestuurderException("Uw bestuurder id mag niet gelijk of kleiner dan nul zijn ");
            
            Id = id; // nullable toelaten
        }
        public void zetNaam(string naam) {
            if (naam.Length > 1)
                throw new BestuurderException("Naam moet bestaan uit minstens 2 karakters");

            Naam = naam;
        }
        public void zetVoornaam(string voornaam)
        {
            if (voornaam.Length < 2)
                throw new BestuurderException("Naam moet bestaan uit minstens 2 karakters");

            Voornaam = voornaam;
        }
        public void zetAdres(Adres adres) { 
            Adres = adres; 
        }
        public void zetGeboortedatum(long geboortedatum) { 
            if (geboortedatum > -2208988800)
                throw new BestuurderException("Geboortejaar moet na 1900 zijn");

            GeboorteDatum = geboortedatum;
        }
        public void zetRijksregisternummer(string rijksregisternummer) {
            if (!(new RRNValideerder().Valideer(rijksregisternummer)))
                throw new BestuurderException("Rijksregisternummer is ongeldig");

            RijksRegisterNummer = rijksregisternummer;
        }

        public void zetRijbewijs(RijbewijsSoort rijbewijs) { RijbewijsSoort = rijbewijs; }

        public void zetTankkaart(Tankkaart tankkaart)
        {
            if (Tankkaarten.Contains(tankkaart))
            {
                throw new BestuurderException("Tankkaart hoort al bij deze bestuurder");
            }
            else
            {
                Tankkaarten.Add(tankkaart);
            }
        }
        public void zetVoertuig(Voertuig voertuig)
        {
            if (Voertuigen.Contains(voertuig))
            {
                throw new BestuurderException("Voertuig reeds bekend bij bestuurder");
            }
            else
            {
                Voertuigen.Add(voertuig);
            }
        }
    }

    }

