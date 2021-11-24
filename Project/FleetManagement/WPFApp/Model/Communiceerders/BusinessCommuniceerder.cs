﻿using BusinessLaag;
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
using System.Reflection;
using System.Linq;

// De Businesscommuniceerder heeft als enigste klasse dependency op de business laag.
// In het geval dat er een API gebruikt wordt zal deze de verantwoordelijkheid voor het
// beheren van de dependency en het aanmaken van de FleetManager op zich moeten nemen.

// Functie return types zijn ResponseDTO(s) of POD types (string, int, ..)
// Functie argumenten zijn RequestDTO(s) of POD types (string, int, ..)

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
			} catch(Exception e) { throw new BusinessCommuniceerderException($"{MethodBase.GetCurrentMethod().Name} > {e.GetType().Name} :\n{e.Message}", e); }
		}

		public List<AdresResponseDTO> GeefAdressen(string kolom = null, object waarde = null) {
			List<AdresResponseDTO> adressen = new();

			try {
				_fleetManager.BestuurderManager
							 .GeefAdressen()
							 .ForEach(a => adressen.Add(
												DomeinNaarResponseDTO.ConverteerAdres(a)
										   )
									 );
			} catch (Exception e) { throw new BusinessCommuniceerderException($"{MethodBase.GetCurrentMethod().Name} > {e.GetType().Name} :\n{e.Message}", e); }

			return adressen;
		}

		public void UpdateAdres(AdresRequestDTO adres) {
			try {
				_fleetManager.BestuurderManager.UpdateAdres(
					RequestDTONaarDomein.ConverteerNaarAdres(adres)
				);
			} catch (Exception e) { throw new BusinessCommuniceerderException($"{MethodBase.GetCurrentMethod().Name} > {e.GetType().Name} :\n{e.Message}", e); }
		}

		public void VerwijderAdres(int id) {
			try {
				_fleetManager.BestuurderManager.VerwijderAdres(
					id
				);
			} catch (Exception e) { throw new BusinessCommuniceerderException($"{MethodBase.GetCurrentMethod().Name} > {e.GetType().Name} :\n{e.Message}", e); }
		}
		#endregion

		#region bestuurder
		public int VoegBestuurderToe(BestuurderRequestDTO bestuurder) {
			try {
				return _fleetManager.BestuurderManager.VoegBestuurderToe(
					RequestDTONaarDomein.ConverteerNaarBestuurder(bestuurder, true)
				);
			} catch (Exception e) { throw new BusinessCommuniceerderException($"{MethodBase.GetCurrentMethod().Name} > {e.GetType().Name} :\n{e.Message}", e); }
		}

		public List<BestuurderResponseDTO> GeefBestuurders(string kolom = null, object waarde = null) {
			List<BestuurderResponseDTO> res = new();

			try {
				_fleetManager.BestuurderManager.GeefBestuurders(
					kolom, waarde
				).ForEach(b => res.Add(
								DomeinNaarResponseDTO.ConverteerBestuurder(b, true)
						 ));
			} catch (Exception e) { throw new BusinessCommuniceerderException($"{MethodBase.GetCurrentMethod().Name} > {e.GetType().Name} :\n{e.Message}", e); }

			return res;
        }

		public BestuurderResponseDTO GeefBestuurderDetail(int id) {
			try {
				Bestuurder b = _fleetManager.BestuurderManager.GeefBestuurderDetail(id);
				if(b is null) { return null; }
				return DomeinNaarResponseDTO.ConverteerBestuurder(b, true);

			} catch (Exception e) { throw new BusinessCommuniceerderException($"{MethodBase.GetCurrentMethod().Name} > {e.GetType().Name} :\n{e.Message}", e); }
		}

		public BestuurderResponseDTO GeefBestuurderZonderRelaties(int id) {
			try {
				Bestuurder b = _fleetManager.BestuurderManager.GeefBestuurderZonderRelaties(id);
				if (b is null) { return null; }
				return DomeinNaarResponseDTO.ConverteerBestuurder(b, true);

			} catch (Exception e) { throw new BusinessCommuniceerderException($"{MethodBase.GetCurrentMethod().Name} > {e.GetType().Name} :\n{e.Message}", e); }
		}

		public List<BestuurderResponseDTO> ZoekBestuurders(string kolom, object waarde) {
			List<BestuurderResponseDTO> res = new();

			try {
				_fleetManager.BestuurderManager.ZoekBestuurders(
					kolom, waarde
				).ForEach(b => res.Add(
								DomeinNaarResponseDTO.ConverteerBestuurder(b, true)
						 ));
			} catch (Exception e) { throw new BusinessCommuniceerderException($"{MethodBase.GetCurrentMethod().Name} > {e.GetType().Name} :\n{e.Message}", e); }

			return res;
		}

		public void UpdateBestuurder(BestuurderRequestDTO bestuurder) {
			try {
				_fleetManager.BestuurderManager.UpdateBestuurder(
					RequestDTONaarDomein.ConverteerNaarBestuurder(bestuurder, true)
				);
			} catch (Exception e) { throw new BusinessCommuniceerderException($"{MethodBase.GetCurrentMethod().Name} > {e.GetType().Name} :\n{e.Message}", e); }
		}

		public void VerwijderBestuurder(int id) {
			try {
				_fleetManager.BestuurderManager.VerwijderBestuurder(
					id
				);
			} catch (Exception e) { throw new BusinessCommuniceerderException($"{MethodBase.GetCurrentMethod().Name} > {e.GetType().Name} :\n{e.Message}", e); }
		}

		#endregion

		#region tankkaart
		public int VoegTankkaartToe(TankkaartRequestDTO tankkaart) {
			try {
				return _fleetManager.TankkaartManager.VoegTankkaartToe(
					RequestDTONaarDomein.ConverteerNaarTankkaart(tankkaart, true)
				);
			} catch (Exception e) { throw new BusinessCommuniceerderException($"{MethodBase.GetCurrentMethod().Name} > {e.GetType().Name} :\n{e.Message}", e); }
		}

		public TankkaartResponseDTO GeefTankkaartDetail(int id) {
			try {
				Tankkaart t = _fleetManager.TankkaartManager.GeefTankkaartDetail(id);
				if(t is null) { return null; }
				return DomeinNaarResponseDTO.ConverteerTankkaart(t, true);

			} catch (Exception e) { throw new BusinessCommuniceerderException($"{MethodBase.GetCurrentMethod().Name} > {e.GetType().Name} :\n{e.Message}", e); }
		}

		public List<TankkaartResponseDTO> GeefTankkaarten() {
			List<TankkaartResponseDTO> res = new();

			try {
				_fleetManager.TankkaartManager.GeefTankkaarten()
					.ToList()
					.ForEach(t => res.Add(DomeinNaarResponseDTO.ConverteerTankkaart(t, true)));

			} catch (Exception e) { throw new BusinessCommuniceerderException($"{MethodBase.GetCurrentMethod().Name} > {e.GetType().Name} :\n{e.Message}", e); }

			return res;
		}

		public TankkaartResponseDTO GeefTankkaartZonderRelaties(int id) {
			try {
				Tankkaart t = _fleetManager.TankkaartManager.GeefTankkaartZonderRelaties(id);
				if(t is null) { return null; }
				return DomeinNaarResponseDTO.ConverteerTankkaart(t, true);

			} catch (Exception e) { throw new BusinessCommuniceerderException($"{MethodBase.GetCurrentMethod().Name} > {e.GetType().Name} :\n{e.Message}", e); }
		}

		public TankkaartResponseDTO ZoekTankkaartMetKaartnummer(string kaartnummer) {
			try {
				Tankkaart t = _fleetManager.TankkaartManager.ZoekTankkaartMetKaartnummer(
					kaartnummer
				);
				if(t is null) { return null; }
				return DomeinNaarResponseDTO.ConverteerTankkaart(t, true);

			} catch (Exception e) { throw new BusinessCommuniceerderException($"{MethodBase.GetCurrentMethod().Name} > {e.GetType().Name} :\n{e.Message}", e); }
		}

		public void UpdateTankkaart(TankkaartRequestDTO tankkaart) {
			try {
				_fleetManager.TankkaartManager.UpdateTankkaart(
					RequestDTONaarDomein.ConverteerNaarTankkaart(tankkaart, true)
				);
			} catch (Exception e) { throw new BusinessCommuniceerderException($"{MethodBase.GetCurrentMethod().Name} > {e.GetType().Name} :\n{e.Message}", e); }
		}

		public void VerwijderTankkaart(int id) {
			try {
				_fleetManager.TankkaartManager.VerwijderTankkaart(
					id
				);
			} catch (Exception e) { throw new BusinessCommuniceerderException($"{MethodBase.GetCurrentMethod().Name} > {e.GetType().Name} :\n{e.Message}", e); }
		}
		#endregion

		#region voertuig
		public int VoegVoertuigToe(VoertuigRequestDTO voertuig) {
			try {
				return _fleetManager.VoertuigManager.VoegVoertuigToe(
					RequestDTONaarDomein.ConverteerNaarVoertuig(voertuig, true)
				);
			} catch (Exception e) { throw new BusinessCommuniceerderException($"{MethodBase.GetCurrentMethod().Name} > {e.GetType().Name} :\n{e.Message}", e); }
		}

		public VoertuigResponseDTO GeefVoertuigZonderRelaties(int id) {
			try {
				Voertuig v = _fleetManager.VoertuigManager.GeefVoertuigZonderRelaties(id);
				if(v is null) { return null; }
				return DomeinNaarResponseDTO.ConverteerVoertuig(v, true);

			} catch (Exception e) { throw new BusinessCommuniceerderException($"{MethodBase.GetCurrentMethod().Name} > {e.GetType().Name} :\n{e.Message}", e); }
		}

		public VoertuigResponseDTO GeefVoertuigDetail(int id) {
			try {
				Voertuig v = _fleetManager.VoertuigManager.GeefVoertuigDetail(id);
				if (v is null) { return null; }
				return DomeinNaarResponseDTO.ConverteerVoertuig(v, true);

			} catch (Exception e) { throw new BusinessCommuniceerderException($"{MethodBase.GetCurrentMethod().Name} > {e.GetType().Name} :\n{e.Message}", e); }
		}

		public List<VoertuigResponseDTO> GeefVoertuigen() {
			List<VoertuigResponseDTO> res = new();

			try {
				_fleetManager.VoertuigManager.GeefVoertuigen()
					.ForEach(v => res.Add(DomeinNaarResponseDTO.ConverteerVoertuig(v, true)));

			} catch (Exception e) { throw new BusinessCommuniceerderException($"{MethodBase.GetCurrentMethod().Name} > {e.GetType().Name} :\n{e.Message}", e); }

			return res;
		}

		public VoertuigResponseDTO ZoekVoertuigMetChassisnummer(string chassisnummer) {
			try {
				Voertuig v = _fleetManager.VoertuigManager.ZoekVoertuigMetChassisnummer(chassisnummer);
				if(v is null) { return null; }
				return DomeinNaarResponseDTO.ConverteerVoertuig(v, true);

			} catch (Exception e) { throw new BusinessCommuniceerderException($"{MethodBase.GetCurrentMethod().Name} > {e.GetType().Name} :\n{e.Message}", e); }
		}

		public VoertuigResponseDTO ZoekVoertuigMetNummerplaat(string nummerplaat) {
			try {
				Voertuig v = _fleetManager.VoertuigManager.ZoekVoertuigMetNummerplaat(nummerplaat);
				if (v is null) { return null; }
				return DomeinNaarResponseDTO.ConverteerVoertuig(v, true);

			} catch (Exception e) { throw new BusinessCommuniceerderException($"{MethodBase.GetCurrentMethod().Name} > {e.GetType().Name} :\n{e.Message}", e); }
		}

		public void UpdateVoertuig(VoertuigRequestDTO Voertuig) {
			try {
				_fleetManager.VoertuigManager.UpdateVoertuig(
					RequestDTONaarDomein.ConverteerNaarVoertuig(Voertuig, true)
				);
			} catch (Exception e) { throw new BusinessCommuniceerderException($"{MethodBase.GetCurrentMethod().Name} > {e.GetType().Name} :\n{e.Message}", e); }
		}

		public void VerwijderVoertuig(int id) {
			try {
				_fleetManager.VoertuigManager.VerwijderVoertuig(
					id
				);
			} catch (Exception e) { throw new BusinessCommuniceerderException($"{MethodBase.GetCurrentMethod().Name} > {e.GetType().Name} :\n{e.Message}", e); }
		}
		#endregion
	}
}