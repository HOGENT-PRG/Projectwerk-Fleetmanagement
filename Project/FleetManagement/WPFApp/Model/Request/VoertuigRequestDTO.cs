using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFApp.Interfaces;

namespace WPFApp.Model.Request {
    public class VoertuigRequestDTO : IRequestDTO {
        public int? Id { get; set; }
        public string? Merk { get; set; }
        public string? Model { get; set; }
        public string? Nummerplaat { get; set; }
        public string? Brandstof { get; set; }
        public string? Voertuigsoort { get; set; }
        public string? Kleur { get; set; }
        public int? AantalDeuren { get; set; }
        public BestuurderRequestDTO? Bestuurder { get; set; }
        public string? Chassisnummer { get; set; }
        public VoertuigRequestDTO(int? id,string? merk,string? model,string nummerplaat,string? brandstof,string? voertuigsoort,string? kleur, int? aantalDeuren, BestuurderRequestDTO? bestuurder,string? chasisnummer)
        {
            this.Id = id;
            this.Merk = merk;
            this.Model = model;
            this.Nummerplaat = nummerplaat;
            this.Brandstof = brandstof;
            this.Voertuigsoort = voertuigsoort;
            this.Kleur = kleur;
            this.AantalDeuren = aantalDeuren;
            this.Bestuurder = bestuurder;
            this.Chassisnummer = chasisnummer;
        }
    }
}
