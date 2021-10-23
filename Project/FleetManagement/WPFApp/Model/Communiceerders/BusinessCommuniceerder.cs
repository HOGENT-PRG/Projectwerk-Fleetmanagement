using BusinessLaag;
using DataLaag;
using DataLaag.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public BestuurderResponseDTO geefBestuurderDetail(int tankkaartId) {
            throw new NotImplementedException();
        }

        public IEnumerable<string> geefBestuurderProperties() {
            throw new NotImplementedException();
        }

        public IEnumerable<BestuurderResponseDTO> geefBestuurders() {
            throw new NotImplementedException();
        }

        public TankkaartResponseDTO geefTankkaartDetail(int tankkaartId) {
            throw new NotImplementedException();
        }

        public IEnumerable<TankkaartResponseDTO> geefTankkaarten() {
            throw new NotImplementedException();
        }

        public IEnumerable<string> geefTankkaartProperties() {
            throw new NotImplementedException();
        }

        public VoertuigResponseDTO geefVoertuigDetail(int voertuigId) {
            throw new NotImplementedException();
        }

        public IEnumerable<VoertuigResponseDTO> geefVoertuigen() {
            throw new NotImplementedException();
        }

        public IEnumerable<string> geefVoertuigProperties() {
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

        public IEnumerable<BestuurderResponseDTO> zoekBestuurders() {
            throw new NotImplementedException();
        }

        public IEnumerable<TankkaartResponseDTO> zoekTankkaarten() {
            throw new NotImplementedException();
        }

        public IEnumerable<VoertuigResponseDTO> zoekVoertuig() {
            throw new NotImplementedException();
        }
    }
}
