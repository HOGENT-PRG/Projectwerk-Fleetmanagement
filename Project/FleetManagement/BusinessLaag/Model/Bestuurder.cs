using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessLaag.Exceptions;
using BusinessLaag.Helpers;
using System.Threading.Tasks;
using BusinessLaag.Model;
using BusinessLaag.Model.Enum;

namespace BusinessLaag.Model
{
    public class Bestuurder
    {
        
        public int? Id { get; private set; } // nullable toegelaten
        public string Naam { get; private set; }
        public string Voornaam { get; private set; }
        public Adres Adres { get; private set; } // nullable toegelaten
        public long GeboorteDatum { get; private set; }

        public string RijksRegisterNummer { get; private set; }

        public RijbewijsSoort RijbewijsSoort { get; private set; }

        public Voertuig Voertuig { get; private set; } // nullable toegelaten
        public Tankkaart Tankkaart { get; private set; } // nullable toegelaten

        public Bestuurder(int? id, string naam, string voornaam, Adres adres, long geboortedatum, 
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

        public void zetId(int? id)
        {
            if(id is not null)
                if (id <= 0)
                    throw new BestuurderException("Uw bestuurder id mag niet gelijk of kleiner dan nul zijn ");
            
            Id = id; // nullable toelaten
        }
        public void zetNaam(string naam) {
            if (naam.Length < 2)
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
            if(Adres == adres)
            {
                throw new BestuurderException("Dit is reeds het ingestelde adres van de bestuurder");
            }

            Adres = adres; //nullable toelaten
        }
        public void zetGeboortedatum(long geboortedatum) { 
            if (geboortedatum < int.MinValue)
                throw new BestuurderException("Geboortejaar moet na 1900 zijn");

            GeboorteDatum = geboortedatum;
        }
        public void zetRijksregisternummer(string rijksregisternummer) {
            if (new RRNValideerder().Valideer(rijksregisternummer) is false)
                throw new BestuurderException("Rijksregisternummer is ongeldig");

            RijksRegisterNummer = rijksregisternummer;
        }
        public void zetRijbewijs(RijbewijsSoort rijbewijs) { 
            RijbewijsSoort = rijbewijs; 
        }
        public void zetTankkaart(Tankkaart tankkaart)
        {
            if (Tankkaart == tankkaart && tankkaart is not null)
            {
                throw new BestuurderException("Tankkaart hoort al bij deze bestuurder");
            }

            Tankkaart = tankkaart; //nullable toelaten
        }
        public void zetVoertuig(Voertuig voertuig)
        {
            if (Voertuig == voertuig && voertuig is not null)
            {
                throw new BestuurderException("Voertuig reeds bekend bij bestuurder");
            }

            Voertuig = voertuig; //nullable toelaten
        }
    }

    }

