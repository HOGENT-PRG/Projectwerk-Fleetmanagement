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
            zetId(id);
            zetMerk(merk);
            zetNummerplaat(nummerplaat);
            zetBrandstof(brandstof);
            zetVoertuigSoort(soort);
            zetKleur(kleur);
            zetAantalDeuren(aantalDeuren);
            zetBestuurder(bestuurder);
            zetChasisnummer(chassisnummer);
    
         
        }
        public void zetId(int id)
        {
            if (id <= 0)
            {
                throw new VoertuigException("Uw voertuig id mag niet gelijk of kleiner dan nul zijn ");
            }
            else if (id.GetType() != typeof(int))
            {
                throw new VoertuigException("Wat u heeft ingevuld is geen numeriek getal");
            }
            Id = id;
        }
        public void zetMerk(Merk merk) {  Merk = merk; }
        //Model is toch hetzelfde als voertuigsoort
        public void zetModel(string model) { }
        public void zetNummerplaat(string nummerplaat) { Nummerplaat = nummerplaat; }
        public void zetChasisnummer(string chasisnummer) { Chassisnummer = chasisnummer; }
        public void zetBrandstof(Brandstof brandstof) { Brandstof = brandstof; }
        public void zetVoertuigSoort(Voertuigsoort voertuigsoort) { Soort = voertuigsoort; }
        public void zetKleur(string kleur) {
            Kleur = kleur;
        }
        public void zetAantalDeuren(int aantal) {
            AantalDeuren = aantal;
        }
#nullable disable
        public void zetBestuurder(Bestuurder bestuurder)
        {
            try
            {
                if (voertuigBestuurder.Keys.Contains(bestuurder))
                {
                    throw new VoertuigException("Bestuurder hoort al bij een wagen");
                }
                else
                {
                    voertuigBestuurder.Add(bestuurder, bestuurder.Voertuig);
                }
                Bestuurder = bestuurder;
            }catch (Exception ex)
            {
                throw new VoertuigException("Voertuig", ex);
            }
        }


    }
}
