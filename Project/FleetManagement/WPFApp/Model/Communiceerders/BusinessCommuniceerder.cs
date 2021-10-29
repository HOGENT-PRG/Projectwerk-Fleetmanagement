using BusinessLaag;
using BusinessLaag.Model;
using DataLaag;
using DataLaag.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFApp.Exceptions;
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

        #region Private methodes

        // TODO: extra behandeling van velden waar geen directe mapping voor is (bv BestuurderNaam)
        // deze zijn extra's om de weergave/zoekfunctie beter te maken, aangezien deze geen encapsulated
        // objecten kunnen weergeven, maar een identificerende property er van is mogelijk en 
        // aangemaakt in de response dto's.
        // Manueel te mappen, zullen door bronparser waarschijnlijk (hopelijk) terugkeren als null
        // casten naar type en instellen alvorens te returnen

        // eventueel over laten aan de viewmodel, maar dat is nogal cru en zou hier bij de omzetting best op zijn
        // plaats staan

        private AdresResponseDTO _conveerAdresNaarDTO(Adres a) {
            try {
                AdresResponseDTO geconvAdres = BronParser.Parse<AdresResponseDTO>(a);
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
                        b.Voertuig.zetBestuurder(null);
                        geconvVoertuig = _converteerVoertuigNaarDTO(b.Voertuig, false);
                    }
                    if (b.Tankkaart != null) {
                        b.Tankkaart.zetBestuurder(null);
                        geconvTankkaart = _converteerTankkaartNaarDTO(b.Tankkaart, false);
                    }
                }

                b.zetAdres(null);
                b.zetVoertuig(null);
                b.zetTankkaart(null);

                BestuurderResponseDTO geconvBestuurder = BronParser.Parse<BestuurderResponseDTO>(b);
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

                t.zetBestuurder(null);

                TankkaartResponseDTO geconvTankkaart = BronParser.Parse<TankkaartResponseDTO>(t);
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

                v.zetBestuurder(null);

                VoertuigResponseDTO geconvVoertuig = BronParser.Parse<VoertuigResponseDTO>(v);
                geconvVoertuig.Bestuurder = geconvBestuurder;

                return geconvVoertuig;
            } catch (Exception e) { throw new BusinessCommuniceerderException(e.Message, e); }
        }

        #endregion
        /*------------------------------->> Einde private methodes <<-------------------------------*/

        public BestuurderResponseDTO geefBestuurderDetail(int bestuurder_id) {
            var resultaat = _fleetManager.BestuurderManager.geefBestuurderDetail(bestuurder_id);
            return _converteerBestuurderNaarDTO(resultaat, true);
        }

        public List<string> geefBestuurderProperties() {
            return _fleetManager.BestuurderManager.geefBestuurderProperties().ToList();
        }

        public List<BestuurderResponseDTO> geefBestuurders(bool inclusiefRelaties=true) {
            var resultaten = _fleetManager.BestuurderManager.geefBestuurders();
            List<BestuurderResponseDTO> geconverteerdeResultaten = new();

            foreach(Bestuurder b in resultaten) {
                geconverteerdeResultaten.Add(_converteerBestuurderNaarDTO(b, inclusiefRelaties));
            }

            return geconverteerdeResultaten;
        }

        public TankkaartResponseDTO geefTankkaartDetail(int tankkaartId) {
            throw new NotImplementedException();
        }

        public List<TankkaartResponseDTO> geefTankkaarten() {
            throw new NotImplementedException();
        }

        public List<string> geefTankkaartProperties() {
            throw new NotImplementedException();
        }

        public VoertuigResponseDTO geefVoertuigDetail(int voertuigId) {
            throw new NotImplementedException();
        }

        public List<VoertuigResponseDTO> geefVoertuigen() {
            throw new NotImplementedException();
        }

        public List<string> geefVoertuigProperties() {
            throw new NotImplementedException();
        }

        public bool updateBestuurder(BestuurderRequestDTO bestuurder) {
            throw new NotImplementedException();
        }

        public bool updateTankkaart(TankkaartRequestDTO tankkaart) {
            throw new NotImplementedException();
        }

        public bool updateVoertuig(VoertuigRequestDTO voertuig) {
            throw new NotImplementedException();
        }

        public bool verwijderBestuurder(int bestuurderId) {
            throw new NotImplementedException();
        }

        public bool verwijderTankkaart(int tankkaartId) {
            throw new NotImplementedException();
        }

        public bool verwijderVoertuig(int voertuigId) {
            throw new NotImplementedException();
        }

        public bool voegBestuurderToe(BestuurderRequestDTO bestuurder) {
            throw new NotImplementedException();
        }

        public bool voegTankkaartToe(TankkaartRequestDTO tankkaart) {
            throw new NotImplementedException();
        }

        public bool voegVoertuigToe(VoertuigRequestDTO voertuig) {
            throw new NotImplementedException();
        }

        public List<BestuurderResponseDTO> zoekBestuurders() {
            throw new NotImplementedException();
        }

        public List<TankkaartResponseDTO> zoekTankkaarten() {
            throw new NotImplementedException();
        }

        public List<VoertuigResponseDTO> zoekVoertuig() {
            throw new NotImplementedException();
        }

        public DatabankStatusResponseDTO geefDatabankStatus() {
            throw new NotImplementedException();
        }

        public List<AdresResponseDTO> geefAdressen() {
            throw new NotImplementedException();
        }
    }
}
