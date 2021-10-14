using System.Collections.Generic;

namespace BusinessLaag.Interfaces
{
    public interface ITankkaartManager
    {
        Tankkaart fetchTankkaartDetail(int id);
        IEnumerable<Tankkaart> fetchTankkaarten();
        IEnumerable<string> fetchTankkaartProperties();
        void updateTankkaart(Tankkaart tankkaart);
        void verwijderTankkaart(Tankkaart tankkaart);
        void voegTankkaartToe(Tankkaart tankkaart);
        IEnumerable<Tankkaart> zoekTankkaarten();
    }
}