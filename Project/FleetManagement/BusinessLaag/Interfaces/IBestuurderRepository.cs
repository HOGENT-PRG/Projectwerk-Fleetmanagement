using System.Collections.Generic;

namespace BusinessLaag.Interfaces
{
    public interface IBestuurderRepository
    {
        void ZetConnectionString(string connectionString);
        void voegBestuurderToe(Bestuurder bestuurder);
        void updateBestuurder(Bestuurder bestuurder);
        void verwijderBestuurder(Bestuurder bestuurder);
        IEnumerable<Bestuurder> fetchBestuurders();
        Bestuurder fetchBestuurderDetail(int id);
        IEnumerable<Bestuurder> zoekBestuurders();
        IEnumerable<string> fetchBestuurderProperties();
    }
}