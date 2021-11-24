using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
