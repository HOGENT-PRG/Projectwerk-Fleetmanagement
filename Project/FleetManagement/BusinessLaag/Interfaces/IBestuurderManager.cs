using BusinessLaag.Model;
using System.Collections.Generic;

namespace BusinessLaag.Interfaces
{
    public interface IBestuurderManager
    {
        Bestuurder GeefBestuurderDetail(int id);
        IEnumerable<Bestuurder> geefBestuurders();
        bool UpdateBestuurder(Bestuurder bestuurder);
        bool verwijderBestuurder(Bestuurder bestuurder);
        bool voegBestuurderToe(Bestuurder bestuurder);
        IEnumerable<Bestuurder> zoekBestuurders();
    }
}