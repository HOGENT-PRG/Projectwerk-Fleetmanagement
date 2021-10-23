using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFApp.Interfaces;

namespace WPFApp.Model.Response {
    public class VoertuigResponseDTO : IResponseDTO {
        public int? Id { get; private set; }
        public string? Merk { get; private set; }
        public string Model { get; private set; }
        public string Nummerplaat { get; private set; }
        public string? Brandstof { get; private set; }
        public string? Voertuigsoort { get; private set; }
        public string? Kleur { get; private set; }
        public int? AantalDeuren { get; private set; }
        public BestuurderResponseDTO? Bestuurder { get; private set; }
        public string Chassisnummer { get; private set; }
    }
}
