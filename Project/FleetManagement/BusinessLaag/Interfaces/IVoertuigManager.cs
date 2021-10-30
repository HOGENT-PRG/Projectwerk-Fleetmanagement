using BusinessLaag.Model;
using System.Collections.Generic;

namespace BusinessLaag.Interfaces
{
    public interface IVoertuigManager
    {
        Voertuig GeefVoertuigDetail(int id);
        List<Voertuig> GeefVoertuigen();
        Voertuig GeefVoertuigZonderRelaties(int id);
        void UpdateVoertuig(Voertuig NieuweVoertuigVersie, bool negeerBestuurder = false);
        void VerwijderVoertuig(Voertuig voertuig);
        void VoegVoertuigToe(Voertuig voertuig);
        Voertuig ZoekVoertuigMetChassisnummer(string chassisnummer);
        Voertuig ZoekVoertuigMetNummerplaat(string nummerplaat);
    }
}