using System;
using WPFApp.Interfaces;

namespace WPFApp.Model.Response {
    public class BestuurderResponseDTO : IResponseDTO {
        public int? Id { get; set; }
        public string Naam { get; set; }
        public string Voornaam { get; set; }
        public DateTime GeboorteDatum { get; set; }
        public string Rijksregisternummer { get; set; }
        public string Rijbewijssoort { get; set; }
        public AdresResponseDTO Adres { get; set; }
        public VoertuigResponseDTO Voertuig { get; set; }
        public TankkaartResponseDTO Tankkaart { get; set; }
    }
}
