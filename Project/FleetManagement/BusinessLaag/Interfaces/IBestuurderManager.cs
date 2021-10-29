using BusinessLaag.Model;
using System.Collections.Generic;

namespace BusinessLaag.Interfaces
{
    public interface IBestuurderManager
    {
        Bestuurder geefBestuurderDetail(int id);
        IEnumerable<Bestuurder> geefBestuurders();
        bool updateBestuurder(Bestuurder bestuurder);
        bool verwijderBestuurder(Bestuurder bestuurder);
        bool voegBestuurderToe(Bestuurder bestuurder);
        IEnumerable<Bestuurder> zoekBestuurders();
    }
}