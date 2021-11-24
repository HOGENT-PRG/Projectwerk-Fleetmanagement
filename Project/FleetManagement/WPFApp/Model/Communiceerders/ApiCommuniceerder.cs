using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFApp.Exceptions;
using WPFApp.Interfaces;
using WPFApp.Model.Request;
using WPFApp.Model.Response;

// TODO bij uitwerken van API laag

namespace WPFApp.Model.Communiceerders {
    internal class ApiCommuniceerder : ICommuniceer {
        private readonly string API_BASIS_PAD;

        public ApiCommuniceerder(string api_basispad) {
            this.API_BASIS_PAD = api_basispad;
        }

		private string _voerAPIRequestUit(string jsonConstruct, string urlPad) {
			throw new NotImplementedException();
		}

		/*------------------------------->> Einde private methodes <<-------------------------------*/

		public List<AdresResponseDTO> GeefAdressen(string kolom = null, object waarde = null) {
			throw new NotImplementedException();
		}

		public BestuurderResponseDTO GeefBestuurderDetail(int id) {
			throw new NotImplementedException();
		}

		public List<BestuurderResponseDTO> GeefBestuurders(string kolom = null, object waarde = null) {
			throw new NotImplementedException();
		}

		public BestuurderResponseDTO GeefBestuurderZonderRelaties(int id) {
			throw new NotImplementedException();
		}

		public DatabankStatusResponseDTO GeefDatabankStatus() {
			throw new NotImplementedException();
		}

		public TankkaartResponseDTO GeefTankkaartDetail(int id) {
			throw new NotImplementedException();
		}

		public List<TankkaartResponseDTO> GeefTankkaarten() {
			throw new NotImplementedException();
		}

		public TankkaartResponseDTO GeefTankkaartZonderRelaties(int id) {
			throw new NotImplementedException();
		}

		public VoertuigResponseDTO GeefVoertuigDetail(int id) {
			throw new NotImplementedException();
		}

		public List<VoertuigResponseDTO> GeefVoertuigen() {
			throw new NotImplementedException();
		}

		public VoertuigResponseDTO GeefVoertuigZonderRelaties(int id) {
			throw new NotImplementedException();
		}

		public void UpdateAdres(AdresRequestDTO adres) {
			throw new NotImplementedException();
		}

		public void UpdateBestuurder(BestuurderRequestDTO bestuurder) {
			throw new NotImplementedException();
		}

		public void UpdateTankkaart(TankkaartRequestDTO tankkaart) {
			throw new NotImplementedException();
		}

		public void UpdateVoertuig(VoertuigRequestDTO Voertuig) {
			throw new NotImplementedException();
		}

		public void VerwijderAdres(int id) {
			throw new NotImplementedException();
		}

		public void VerwijderBestuurder(int id) {
			throw new NotImplementedException();
		}

		public void VerwijderTankkaart(int id) {
			throw new NotImplementedException();
		}

		public void VerwijderVoertuig(int id) {
			throw new NotImplementedException();
		}

		public int VoegAdresToe(AdresRequestDTO adres) {
			throw new NotImplementedException();
		}

		public int VoegBestuurderToe(BestuurderRequestDTO bestuurder) {
			throw new NotImplementedException();
		}

		public int VoegTankkaartToe(TankkaartRequestDTO tankkaart) {
			throw new NotImplementedException();
		}

		public int VoegVoertuigToe(VoertuigRequestDTO voertuig) {
			throw new NotImplementedException();
		}

		public List<BestuurderResponseDTO> ZoekBestuurders(string kolom, object waarde) {
			throw new NotImplementedException();
		}

		public TankkaartResponseDTO ZoekTankkaartMetKaartnummer(string kaartnummer) {
			throw new NotImplementedException();
		}

		public VoertuigResponseDTO ZoekVoertuigMetChassisnummer(string chassisnummer) {
			throw new NotImplementedException();
		}

		public VoertuigResponseDTO ZoekVoertuigMetNummerplaat(string nummerplaat) {
			throw new NotImplementedException();
		}
        

    }
}
