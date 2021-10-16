using System.Collections.Generic;

namespace BusinessLaag.Interfaces
{
    public interface IBestuurderOpslag
    {
        void ZetConnectionString(string connectionString);
        void voegBestuurderToe(Bestuurder bestuurder);
        void updateBestuurder(Bestuurder bestuurder);
        void verwijderBestuurder(Bestuurder bestuurder);
        IEnumerable<Bestuurder> geefBestuurders();
        Bestuurder geefBestuurderDetail(int id);
        IEnumerable<Bestuurder> zoekBestuurders();
        IEnumerable<string> geefBestuurderProperties();
    }
}