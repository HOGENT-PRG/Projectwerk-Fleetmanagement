using System.Collections.Generic;

namespace BusinessLaag.Interfaces
{
    public interface ITankkaartRepository
    {
        void ZetConnectionString(string connectionString);
        void voegTankkaartToe(Tankkaart tankkaart);
        void updateTankkaart(Tankkaart tankkaart);
        void verwijderTankkaart(Tankkaart tankkaart);
        IEnumerable<Tankkaart> fetchTankkaarten();
        Tankkaart fetchTankkaartDetail(int id);
        IEnumerable<Tankkaart> zoekTankkaarten();
        IEnumerable<string> fetchTankkaartProperties();
    }
}