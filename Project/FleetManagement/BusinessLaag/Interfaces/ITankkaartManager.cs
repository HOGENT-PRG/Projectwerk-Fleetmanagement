using BusinessLaag.Model;
using System.Collections.Generic;

namespace BusinessLaag.Interfaces
{
    public interface ITankkaartManager
    {
        Tankkaart geefTankkaartDetail(int id);
        IEnumerable<Tankkaart> geefTankkaarten();
        void updateTankkaart(Tankkaart tankkaart);
        void verwijderTankkaart(Tankkaart tankkaart);
        void voegTankkaartToe(Tankkaart tankkaart);
        IEnumerable<Tankkaart> zoekTankkaarten();
    }
}