using System.Collections.Generic;

namespace BusinessLaag.Interfaces
{
    public interface IVoertuigManager
    {
        Voertuig fetchVoertuigDetail(int id);
        IEnumerable<Voertuig> fetchVoertuigen();
        IEnumerable<string> fetchVoertuigProperties();
        void updateVoertuig(Voertuig voertuig);
        void verwijderVoertuig(Voertuig voertuig);
        void voegVoertuigToe(Voertuig voertuig);
        IEnumerable<Voertuig> zoekVoertuig();
    }
}