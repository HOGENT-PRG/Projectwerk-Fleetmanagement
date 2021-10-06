using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessLaag.Exceptions;
using System.Threading.Tasks;

namespace BusinessLaag
{
    public class Bestuurder
    {
        public Bestuurder(int id, string naam, string voornaam, string adres, DateTime geboortedatum, RijksregisterNumm rijkregn, RijbewijsSoort rijbewijs, Voertuig voertuig, Tankkaart tankkaart)
        {
            setId(id);
            setNaam(naam);
            setVoornaam(voornaam);


        }
        public int Id { get; private set; }
        public string Naam { get; set; }
        public string Voornaam { get; set; }
        public string Adres { get; set; }
        public DateTime GeboorteDatum { get; set; }
        public RijksregisterNumm RijkRegNummer { get; set; }
        public RijbewijsSoort RijbewijsSoort { get; set; }
        public Voertuig Voertuig { get; set; }
        public Tankkaart Tank { get; set; }
        public void setAdres(string adres)
        {
            if (string.IsNullOrEmpty(adres))
            {
                throw new BestuurderException("Adres mag niet leegstaan");
            }
        }
        public void setVoornaam(string voornaam)
        {
            if (string.IsNullOrEmpty(voornaam))
            {
                throw new BestuurderException("VoorNaam moet ingevuld worden");
            }
            Voornaam = voornaam;
        }  public void setNaam(string naam)
        {
            if (string.IsNullOrEmpty(naam))
            {
                throw new BestuurderException("Naam moet ingevuld worden");
            }
            Naam = naam;
        }
        public void setId(int id)
        {
            if (id <= 0)
            {
                throw new BestuurderException("Uw id mag niet gelijk of kleiner dan nul zijn ");
            }
            else if (id.GetType() != typeof(int))
            {
                throw new VoertuigException("Wat u heeft ingevuld is geen numeriek getal");
            }

            Id = id;
        }
    }
}
