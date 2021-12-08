using BusinessLaag.Model;
using System.Collections.Generic;

namespace BusinessLaag.Interfaces
{
    public interface IVoertuigOpslag
    {
		Voertuig GeefVoertuigDetail(int id);
		List<Voertuig> GeefVoertuigen(string kolomnaam = null, object waarde = null);
		List<Voertuig> ZoekVoertuigen(List<string> kolomnamen, List<object> zoektermen, bool likeWildcard = false);
		void UpdateVoertuig(Voertuig voertuig);
		void VerwijderVoertuig(int id);
		int VoegVoertuigToe(Voertuig voertuig);
		void ZetConnectionString(string connString);
		Voertuig ZoekVoertuig(string kolomnaamHoofdletterGevoelig, string waarde);
	}
}