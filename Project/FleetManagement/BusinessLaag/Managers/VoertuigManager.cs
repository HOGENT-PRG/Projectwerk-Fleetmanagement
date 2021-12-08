using System;
using System.Collections.Generic;
using BusinessLaag.Exceptions;
using BusinessLaag.Interfaces;
using BusinessLaag.Model;

namespace BusinessLaag.Managers {
	public class VoertuigManager : IVoertuigManager {
		private static FleetManager _fleetManager;
		private IVoertuigOpslag _opslag;

		public VoertuigManager(FleetManager fleetmanager, IVoertuigOpslag opslag) {
			_fleetManager = fleetmanager;
			_opslag = opslag;
		}

		// -- Create
		public int VoegVoertuigToe(Voertuig voertuig) {
			if (voertuig.Id is not null && voertuig.Id > 0) {
				throw new VoertuigManagerException("Voertuig beschikt reeds over een id en kan niet toegevoegd worden.");
			}

			if (ZoekVoertuigMetChassisnummer(voertuig.Chassisnummer) is not null) {
				throw new VoertuigManagerException("Chassisnummer is reeds toegewezen aan een voertuig.");
			}

			if (ZoekVoertuigMetNummerplaat(voertuig.Nummerplaat) is not null) {
				throw new VoertuigManagerException("Nummerplaat is reeds toegewezen aan een voertuig.");
			}

			if (voertuig.Bestuurder is not null) {
				if (voertuig.Bestuurder.Id is null) {
					throw new VoertuigManagerException("Toevoegen niet mogelijk, voertuig bevat een bestuurder maar deze bevat geen id.");
				}
			}

			try {
				return _opslag.VoegVoertuigToe(voertuig);
			} catch (Exception e) {
				throw new VoertuigManagerException("Er is een onverwachte fout opgetreden.", e);
			}
		}

		// -- Read
		public Voertuig GeefVoertuigZonderRelaties(int id) {
			Voertuig v = this.GeefVoertuigDetail(id);
			v.ZetBestuurder(null);
			return v;
		}

		public Voertuig GeefVoertuigDetail(int id) {
			try {
				return _opslag.GeefVoertuigDetail(id);
			} catch (Exception e) {
				throw new VoertuigManagerException("Er trad een onverwachte fout op.", e);
			}
		}

		public List<Voertuig> GeefVoertuigen() {
			try {
				return _opslag.GeefVoertuigen();
			} catch (Exception e) {
				throw new VoertuigManagerException("Er trad een onverwachte fout op.", e);
			}
		}

		public List<Voertuig> ZoekVoertuigen(List<string> kolomnamen, List<object> zoektermen, bool likeWildcard = false) {
			try {
				return _opslag.ZoekVoertuigen(kolomnamen, zoektermen, likeWildcard);
			} catch (Exception e) {
				throw new VoertuigManagerException("Fout tijdens het zoeken naar voertuigen.", e);
			}
		}

		private Voertuig _zoekVoertuigMetKolomnaam(string kolomnaam, string waarde) {
			try {
				return _opslag.ZoekVoertuig(kolomnaam, waarde);
			} catch (Exception e) {
				throw new VoertuigManagerException("Er trad een onverwachte fout op.", e);
			}
		}

		public Voertuig ZoekVoertuigMetChassisnummer(string chassisnummer) {
			return _zoekVoertuigMetKolomnaam("Chasisnummer", chassisnummer);
		}

		public Voertuig ZoekVoertuigMetNummerplaat(string nummerplaat) {
			return _zoekVoertuigMetKolomnaam("Nummerplaat", nummerplaat);
		}

		// Update
		public void UpdateVoertuig(Voertuig NieuwVoertuig) {
			if (NieuwVoertuig?.Id is null || NieuwVoertuig.Id <= 0) {
				throw new VoertuigManagerException("Opgegeven voertuig id is null of < 1.");
			}

			if (NieuwVoertuig.Bestuurder is not null) {
				if (NieuwVoertuig.Bestuurder.Id is null) {
					throw new VoertuigManagerException("Er is een bestuurder meegegeven maar deze bevat geen id.");
				}
				if (_fleetManager.BestuurderManager.GeefBestuurderDetail((int)NieuwVoertuig.Bestuurder.Id) is null) {
					throw new VoertuigManagerException($"De opgegeven bestuurder met id {NieuwVoertuig.Bestuurder.Id} bestaat niet.");
				}
			}

			Voertuig BestaandVoertuig = this.GeefVoertuigDetail((int)NieuwVoertuig.Id);

			if (BestaandVoertuig is null) {
				throw new VoertuigManagerException("Kan geen voertuig vinden voor opgegeven id.");
			}

			if (BestaandVoertuig.Chassisnummer != NieuwVoertuig.Chassisnummer) {
				Voertuig chassisCheckVoertuig = this.ZoekVoertuigMetChassisnummer(NieuwVoertuig.Chassisnummer);
				if (chassisCheckVoertuig is not null) {
					if (chassisCheckVoertuig.Id != NieuwVoertuig.Id) {
						throw new VoertuigManagerException("Er bestaat reeds een voertuig met dit chassisnummer.");
					}
				}
			}

			if (BestaandVoertuig.Nummerplaat != NieuwVoertuig.Nummerplaat) {
				Voertuig nummerplaatCheckVoertuig = this.ZoekVoertuigMetNummerplaat(NieuwVoertuig.Nummerplaat);
				if (nummerplaatCheckVoertuig is not null) {
					if (nummerplaatCheckVoertuig.Id != NieuwVoertuig.Id) {
						throw new VoertuigManagerException("Er bestaat reeds een voertuig met deze nummerplaat.");
					}
				}
			}

			try {
				_opslag.UpdateVoertuig(NieuwVoertuig);
			} catch (Exception e) {
				throw new VoertuigManagerException("Er deed zich een onverwachte fout voor.", e);
			}

		}

		// Delete
		public void VerwijderVoertuig(int id) {
			if (id <= 0) {
				throw new VoertuigManagerException("Het opgegeven voertuig id is ongeldig.");
			}

			if (this.GeefVoertuigDetail(id) is null) {
				throw new VoertuigManagerException("Dit voertuig bestaat niet.");
			}

			try {
				_opslag.VerwijderVoertuig(id);
			} catch (Exception e) {
				throw new VoertuigManagerException($"Er is een onverwachte fout opgetreden: {e.GetType().Name}", e);
			}

		}


	}
}
