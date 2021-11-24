using BusinessLaag;
using BusinessLaag.Interfaces;
using BusinessLaag.Model;
using DataLaag;
using DataLaag.Repositories;
using System;
using System.Collections.Generic;
using WPFApp.Interfaces;
using WPFApp.Model.Request;
using WPFApp.Model.Response;
using WPFApp.Model.Mappers.Business;
using WPFApp.Exceptions;

// De Businesscommuniceerder heeft als enigste klasse dependency op de business laag.
// In het geval dat er een API gebruikt wordt zal deze de verantwoordelijkheid voor het
// beheren van de dependency en het aanmaken van de FleetManager op zich moeten nemen.

namespace WPFApp.Model.Communiceerders {
    internal class BusinessCommuniceerder : ICommuniceer {
        private FleetManager _fleetManager;

        public BusinessCommuniceerder() {
            _fleetManager = new(new VoertuigOpslag(), new BestuurderOpslag(), new TankkaartOpslag(), new DatabankConfigureerder(null));
        }

		#region databank info
		public DatabankStatusResponseDTO GeefDatabankStatus() {
            IDatabankConfigureerder db = _fleetManager.DatabankConfigureerder;
            return new DatabankStatusResponseDTO(db.ConnectieSuccesvol, db.DatabaseBestaat, db.AlleTabellenBestaan, db.AantalTabellen, db.SequentieDoorlopen);
        }
		#endregion

		#region Adres
		public int VoegAdresToe(AdresRequestDTO adres) {
			try {
				return _fleetManager.BestuurderManager.VoegAdresToe(
					RequestDTONaarDomein.ConverteerNaarAdres(adres)
				);
			} catch(Exception e) { throw new BusinessCommuniceerderException(e.Message, e); }
		}

		public List<AdresResponseDTO> GeefAdressen(string kolom = null, object waarde = null) {
			List<AdresResponseDTO> geconvAdressen = new();

			try {
				_fleetManager.BestuurderManager
							 .GeefAdressen()
							 .ForEach(a => geconvAdressen.Add(
												DomeinNaarResponseDTO.ConverteerAdres(a)
										   )
									 );

				return geconvAdressen;
			} catch (Exception e) { throw new BusinessCommuniceerderException(e.Message, e); }
		}

		public void UpdateAdres(AdresRequestDTO adres) {
			try {
				_fleetManager.BestuurderManager.UpdateAdres(
					RequestDTONaarDomein.ConverteerNaarAdres(adres)
				);
			} catch (Exception e) { throw new BusinessCommuniceerderException(e.Message, e); }
		}

		public void VerwijderAdres(int id) {
			try {
				_fleetManager.BestuurderManager.VerwijderAdres(
					id
				);
			} catch (Exception e) { throw new BusinessCommuniceerderException(e.Message, e); }
		}
		#endregion

		#region bestuurder
		public int VoegBestuurderToe(BestuurderRequestDTO bestuurder) {
			throw new NotImplementedException();
		}

		public List<BestuurderResponseDTO> GeefBestuurders(string kolom = null, object waarde = null) {
            var resultaten = _fleetManager.BestuurderManager.GeefBestuurders();
            List<BestuurderResponseDTO> geconverteerdeResultaten = new();

            foreach (Bestuurder b in resultaten) {
                geconverteerdeResultaten.Add(DomeinNaarResponseDTO.ConverteerBestuurder(b, true));
            }

            return geconverteerdeResultaten;
        }

		public BestuurderResponseDTO GeefBestuurderDetail(int id) {
            var resultaat = _fleetManager.BestuurderManager.GeefBestuurderDetail(id);
            return DomeinNaarResponseDTO.ConverteerBestuurder(resultaat, true);
        }

		public BestuurderResponseDTO GeefBestuurderZonderRelaties(int id) {
			throw new NotImplementedException();
		}

		public List<BestuurderResponseDTO> ZoekBestuurders(string kolom, object waarde) {
			throw new NotImplementedException();
		}

		public void UpdateBestuurder(BestuurderRequestDTO bestuurder) {
			throw new NotImplementedException();
		}

		public void VerwijderBestuurder(int id) {
			throw new NotImplementedException();
		}

		#endregion

		#region tankkaart
		public int VoegTankkaartToe(TankkaartRequestDTO tankkaart) {
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

		public TankkaartResponseDTO ZoekTankkaartMetKaartnummer(string kaartnummer) {
			throw new NotImplementedException();
		}

		public void UpdateTankkaart(TankkaartRequestDTO tankkaart) {
			throw new NotImplementedException();
		}

		public void VerwijderTankkaart(int id) {
			throw new NotImplementedException();
		}
		#endregion

		#region voertuig
		public int VoegVoertuigToe(VoertuigRequestDTO voertuig) {
			throw new NotImplementedException();
		}

		public VoertuigResponseDTO GeefVoertuigZonderRelaties(int id) {
			throw new NotImplementedException();
		}

		public VoertuigResponseDTO GeefVoertuigDetail(int id) {
			throw new NotImplementedException();
		}

		public List<VoertuigResponseDTO> GeefVoertuigen() {
			throw new NotImplementedException();
		}

		public VoertuigResponseDTO ZoekVoertuigMetChassisnummer(string chassisnummer) {
			throw new NotImplementedException();
		}

		public VoertuigResponseDTO ZoekVoertuigMetNummerplaat(string nummerplaat) {
			throw new NotImplementedException();
		}

		public void UpdateVoertuig(VoertuigRequestDTO Voertuig) {
			throw new NotImplementedException();
		}

		public void VerwijderVoertuig(int id) {
			throw new NotImplementedException();
		}
		#endregion
	}
}
