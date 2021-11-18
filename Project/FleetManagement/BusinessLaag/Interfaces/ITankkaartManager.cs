using BusinessLaag.Model;
using System.Collections.Generic;

namespace BusinessLaag.Interfaces
{
    public interface ITankkaartManager
    {
		Tankkaart GeefTankkaartDetail(int id);
		IEnumerable<Tankkaart> GeefTankkaarten();
		Tankkaart GeefTankkaartZonderRelaties(int id);
		void UpdateTankkaart(Tankkaart tankkaart);
		void VerwijderTankkaart(int id);
		int VoegTankkaartToe(Tankkaart tankkaart);
		Tankkaart ZoekTankkaartMetKaartnummer(string kaartnummer);
	}
}