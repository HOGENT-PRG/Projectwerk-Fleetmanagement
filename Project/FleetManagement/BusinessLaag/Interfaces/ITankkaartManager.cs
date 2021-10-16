using System.Collections.Generic;

namespace BusinessLaag.Interfaces
{
    public interface ITankkaartManager
    {
        Tankkaart geefTankkaartDetail(int id);
        IEnumerable<Tankkaart> geefTankkaarten();
        IEnumerable<string> geefTankkaartProperties();
        void updateTankkaart(Tankkaart tankkaart);
        void verwijderTankkaart(Tankkaart tankkaart);
        void voegTankkaartToe(Tankkaart tankkaart);
        IEnumerable<Tankkaart> zoekTankkaarten();
    }
}