using System.Collections.Generic;

namespace BusinessLaag.Interfaces
{
    public interface IVoertuigManager
    {
        Voertuig geefVoertuigDetail(int id);
        IEnumerable<Voertuig> geefVoertuigen();
        IEnumerable<string> geefVoertuigProperties();
        void updateVoertuig(Voertuig voertuig);
        void verwijderVoertuig(Voertuig voertuig);
        void voegVoertuigToe(Voertuig voertuig);
        IEnumerable<Voertuig> zoekVoertuig();
    }
}