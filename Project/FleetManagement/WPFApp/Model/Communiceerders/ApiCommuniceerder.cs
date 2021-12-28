using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFApp.Exceptions;
using WPFApp.Interfaces;
using WPFApp.Model.Request;
using WPFApp.Model.Response;

// Communiceert met endpoints (wellicht interessant om endpoints te verzamelen in een record) en zet om van en naar DTO's en json strings
// Indien ontwikkeling aangevat wordt dienen de funcs verzameld te worden per groep als region
// Adres bij adres, etc - nu staan ze alfabetisch door automatisch aanvullen met quick actions

namespace WPFApp.Model.Communiceerders {
	internal class ApiCommuniceerder : ICommuniceer {
		private readonly string API_BASIS_PAD;

		public ApiCommuniceerder(string api_basispad) {
			this.API_BASIS_PAD = api_basispad;
		}

		private string _voerAPIRequestUit(string json, string urlPad) {
			throw new NotImplementedException();
		}

		/*------------------------------->> Einde private methodes <<-------------------------------*/

		public List<AdresResponseDTO> GeefAdressen() {
			throw new NotImplementedException();
		}

		public List<AdresResponseDTO> GeefAdressen(string kolom = null, object waarde = null) {
			throw new NotImplementedException();
		}

		public BestuurderResponseDTO GeefBestuurderDetail(int id) {
			throw new NotImplementedException();
		}

		public List<BestuurderResponseDTO> GeefBestuurders() {
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

		public List<AdresResponseDTO> ZoekAdressen(List<string> kolomnamen, List<object> zoektermen, bool likeWildcard = false) {
			throw new NotImplementedException();
		}

		public List<BestuurderResponseDTO> ZoekBestuurders(List<string> kolomnamen, List<object> zoektermen, bool likeWildcard = false) {
			throw new NotImplementedException();
		}

		public List<BestuurderResponseDTO> ZoekBestuurders(string kolom, object waarde) {
			throw new NotImplementedException();
		}

		public List<TankkaartResponseDTO> ZoekTankkaarten(List<string> kolomnamen, List<object> zoektermen, bool likeWildcard = false) {
			throw new NotImplementedException();
		}

		public TankkaartResponseDTO ZoekTankkaartMetKaartnummer(string kaartnummer) {
			throw new NotImplementedException();
		}

		public List<VoertuigResponseDTO> ZoekVoertuigen(List<string> kolomnamen, List<object> zoektermen, bool likeWildcard = false) {
			throw new NotImplementedException();
		}

		public VoertuigResponseDTO ZoekVoertuigMetChassisnummer(string chassisnummer) {
			throw new NotImplementedException();
		}

		public VoertuigResponseDTO ZoekVoertuigMetNummerplaat(string nummerplaat) {
			throw new NotImplementedException();
		}

		public bool ValideerRRN(string rrn) {
			throw new NotImplementedException();
		}
	}
}
