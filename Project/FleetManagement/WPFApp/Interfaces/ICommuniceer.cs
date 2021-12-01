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
        List<AdresResponseDTO> GeefAdressen(string? kolom = null, object? waarde = null);
        void UpdateAdres(AdresRequestDTO adres);
        void VerwijderAdres(int id);

        int VoegBestuurderToe(BestuurderRequestDTO bestuurder);
        List<BestuurderResponseDTO> GeefBestuurders();
        List<BestuurderResponseDTO> GeefBestuurders(string? kolom = null, object? waarde = null);
        BestuurderResponseDTO GeefBestuurderDetail(int id);
        BestuurderResponseDTO GeefBestuurderZonderRelaties(int id);
        List<BestuurderResponseDTO> ZoekBestuurders(string kolom, object waarde);
        void UpdateBestuurder(BestuurderRequestDTO bestuurder);
        void VerwijderBestuurder(int id);


        /* Tankkaart */
        int VoegTankkaartToe(TankkaartRequestDTO tankkaart);
        TankkaartResponseDTO GeefTankkaartDetail(int id);
        List<TankkaartResponseDTO> GeefTankkaarten();
        TankkaartResponseDTO GeefTankkaartZonderRelaties(int id);
        TankkaartResponseDTO ZoekTankkaartMetKaartnummer(string kaartnummer);
        void UpdateTankkaart(TankkaartRequestDTO tankkaart);
        void VerwijderTankkaart(int id);


        /* Voertuig */
        int VoegVoertuigToe(VoertuigRequestDTO voertuig);
        VoertuigResponseDTO GeefVoertuigZonderRelaties(int id);
        VoertuigResponseDTO GeefVoertuigDetail(int id);
        List<VoertuigResponseDTO> GeefVoertuigen();
        VoertuigResponseDTO ZoekVoertuigMetChassisnummer(string chassisnummer);
        VoertuigResponseDTO ZoekVoertuigMetNummerplaat(string nummerplaat);
        void UpdateVoertuig(VoertuigRequestDTO Voertuig);
        void VerwijderVoertuig(int id);


        /* Databank */
        DatabankStatusResponseDTO GeefDatabankStatus();
    }
}
