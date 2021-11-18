using BusinessLaag.Model;
using System.Collections.Generic;

namespace BusinessLaag.Interfaces
{
    public interface IBestuurderManager
    {
        Bestuurder GeefBestuurderDetail(int id);
        List<Bestuurder> GeefBestuurders(string? kolom=null, object? waarde=null);
        void UpdateBestuurder(Bestuurder bestuurder);
        void VerwijderBestuurder(int id);
        int VoegBestuurderToe(Bestuurder bestuurder);
        List<Bestuurder> ZoekBestuurders(string kolom, object waarde);

        int VoegAdresToe(Adres adres);
        List<Adres> GeefAdressen(string? kolom = null, object? waarde = null);
        void UpdateAdres(Adres adres);
        void VerwijderAdres(int id);
    }
}