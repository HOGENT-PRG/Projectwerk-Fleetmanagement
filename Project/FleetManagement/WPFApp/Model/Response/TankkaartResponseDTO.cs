using System;
using System.Collections.Generic;
using WPFApp.Interfaces;
using WPFApp.Views;

namespace WPFApp.Model.Response {
    public class TankkaartResponseDTO : IResponseDTO {
        public int? Id { get; set; }
        public string Kaartnummer { get; set; }
        public DateTime Vervaldatum { get; set; }
        public string? Pincode { get; set; }
        public List<string> GeldigVoorBrandstoffen { get; set; }
        public BestuurderResponseDTO? Bestuurder { get; set; }
    }
}
