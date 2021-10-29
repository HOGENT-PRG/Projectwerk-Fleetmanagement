using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BusinessLaag.Interfaces;
using BusinessLaag.Model;

namespace BusinessLaag.Managers
{
    public class VoertuigManager : IVoertuigManager
    {
        private static FleetManager _fleetManager;
        private IVoertuigOpslag _opslag;

        public VoertuigManager(FleetManager fleetmanager, IVoertuigOpslag repository)
        {
            _fleetManager = fleetmanager;
            _opslag = repository;
        }

        //Domeinregels bewaken en objecten voorzien van foreignkey objecten
        public Voertuig geefVoertuigDetail(int id)
        {
            // DAL geeft voertuig terug
            // ga na of dit voertuig een bestuurder heeft
            // indien ja, vraag deze op en zetbestuurder van voertuig
            // return
            throw new NotImplementedException();
        }

        public IEnumerable<Voertuig> geefVoertuigen()
        {
            // DAL geeft voertuigen terug
            // ga na of dit voertuig een bestuurder heeft
            // indien ja, vraag deze op en zetbestuurder van voertuig
            // return
            throw new NotImplementedException();
        }

        public bool updateVoertuig(Voertuig voertuig)
        {
            // chassisnummer bestaat al voor voertuig met ander voertuig.id? -- afbreken
            // nummerplaat bestaat al voor voertuig met ander voertuig.id? -- afbreken
            // bestuurder wijzigt? (fetch 1 en check)
                // oude bestuurder (if any) referentie naar voertuig verwijderen
                // updaten
            // return
            throw new NotImplementedException();
        }

        public bool verwijderVoertuig(Voertuig voertuig)
        {
            // indien een bestuurder aangewezen is -- afbreken (of verwijder relaties??)
            // verwijder bestuurder referentie naar voertuig
            // verwijder voertuig
            throw new NotImplementedException();
        }

        public bool voegVoertuigToe(Voertuig voertuig)
        {
            // chassisnummer bestaat al? -- afbreken
            // nummerplaat bestaat al? -- afbreken
            throw new NotImplementedException();
        }

        public IEnumerable<Voertuig> zoekVoertuig()
        {
            throw new NotImplementedException();
        }
    }
}
