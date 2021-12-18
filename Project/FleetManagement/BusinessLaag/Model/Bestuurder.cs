using System;
using System.Linq;
using BusinessLaag.Exceptions;
using BusinessLaag.Helpers;
using BusinessLaag.Model.Enum;

namespace BusinessLaag.Model
{
    public class Bestuurder
    {  
        public int? Id { get; private set; } 
        public string Naam { get; private set; }
        public string Voornaam { get; private set; }
        public Adres Adres { get; private set; } 
        public DateTime GeboorteDatum { get; private set; }

        public string RijksRegisterNummer { get; private set; }

        public RijbewijsSoort RijbewijsSoort { get; private set; }

        public Voertuig Voertuig { get; private set; }
        public Tankkaart Tankkaart { get; private set; }

        public Bestuurder(int? id, string naam, string voornaam, Adres adres, DateTime geboortedatum, 
            string rijksregisternummer, RijbewijsSoort rijbewijssoort, Voertuig voertuig, Tankkaart tankkaart)
        {
            ZetId(id);
            ZetNaam(naam);
            ZetVoornaam(voornaam);
            ZetAdres(adres);
            ZetGeboortedatum(geboortedatum);
            ZetRijksregisternummer(rijksregisternummer);
            ZetRijbewijs(rijbewijssoort);
            ZetVoertuig(voertuig);
            ZetTankkaart(tankkaart);
        }

        public void ZetId(int? id)
        {
            if (id is not null) {
                if (id <= 0) {
                    throw new BestuurderException("Uw bestuurder id mag niet gelijk of kleiner dan nul zijn ");
                }
            }

            Id = id; // nullable toelaten
        }
        public void ZetNaam(string naam) {
            if (string.IsNullOrEmpty(naam) || naam.Length < 2 || naam.Length > 70 || naam.Any(c => Char.IsDigit(c))) {
                throw new BestuurderException("Naam moet bestaan uit min 2 en max 70 karakters, mag niet leeg zijn en mag geen cijfers bevatten.");
            }
            Naam = naam;
        }
        public void ZetVoornaam(string voornaam)
        {
            if (string.IsNullOrEmpty(voornaam) || voornaam.Length < 2 || voornaam.Length > 70 || voornaam.Any(c => Char.IsDigit(c))) {
                throw new BestuurderException("Naam moet bestaan uit minstens 2, max 70 karakters en mag geen cijfers bevatten.");
            }
            Voornaam = voornaam;
        }
        public void ZetAdres(Adres adres) { 
            if(Adres == adres && adres is not null) {
                throw new BestuurderException("Dit is reeds het ingestelde adres van de bestuurder");
            }

            Adres = adres; //nullable toelaten
        }
        public void ZetGeboortedatum(DateTime geboortedatum) { 
            DateTime minimumdate = DateTime.Now.AddYears(-120);
            if ((DateTime.Compare(minimumdate, geboortedatum) < 0) && (DateTime.Compare(DateTime.Now, geboortedatum) > 0)) {
                GeboorteDatum = geboortedatum;
            } else { 
                throw new BestuurderException($"Geboortejaar moet na {minimumdate.Year} zijn, en mag niet later zijn dan vandaag."); 
            }
        }
        public void ZetRijksregisternummer(string rijksregisternummer) {
            if (RRNValideerder.Valideer(rijksregisternummer) is false) {
                throw new BestuurderException("Rijksregisternummer is ongeldig");
            }

            RijksRegisterNummer = rijksregisternummer;
        }
        public void ZetRijbewijs(RijbewijsSoort rijbewijs) { 
            RijbewijsSoort = rijbewijs; 
        }
        public void ZetTankkaart(Tankkaart tankkaart)
        {
            if (Tankkaart == tankkaart && tankkaart is not null) {
                throw new BestuurderException("Tankkaart hoort al bij deze bestuurder");
            }

            Tankkaart = tankkaart; //nullable toelaten
        }
        public void ZetVoertuig(Voertuig voertuig)
        {
            if (Voertuig == voertuig && voertuig is not null) {
                throw new BestuurderException("Voertuig reeds bekend bij bestuurder");
            }

            Voertuig = voertuig; //nullable toelaten
        }
    }

    }

