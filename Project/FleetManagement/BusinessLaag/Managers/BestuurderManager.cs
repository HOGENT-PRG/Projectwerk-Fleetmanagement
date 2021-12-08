using System;
using System.Collections.Generic;
using BusinessLaag.Interfaces;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BusinessLaag.Model;
using BusinessLaag.Exceptions;
using Newtonsoft.Json;

namespace BusinessLaag.Managers
{
    public class BestuurderManager : IBestuurderManager {
        private static FleetManager _fleetManager;
        private IBestuurderOpslag _opslag;
        
        public BestuurderManager(FleetManager fleetmanager, IBestuurderOpslag repository) {
            _fleetManager = fleetmanager;
            _opslag = repository;
        }

        // Adres heeft geen manager en valt onder het bewind van bestuurder
        public int VoegAdresToe(Adres adres) {
            if(adres is null) {
                throw new BestuurderManagerException("Opgegeven adres is null");
			}

            if(adres.Id is not null && adres.Id > 0) {
                throw new BestuurderManagerException("Adres beschikt reeds over een id.");
			}

            try {
                return _opslag.VoegAdresToe(adres);
            } catch (Exception e) {
                throw new BestuurderManagerException("Adres kon niet toegevoegd worden.", e);
			}
		}

        public List<Adres> GeefAdressen(string? kolom = null, object? waarde = null) {
            try {
                return _opslag.GeefAdressen(kolom, waarde);
            } catch (Exception e) {
                throw new BestuurderManagerException("Fout bij opvragen van adressen.", e);
            }
        }

        public List<Adres> ZoekAdressen(List<string> kolomnamen, List<object> zoektermen, bool likeWildcard = false) {
            try {
                return _opslag.ZoekAdressen(kolomnamen, zoektermen, likeWildcard);
			} catch(Exception e) {
                throw new BestuurderManagerException("Fout bij zoeken van adressen.", e);
			}
		}

        public void UpdateAdres(Adres adres) {
            if(adres?.Id is not null && adres.Id > 0 && this.GeefAdressen("Id", adres.Id).Count < 1) {
                throw new BestuurderManagerException("Adres met opgegeven id kon niet gevonden worden.");
			}

            try {
                _opslag.UpdateAdres(adres);
            } catch (Exception e) {
                throw new BestuurderManagerException("Fout bij updaten van adres.", e);
            }
        }

        public void VerwijderAdres(int id) {
            if(id < 1) {
                throw new BestuurderManagerException("Opgegeven id moet minimum 1 zijn.");
			}

            if (id > 0 && this.GeefAdressen("Id", id).Count < 1) {
                throw new BestuurderManagerException("Adres met opgegeven id kon niet gevonden worden.");
            }

            try {
                _opslag.VerwijderAdres(id);
            } catch (Exception e) {
                throw new BestuurderManagerException("Fout bij verwijderen van adres.", e);
            }
        }

        // -- Create
        public int VoegBestuurderToe(Bestuurder bestuurder) {

            if(bestuurder is null) {
                throw new BestuurderManagerException("Meegegeven bestuurder is null.");
			}

            if(bestuurder.Id is not null || bestuurder.Id > 0) {
                throw new BestuurderManagerException("Id van bestuurder is reeds ingesteld.");
			}

            if(this.ZoekBestuurders("Rijksregisternummer", bestuurder.RijksRegisterNummer).Count > 0) {
                throw new BestuurderManagerException("Er bestaat reeds een bestuurder met dit RRN.");
			}

            // Adres behandeling in opslag, enigste relatie waarvoor lokaal iets gedaan wordt (toevoegen/updaten)
            // Dat doen we niet voor tankkaart en voertuig.
            // Adres velden kunnen, indien daar plaats voor is, voorkomen in WPF window Bestuurder Toevoegen
            // Tankkaart, voertuig worden slechts geselecteerd en elders aangemaakt

            if(bestuurder.Tankkaart is not null) {
                if(bestuurder.Tankkaart.Id is null) { 
                    throw new BestuurderManagerException("Tankkaart werd meegegeven maar deze heeft geen id."); 
                }
                if(_fleetManager.TankkaartManager.GeefTankkaartDetail((int)bestuurder.Tankkaart.Id) is null) {
                    throw new BestuurderManagerException("Tankkaart werd meegegeven maar kon niet gevonden worden.");
				}
			}

            if(bestuurder.Voertuig is not null) {
                if (bestuurder.Voertuig.Id is null) { 
                    throw new BestuurderManagerException("Voertuig werd meegegeven maar deze heeft geen id."); 
                }
                if(_fleetManager.VoertuigManager.GeefVoertuigDetail((int)bestuurder.Voertuig.Id) is null) {
                    throw new BestuurderManagerException("Voertuig werd meegegeven maar kon niet gevonden worden.");
				}
            }

            try {
                return _opslag.VoegBestuurderToe(bestuurder);
			} catch (Exception e) {
                throw new BestuurderManagerException("Bestuurder kon niet toegevoegd worden.", e);
			}
        }

