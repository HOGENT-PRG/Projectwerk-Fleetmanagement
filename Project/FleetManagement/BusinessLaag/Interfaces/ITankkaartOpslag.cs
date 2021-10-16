using System.Collections.Generic;

namespace BusinessLaag.Interfaces
{
    public interface ITankkaartOpslag
    {
        void ZetConnectionString(string connectionString);
        void voegTankkaartToe(Tankkaart tankkaart);
        void updateTankkaart(Tankkaart tankkaart);
        void verwijderTankkaart(Tankkaart tankkaart);
        IEnumerable<Tankkaart> geefTankkaarten();
        Tankkaart geefTankkaartDetail(int id);
        IEnumerable<Tankkaart> zoekTankkaarten();
        IEnumerable<string> geefTankkaartProperties();
    }
}