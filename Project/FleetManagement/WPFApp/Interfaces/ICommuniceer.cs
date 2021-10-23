using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFApp.Model.Request;
using WPFApp.Model.Response;

namespace WPFApp.Interfaces {
    public interface ICommuniceer {
        BestuurderResponseDTO geefBestuurderDetail(int tankkaartId);
        IEnumerable<BestuurderResponseDTO> geefBestuurders();
        bool updateBestuurder(BestuurderRequestDTO bestuurder);
        bool verwijderBestuurder(int bestuurderId);
        bool voegBestuurderToe(BestuurderRequestDTO bestuurder);
        IEnumerable<BestuurderResponseDTO> zoekBestuurders();


        TankkaartResponseDTO geefTankkaartDetail(int tankkaartId);
        IEnumerable<TankkaartResponseDTO> geefTankkaarten();
        bool updateTankkaart(TankkaartRequestDTO tankkaart);
        bool verwijderTankkaart(int tankkaartId);
        bool voegTankkaartToe(TankkaartRequestDTO tankkaart);
        IEnumerable<TankkaartResponseDTO> zoekTankkaarten();


        VoertuigResponseDTO geefVoertuigDetail(int voertuigId);
        IEnumerable<VoertuigResponseDTO> geefVoertuigen();
        bool updateVoertuig(VoertuigRequestDTO voertuig);
        bool verwijderVoertuig(int voertuigId);
        bool voegVoertuigToe(VoertuigRequestDTO voertuig);
        IEnumerable<VoertuigResponseDTO> zoekVoertuig();


        IEnumerable<string> geefBestuurderProperties();
        IEnumerable<string> geefTankkaartProperties();
        IEnumerable<string> geefVoertuigProperties();
    }
}
