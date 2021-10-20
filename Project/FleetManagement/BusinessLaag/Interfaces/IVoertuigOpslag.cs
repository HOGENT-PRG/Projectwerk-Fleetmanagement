using BusinessLaag.Model;
using System.Collections.Generic;

namespace BusinessLaag.Interfaces
{
    public interface IVoertuigOpslag
    {
        void ZetConnectionString(string connectionString);
        void voegVoertuigToe(Voertuig voertuig);
        void updateVoertuig(Voertuig voertuig);
        void verwijderVoertuig(Voertuig voertuig);
        IEnumerable<Voertuig> geefVoertuigen();
        Voertuig geefVoertuigDetail(int id);
        IEnumerable<Voertuig> zoekVoertuig();
        IEnumerable<string> geefVoertuigProperties();
    }
}