using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFApp.Interfaces;
using WPFApp.Model.Request;
using WPFApp.Model.Response;

// Pas ontwikkelen indien api implementatie vereist is (na al de rest dus)

namespace WPFApp.Model.Communiceerders {
    internal class ApiCommuniceerder : ICommuniceer {
        private readonly string API_BASIS_PAD;

        public ApiCommuniceerder(string api_basispad) {
            this.API_BASIS_PAD = api_basispad;
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
