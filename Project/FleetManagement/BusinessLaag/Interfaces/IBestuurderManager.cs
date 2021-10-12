using System.Collections.Generic;

namespace BusinessLaag.Interfaces
{
    public interface IBestuurderManager
    {
        Bestuurder fetchBestuurderDetail(int id);
        IEnumerable<string> fetchBestuurderProperties();
        IEnumerable<Bestuurder> fetchBestuurders();
        void updateBestuurder(Bestuurder bestuurder);
        void verwijderBestuurder(Bestuurder bestuurder);
        void voegBestuurderToe(Bestuurder bestuurder);
        IEnumerable<Bestuurder> zoekBestuurders();
    }
}