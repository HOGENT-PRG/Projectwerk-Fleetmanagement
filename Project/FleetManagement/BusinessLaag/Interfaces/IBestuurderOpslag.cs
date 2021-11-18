using BusinessLaag.Model;
using System.Collections.Generic;

namespace BusinessLaag.Interfaces
{
    public interface IBestuurderOpslag
    {
        void ZetConnectionString(string connectionString);
        int voegBestuurderToe(Bestuurder bestuurder);
        void updateBestuurder(Bestuurder bestuurder);
        void verwijderBestuurder(Bestuurder bestuurder);
        List<Bestuurder> geefBestuurders();
        Bestuurder geefBestuurderDetail(int id);
        List<Bestuurder> zoekBestuurders();
    }
}