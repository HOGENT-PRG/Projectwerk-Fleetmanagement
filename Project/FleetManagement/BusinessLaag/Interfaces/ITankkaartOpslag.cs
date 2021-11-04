using BusinessLaag.Model;
using System.Collections.Generic;

namespace BusinessLaag.Interfaces
{
    public interface ITankkaartOpslag
    {
        void ZetConnectionString(string connectionString);
        void voegTankkaartToe(Tankkaart tankkaart);
        void updateTankkaart(Tankkaart tankkaart);
        void verwijderTankkaart(Tankkaart tankkaart);
        List<KeyValuePair<int?, Tankkaart>> GeefTankkaarten();
        KeyValuePair<int?, Tankkaart> GeefTankkaartDetail(int id);
        IEnumerable<Tankkaart> zoekTankkaarten(string kolom,string waarde);
        IEnumerable<string> geefTankkaartProperties();
        int voegTankkaartToe(int? id);
    }
}