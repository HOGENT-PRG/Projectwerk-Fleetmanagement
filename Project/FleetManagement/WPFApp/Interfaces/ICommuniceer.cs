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
        /*
            Minimum functionaliteiten Business- en ApiCommuniceerder, bevat helper functies die gebruikt kunnen worden door een viewmodel (ValideerRRN bijvoorbeeld).

            Bevat tevens functies die de functionaliteiten van de managers voorstellen, er wordt geen onderscheid meer gemaakt op vlak van manager type. 
            Dit vormt echter geen groot probleem aangezien de function bodies gelimiteerd zijn in omvang, hun doel is namelijk louter om te communiceren met een provider (rechtstreeks met businesslaag of communicatie met api endpoint) en om te vormen van en naar DTO type.
        */
        /* Helper functies voor WPF */
        bool ValideerRRN(string rrn);

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