        // -- Read

        public List<Bestuurder> GeefBestuurders(string? kolom=null, object? waarde=null) {
            try {
                return _opslag.GeefBestuurders(kolom, waarde);
            } catch (Exception e) {
                throw new BestuurderManagerException("Bestuurders konden niet opgevraagd worden.", e);
            }
        }

        public List<Bestuurder> ZoekBestuurders(List<string> kolomnamen, List<object> zoektermen, bool likeWildcard = false) {
            try {
                return _opslag.ZoekBestuurders(kolomnamen, zoektermen, likeWildcard);
            } catch (Exception e) {
                throw new BestuurderManagerException("Bestuurders konden niet gezocht worden.", e);
            }
        }

        public Bestuurder GeefBestuurderDetail(int id) {
            if(id <= 0) {
                throw new BestuurderManagerException("Het opgegeven id is < 1.");
			}

            try {
                return _opslag.GeefBestuurderDetail(id);
            } catch (Exception e) {
                throw new BestuurderManagerException("Bestuurder kon niet opgevraagd worden.", e);
            }
        }

        public Bestuurder GeefBestuurderZonderRelaties(int id) {
            if (id <= 0) {
                throw new BestuurderManagerException("Het opgegeven id is < 1.");
            }

            try {
                Bestuurder b = this.GeefBestuurderDetail(id);
                b.ZetAdres(null);
                b.ZetTankkaart(null);
                b.ZetVoertuig(null);
                return b;
            } catch (Exception e) {
                throw new BestuurderManagerException("Bestuurder kon niet opgevraagd worden.", e);
            }
        }

        public List<Bestuurder> ZoekBestuurders(string kolom, object waarde) {
            try {
                return this.GeefBestuurders(kolom, waarde);
            } catch (Exception e) {
                throw new BestuurderManagerException("Bestuurders konden niet gezocht worden.", e);
            }
        }

        // -- Update
        public void UpdateBestuurder(Bestuurder bestuurder) {
            if (bestuurder?.Id is null || bestuurder.Id <= 0) {
                throw new BestuurderManagerException("Meegegeven bestuurder is null of id is null / < 1.");
            }

            if(this.GeefBestuurderDetail((int)bestuurder.Id) is null) {
                throw new BestuurderManagerException("Er kan geen bestuurder gevonden worden om te updaten.");
			}

            try {
                _opslag.UpdateBestuurder(bestuurder);
            } catch (Exception e) {
                throw new BestuurderManagerException("Bestuurder kon niet geupdatet worden.", e);
            }
        }

        // -- Delete
        public void VerwijderBestuurder(int id) {
            if (id <= 0) {
                throw new BestuurderManagerException("Meegegeven id is < 1.");
            }

            if (this.GeefBestuurderDetail(id) is null) {
                throw new BestuurderManagerException("Er kan geen bestuurder gevonden worden om te verwijderen.");
            }

            try {
                _opslag.VerwijderBestuurder(id);
            } catch (Exception e) {
                throw new BestuurderManagerException("Bestuurder kon niet geupdatet worden.", e);
            }
        }


    }
}
