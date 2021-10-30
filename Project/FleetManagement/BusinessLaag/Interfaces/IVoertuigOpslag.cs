using BusinessLaag.Model;
using System.Collections.Generic;

namespace BusinessLaag.Interfaces
{
    public interface IVoertuigOpslag
    {
        void ZetConnectionString(string connectionString);
        Voertuig VoegVoertuigToe(Voertuig voertuig);
        void UpdateVoertuig(Voertuig voertuig);
        void VerwijderVoertuig(Voertuig voertuig);
        Dictionary<int?, Voertuig> GeefVoertuigen();
        KeyValuePair<int?, Voertuig> GeefVoertuigDetail(int id);
        Voertuig ZoekVoertuig(string kolomNaam, string chassisnummer);
    }
}