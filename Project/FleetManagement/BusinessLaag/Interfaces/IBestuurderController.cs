using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLaag.Interfaces
{
    public interface IBestuurderController
    {
        void voegBestuurderToe(Bestuurder bestuurder);
        void updateBestuurder(Bestuurder bestuurder);
        void verwijderBestuurder(Bestuurder bestuurder);
        IEnumerable<Bestuurder> fetchBestuurders();
        Bestuurder fetchBestuurderDetail(int id);
        IEnumerable<Bestuurder> zoekBestuurders();
        IEnumerable<string> fetchBestuurderProperties();

    }
}
