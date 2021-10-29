using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFApp.Model.Request;
using WPFApp.Model.Response;

namespace WPFApp.Interfaces {
    public interface ICommuniceer {
        List<AdresResponseDTO> geefAdressen();

        BestuurderResponseDTO geefBestuurderDetail(int tankkaartId);
        List<BestuurderResponseDTO> geefBestuurders(bool inclusiefRelaties);
        bool updateBestuurder(BestuurderRequestDTO bestuurder);
        bool verwijderBestuurder(int bestuurderId);
        bool voegBestuurderToe(BestuurderRequestDTO bestuurder);
        List<BestuurderResponseDTO> zoekBestuurders();


        TankkaartResponseDTO geefTankkaartDetail(int tankkaartId);
        List<TankkaartResponseDTO> geefTankkaarten();
        bool updateTankkaart(TankkaartRequestDTO tankkaart);
        bool verwijderTankkaart(int tankkaartId);
        bool voegTankkaartToe(TankkaartRequestDTO tankkaart);
        List<TankkaartResponseDTO> zoekTankkaarten();


        VoertuigResponseDTO geefVoertuigDetail(int voertuigId);
        List<VoertuigResponseDTO> geefVoertuigen();
        bool updateVoertuig(VoertuigRequestDTO voertuig);
        bool verwijderVoertuig(int voertuigId);
        bool voegVoertuigToe(VoertuigRequestDTO voertuig);
        List<VoertuigResponseDTO> zoekVoertuig();


        List<string> geefBestuurderProperties();
        List<string> geefTankkaartProperties();
        List<string> geefVoertuigProperties();

        DatabankStatusResponseDTO geefDatabankStatus();
    }
}
