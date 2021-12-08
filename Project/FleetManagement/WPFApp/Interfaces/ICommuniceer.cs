using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFApp.Model;
using WPFApp.Model.Request;
using WPFApp.Model.Response;

namespace WPFApp.Interfaces {
    public interface ICommuniceer {

        /* Adres & Bestuurder */
        int VoegAdresToe(AdresRequestDTO adres);
        List<AdresResponseDTO> GeefAdressen();
        List<AdresResponseDTO> ZoekAdressen(List<string> kolomnamen, List<object> zoektermen, bool likeWildcard = false);
        void UpdateAdres(AdresRequestDTO adres);
        void VerwijderAdres(int id);

        int VoegBestuurderToe(BestuurderRequestDTO bestuurder);
        List<BestuurderResponseDTO> GeefBestuurders();
        List<BestuurderResponseDTO> ZoekBestuurders(List<string> kolomnamen, List<object> zoektermen, bool likeWildcard = false);
        BestuurderResponseDTO GeefBestuurderDetail(int id);
        BestuurderResponseDTO GeefBestuurderZonderRelaties(int id);
        void UpdateBestuurder(BestuurderRequestDTO bestuurder);
        void VerwijderBestuurder(int id);


        /* Tankkaart */
        int VoegTankkaartToe(TankkaartRequestDTO tankkaart);
        TankkaartResponseDTO GeefTankkaartDetail(int id);
        List<TankkaartResponseDTO> GeefTankkaarten();
        List<TankkaartResponseDTO> ZoekTankkaarten(List<string> kolomnamen, List<object> zoektermen, bool likeWildcard = false);
        TankkaartResponseDTO GeefTankkaartZonderRelaties(int id);
        void UpdateTankkaart(TankkaartRequestDTO tankkaart);
        void VerwijderTankkaart(int id);


        /* Voertuig */
        int VoegVoertuigToe(VoertuigRequestDTO voertuig);
        VoertuigResponseDTO GeefVoertuigZonderRelaties(int id);
        VoertuigResponseDTO GeefVoertuigDetail(int id);
        List<VoertuigResponseDTO> GeefVoertuigen();
        List<VoertuigResponseDTO> ZoekVoertuigen(List<string> kolomnamen, List<object> zoektermen, bool likeWildcard = false);
        void UpdateVoertuig(VoertuigRequestDTO Voertuig);
        void VerwijderVoertuig(int id);


        /* Databank */
        DatabankStatusResponseDTO GeefDatabankStatus();
    }
}
