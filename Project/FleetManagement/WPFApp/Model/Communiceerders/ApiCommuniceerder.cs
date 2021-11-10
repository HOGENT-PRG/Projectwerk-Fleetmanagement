﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFApp.Exceptions;
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

        private string _voerAPIRequestUit(string jsonConstruct, string urlPad) {

            throw new NotImplementedException();
        }

        /*------------------------------->> Einde private methodes <<-------------------------------*/

        public BestuurderResponseDTO geefBestuurderDetail(int tankkaartId) {
            throw new NotImplementedException();
        }

        public List<BestuurderResponseDTO> geefBestuurders() {
            return geefBestuurders(true);
        }

        public List<BestuurderResponseDTO> geefBestuurders(bool inclusiefRelaties) {
            throw new NotImplementedException();
        }

        public TankkaartResponseDTO geefTankkaartDetail(int tankkaartId) {
            throw new NotImplementedException();
        }

        public List<TankkaartResponseDTO> geefTankkaarten() {
            throw new NotImplementedException();
        }

        public VoertuigResponseDTO geefVoertuigDetail(int voertuigId) {
            throw new NotImplementedException();
        }

        public List<VoertuigResponseDTO> geefVoertuigen() {
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
