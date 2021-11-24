﻿using System;
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
    }
}