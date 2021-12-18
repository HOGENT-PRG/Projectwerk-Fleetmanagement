using System;
using WPFApp.Interfaces;

namespace WPFApp.Model.Request {
    public class BestuurderRequestDTO : IRequestDTO {
        public int? Id { get; set; }
        public string? Naam { get; set; }
        public string? Voornaam { get; set; }
        public AdresRequestDTO? Adres { get; set; }
        public DateTime? GeboorteDatum { get; set; }
        public string? RijksRegisterNummer { get; set; }
        public string? RijbewijsSoort { get; set; }
        public VoertuigRequestDTO? Voertuig { get; set; }
        public TankkaartRequestDTO? Tankkaart { get; set; }

        public BestuurderRequestDTO() { }
        public BestuurderRequestDTO(int? id, string? naam, string? voornaam, string? rrn, string? rijbewijssoort, DateTime geboortedatum, AdresRequestDTO? adres, VoertuigRequestDTO? voertuig, TankkaartRequestDTO? tankkaart) {
            this.Id = id;
            this.Naam = naam;
            this.Voornaam = voornaam;
            this.RijksRegisterNummer = rrn;
            this.RijbewijsSoort = rijbewijssoort;
            this.GeboorteDatum = geboortedatum;
            this.Adres = adres;
            this.Voertuig = voertuig;
            this.Tankkaart = tankkaart;
		}
    }
}
