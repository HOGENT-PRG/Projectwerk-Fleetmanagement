using BusinessLaag;
using BusinessLaag.Interfaces;
using BusinessLaag.Model;
using DataLaag;
using DataLaag.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFApp.Exceptions;
using WPFApp.Helpers;
using WPFApp.Interfaces;
using WPFApp.Model.Request;
using WPFApp.Model.Response;

// De Businesscommuniceerder heeft als enigste klasse dependency op de business laag.
// In het geval dat er een API gebruikt wordt zal deze de verantwoordelijkheid voor het
// beheren van de dependency en het aanmaken van de FleetManager op zich moeten nemen.

namespace WPFApp.Model.Communiceerders {
    internal class BusinessCommuniceerder : ICommuniceer {
        private FleetManager _fleetManager;

        public BusinessCommuniceerder() {
            _fleetManager = new(new VoertuigOpslag(), new BestuurderOpslag(), new TankkaartOpslag(), new DatabankConfigureerder(null));
        }

        #region conversies
        private AdresResponseDTO _conveerAdresNaarDTO(Adres a) {
            try {
                AdresResponseDTO geconvAdres = BronParser.ParseCast<AdresResponseDTO>(a);
                return geconvAdres;
            } catch (Exception e) { throw new BusinessCommuniceerderException(e.Message, e); }
        }

        private BestuurderResponseDTO _converteerBestuurderNaarDTO(Bestuurder b, bool inclusiefRelaties) {
            try {
                AdresResponseDTO geconvAdres = null;
                VoertuigResponseDTO geconvVoertuig = null;
                TankkaartResponseDTO geconvTankkaart = null;

                if (inclusiefRelaties) {
                    if (b.Adres != null) {
                        geconvAdres = _conveerAdresNaarDTO(b.Adres);
                    }
                    if (b.Voertuig != null) {
                        b.Voertuig.ZetBestuurder(null);
                        geconvVoertuig = _converteerVoertuigNaarDTO(b.Voertuig, false);
                    }
                    if (b.Tankkaart != null) {
                        b.Tankkaart.ZetBestuurder(null);
                        geconvTankkaart = _converteerTankkaartNaarDTO(b.Tankkaart, false);
                    }
                }

                b.ZetAdres(null);
                b.ZetVoertuig(null);
                b.ZetTankkaart(null);

                BestuurderResponseDTO geconvBestuurder = BronParser.ParseCast<BestuurderResponseDTO>(b);
                geconvBestuurder.Adres = geconvAdres;
                geconvBestuurder.Voertuig = geconvVoertuig;
                geconvBestuurder.Tankkaart = geconvTankkaart;

                return geconvBestuurder;
            } catch (Exception e) { throw new BusinessCommuniceerderException(e.Message, e); }
        }

        private TankkaartResponseDTO _converteerTankkaartNaarDTO(Tankkaart t, bool inclusiefRelaties) {
                
            try {
                BestuurderResponseDTO geconvBestuurder = null;
                if (inclusiefRelaties) {
                    if(t.Bestuurder != null) {
                        geconvBestuurder = _converteerBestuurderNaarDTO(t.Bestuurder, false);
                    } 
                }

                t.ZetBestuurder(null);

                TankkaartResponseDTO geconvTankkaart = BronParser.ParseCast<TankkaartResponseDTO>(t);
                geconvTankkaart.Bestuurder = geconvBestuurder;

                return geconvTankkaart;
            } catch (Exception e) { throw new BusinessCommuniceerderException(e.Message, e); }
        }

        private VoertuigResponseDTO _converteerVoertuigNaarDTO(Voertuig v, bool inclusiefRelaties) {
            try {
                BestuurderResponseDTO geconvBestuurder = null;
                if (inclusiefRelaties) {
                    if (v.Bestuurder != null) {
                        geconvBestuurder = _converteerBestuurderNaarDTO(v.Bestuurder, false);
                    }
                }

                v.ZetBestuurder(null);

                VoertuigResponseDTO geconvVoertuig = BronParser.ParseCast<VoertuigResponseDTO>(v);
                geconvVoertuig.Bestuurder = geconvBestuurder;

                return geconvVoertuig;
            } catch (Exception e) { throw new BusinessCommuniceerderException(e.Message, e); }
        }

		#endregion

		#region databank info
		public DatabankStatusResponseDTO GeefDatabankStatus() {
            IDatabankConfigureerder db = _fleetManager.DatabankConfigureerder;
            return new DatabankStatusResponseDTO(db.ConnectieSuccesvol, db.DatabaseBestaat, db.AlleTabellenBestaan, db.AantalTabellen, db.SequentieDoorlopen);
        }
		#endregion

		#region Adres
		public int VoegAdresToe(AdresRequestDTO adres) {
			throw new NotImplementedException();
		}

		public List<AdresResponseDTO> GeefAdressen(string kolom = null, object waarde = null) {
			throw new NotImplementedException();
		}

		public void UpdateAdres(AdresRequestDTO adres) {
			throw new NotImplementedException();
		}

		public void VerwijderAdres(int id) {
			throw new NotImplementedException();
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
                geconverteerdeResultaten.Add(_converteerBestuurderNaarDTO(b, true));
            }

            return geconverteerdeResultaten;
        }

		public BestuurderResponseDTO GeefBestuurderDetail(int id) {
            var resultaat = _fleetManager.BestuurderManager.GeefBestuurderDetail(id);
            return _converteerBestuurderNaarDTO(resultaat, true);
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
