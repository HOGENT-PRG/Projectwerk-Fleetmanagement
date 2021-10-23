using BusinessLaag.Model;
using System.Collections.Generic;

namespace BusinessLaag.Interfaces
{
    public interface IVoertuigManager
    {
        Voertuig geefVoertuigDetail(int id);
        IEnumerable<Voertuig> geefVoertuigen();
        IEnumerable<string> geefVoertuigProperties();
        bool updateVoertuig(Voertuig voertuig);
        bool verwijderVoertuig(Voertuig voertuig);
        bool voegVoertuigToe(Voertuig voertuig);
        IEnumerable<Voertuig> zoekVoertuig();
    }
}