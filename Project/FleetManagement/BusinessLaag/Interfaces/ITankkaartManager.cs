using BusinessLaag.Model;
using System.Collections.Generic;

namespace BusinessLaag.Interfaces
{
    public interface ITankkaartManager
    {
        Tankkaart geefTankkaartDetail(int id);
        IEnumerable<Tankkaart> geefTankkaarten();
        IEnumerable<string> geefTankkaartProperties();
        bool updateTankkaart(Tankkaart tankkaart);
        bool verwijderTankkaart(Tankkaart tankkaart);
        bool voegTankkaartToe(Tankkaart tankkaart);
        IEnumerable<Tankkaart> zoekTankkaarten();
    }
}