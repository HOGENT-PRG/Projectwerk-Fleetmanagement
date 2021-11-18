using BusinessLaag.Model;
using System.Collections.Generic;

namespace BusinessLaag.Interfaces
{
    public interface ITankkaartOpslag
    {
		Tankkaart GeefTankkaartDetail(int id);
		List<Tankkaart> GeefTankkaarten(string kolomnaam = null, object waarde = null);
		void UpdateTankkaart(Tankkaart tankkaart);
		void VerwijderTankkaart(int id);
		int VoegTankkaartToe(Tankkaart tankkaart);
		void ZetConnectionString(string connString);
		List<Tankkaart> ZoekTankkaarten(string kolom, object waarde);
	}
}