using BusinessLaag.Model;
using System.Collections.Generic;

namespace BusinessLaag.Interfaces
{
    public interface IVoertuigOpslag
    {
        void ZetConnectionString(string connectionString);
        int VoegVoertuigToe(Voertuig voertuig);
        void UpdateVoertuig(Voertuig voertuig);
        void VerwijderVoertuig(Voertuig voertuig);
        List<KeyValuePair<int?, Voertuig>> GeefVoertuigen();
        KeyValuePair<int?, Voertuig> GeefVoertuigDetail(int id);
        Voertuig ZoekVoertuig(string kolomNaamHoofdletterGevoelig, string waarde);
    }
}