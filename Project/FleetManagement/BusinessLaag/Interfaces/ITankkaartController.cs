using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLaag.Interfaces
{
    public interface ITankkaartController
    {
        void voegTankkaartToe(Tankkaart tankkaart);
        void updateTankkaart(Tankkaart tankkaart);
        void verwijderTankkaart(Tankkaart tankkaart);
        IEnumerable<Tankkaart> fetchTankkaarten();
        Tankkaart fetchTankkaartDetail(int id);
        IEnumerable<Tankkaart> zoekTankkaarten();
        IEnumerable<string> fetchTankkaartProperties();
    }
}
