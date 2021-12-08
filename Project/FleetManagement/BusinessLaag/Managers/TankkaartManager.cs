using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLaag.Interfaces;
using BusinessLaag.Model;
using BusinessLaag.Exceptions;

namespace BusinessLaag.Managers
{
	public class TankkaartManager : ITankkaartManager {
		private static FleetManager _fleetManager;
		private ITankkaartOpslag _opslag;

		public TankkaartManager(FleetManager fleetmanager, ITankkaartOpslag repository) {
			_fleetManager = fleetmanager;
			_opslag = repository;
		}

		// -- Create
		public int VoegTankkaartToe(Tankkaart tankkaart) {
			if (tankkaart.Id is not null && tankkaart.Id > 0) {
				throw new TankkaartManagerException("Kan tankkaart niet toevoegen aangezien deze reeds een id bevat.");
			}

			if (ZoekTankkaartMetKaartnummer(tankkaart.Kaartnummer) is not null) {
				throw new TankkaartManagerException("kaartnummer is reeds toegewezen aan een tankkaart.");
			}

			if (tankkaart.Bestuurder is not null) {
				if (tankkaart.Bestuurder.Id is null) {
					throw new TankkaartManagerException("Tankkaart bevat een bestuurder maar deze bevat geen id.");
				}
			}

			try {
				return _opslag.VoegTankkaartToe(tankkaart);
			} catch (Exception e) {
				throw new TankkaartManagerException("Er is een onverwachte fout opgetreden.", e);
			}
		}

		// -- Read
		public Tankkaart GeefTankkaartDetail(int id) {
			if (id < 1) { throw new TankkaartManagerException("Id mag niet null zijn."); }

			try {
				return _opslag.GeefTankkaartDetail(id);
			} catch (Exception e) {
				throw new TankkaartManagerException("Tankkaart opvragen mislukt.", e);
			}
		}

		public IEnumerable<Tankkaart> GeefTankkaarten() {
			try {
				return _opslag.GeefTankkaarten();
			} catch (Exception e) {
				throw new TankkaartManagerException("Tankkaarten opvragen mislukt.", e);
			}
		}

		public List<Tankkaart> ZoekTankkaarten(List<string> kolomnamen, List<object> zoektermen, bool likeWildcard = false) {
			try {
				return _opslag.ZoekTankkaarten(kolomnamen, zoektermen, likeWildcard);
			} catch (Exception e) {
				throw new TankkaartManagerException("Tankkaarten zoeken mislukt.", e);
			}
		}

		public Tankkaart GeefTankkaartZonderRelaties(int id) {
			if (id < 1) { throw new TankkaartManagerException("Id mag niet kleiner zijn dan 1."); }
			try {
				Tankkaart t = _opslag.GeefTankkaartDetail(id);
				t.ZetBestuurder(null);
				return t;
			} catch (Exception e) {
				throw new TankkaartManagerException("Tankkaart opvragen mislukt.", e);
			}
		}

		private List<Tankkaart> _zoekTankkaartMetFilter(string kolomnaam, string waarde) {
			try {
				return _opslag.GeefTankkaarten(kolomnaam, waarde);
			} catch (Exception e) {
				throw new TankkaartManagerException("Tankkaarten zoeken mislukt.", e);
			}
		}

		public Tankkaart ZoekTankkaartMetKaartnummer(string kaartnummer) {
			var operatie = _zoekTankkaartMetFilter("Kaartnummer", kaartnummer);
			if (operatie.Count > 0) {
				return operatie.First();
			}
			return null;
		}

		// -- Update
		public void UpdateTankkaart(Tankkaart tankkaart) {
			if (tankkaart?.Id is null || tankkaart.Id <= 0) {
				throw new TankkaartManagerException("Tankkaart id mag niet null en < 1 zijn.");
			}

			try {
				Tankkaart BestaandeTankkaart = this.GeefTankkaartDetail((int)tankkaart.Id);
				if (BestaandeTankkaart is null) {
					throw new TankkaartManagerException("Kan geen tankkaart vinden voor opgegeven id. Er werden geen wijzigingen aangebracht.");
				}

				if (tankkaart.Bestuurder is not null) {
					if (tankkaart.Bestuurder.Id is null) {
						throw new TankkaartManagerException("Er is een bestuurder meegegeven maar deze bevat geen id.");
					}
					if (_fleetManager.BestuurderManager.GeefBestuurderDetail((int)tankkaart.Bestuurder.Id) is null) {
						throw new TankkaartManagerException($"De opgegeven bestuurder met id {tankkaart.Bestuurder.Id} bestaat niet.");
					}
				}

				if (BestaandeTankkaart.Kaartnummer != tankkaart.Kaartnummer) {
					Tankkaart KaartnummerCheck = this.ZoekTankkaartMetKaartnummer(tankkaart.Kaartnummer);

					if (KaartnummerCheck is not null) {
						if (KaartnummerCheck.Id != tankkaart.Id) {
							throw new TankkaartManagerException("Er bestaat reeds een tankkaart met dit kaartnummer. Er werden geen wijzigingen aangebracht.");
						}
					}
				}

				_opslag.UpdateTankkaart(tankkaart);
			} catch (Exception e) {
				throw new TankkaartManagerException("Tankkaart updaten mislukt.", e);
			}
		}

		// -- Delete
		public void VerwijderTankkaart(int id) {
			if (id <= 0) {
				throw new TankkaartManagerException("Het opgegeven id is ongeldig, vereiste: > 0.");
			}

			Tankkaart volledigeTankkaart = this.GeefTankkaartDetail((int)id);

			if (volledigeTankkaart is null) {
				throw new TankkaartManagerException("Deze tankkaart bestaat niet.");
			}

			try {
				_opslag.VerwijderTankkaart(id);
			} catch (Exception e) {
				throw new TankkaartManagerException($"Er is een onverwachte fout opgetreden: {e.GetType().Name}", e);
			}
		}


	}
}
