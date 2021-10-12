using System.Collections.Generic;

namespace BusinessLaag.Interfaces
{
    public interface IVoertuigRepository
    {
        void ZetConnectionString(string connectionString);
        void voegVoertuigToe(Voertuig voertuig);
        void updateVoertuig(Voertuig voertuig);
        void verwijderVoertuig(Voertuig voertuig);
        IEnumerable<Voertuig> fetchVoertuigen();
        Voertuig fetchVoertuigDetail(int id);
        IEnumerable<Voertuig> zoekVoertuig();
        IEnumerable<string> fetchVoertuigProperties();
    }
}