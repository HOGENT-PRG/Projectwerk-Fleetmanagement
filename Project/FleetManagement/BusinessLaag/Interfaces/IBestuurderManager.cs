using BusinessLaag.Model;
using System.Collections.Generic;

namespace BusinessLaag.Interfaces
{
    public interface IBestuurderManager
    {
        Bestuurder geefBestuurderDetail(int id);
        IEnumerable<string> geefBestuurderProperties();
        IEnumerable<Bestuurder> geefBestuurders();
        void updateBestuurder(Bestuurder bestuurder);
        void verwijderBestuurder(Bestuurder bestuurder);
        void voegBestuurderToe(Bestuurder bestuurder);
        IEnumerable<Bestuurder> zoekBestuurders();
    }
}