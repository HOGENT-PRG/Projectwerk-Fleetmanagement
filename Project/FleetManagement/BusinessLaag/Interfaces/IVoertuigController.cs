using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLaag.Interfaces
{
    public interface IVoertuigController
    {
        void voegVoertuigToe(Voertuig voertuig);
        void updateVoertuig(Voertuig voertuig);
        void verwijderVoertuig(Voertuig voertuig);
        IEnumerable<Voertuig> fetchVoertuigen();
        Voertuig fetchVoertuigDetail(int id);
        IEnumerable<Voertuig> zoekVoertuig();
        IEnumerable<string> fetchVoertuigProperties();
    }
}
