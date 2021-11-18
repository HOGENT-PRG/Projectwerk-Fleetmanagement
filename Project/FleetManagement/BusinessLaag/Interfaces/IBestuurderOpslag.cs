using BusinessLaag.Model;
using System.Collections.Generic;

namespace BusinessLaag.Interfaces
{
    public interface IBestuurderOpslag
    {
        void ZetConnectionString(string connectionString);
        int VoegBestuurderToe(Bestuurder bestuurder);
        void UpdateBestuurder(Bestuurder bestuurder);
        void VerwijderBestuurder(int id);
        List<Bestuurder> GeefBestuurders(string? kolom=null, object? waarde=null);
        Bestuurder GeefBestuurderDetail(int id);

        int VoegAdresToe(Adres adres);
        List<Adres> GeefAdressen(string? kolom = null, object? waarde = null);
        void UpdateAdres(Adres adres);
        void VerwijderAdres(int id);
    }
}