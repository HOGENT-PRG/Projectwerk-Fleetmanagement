using BusinessLaag.Model;
using System.Collections.Generic;

namespace BusinessLaag.Interfaces
{
    public interface ITankkaartManager
    {
		Tankkaart GeefTankkaartDetail(int id);
		IEnumerable<Tankkaart> GeefTankkaarten();
		List<Tankkaart> ZoekTankkaarten(List<string> kolomnamen, List<object> zoektermen, bool likeWildcard = false);
		Tankkaart GeefTankkaartZonderRelaties(int id);
		void UpdateTankkaart(Tankkaart tankkaart);
		void VerwijderTankkaart(int id);
		int VoegTankkaartToe(Tankkaart tankkaart);
		Tankkaart ZoekTankkaartMetKaartnummer(string kaartnummer);
	}
}