using System;
using System.Collections.Generic;
using WPFApp.Interfaces;
using WPFApp.Views;

namespace WPFApp.Model.Request {
    public class TankkaartRequestDTO : IRequestDTO {
        public int? Id { get; set; }
        public string? Kaartnummer { get; set; }
        public DateTime? Vervaldatum { get; set; }
        public string? Pincode { get; set; }
        public bool IsGeblokkeerd { get; set; }
        public List<string>? GeldigVoorBrandstoffen { get; set; }
        public BestuurderRequestDTO? Bestuurder { get; set; }

        public TankkaartRequestDTO() { }

        public TankkaartRequestDTO(int? id, string? kaartnummer, DateTime? vervaldatum, string? pincode, List<string> geldigvoorbrandstoffen, BestuurderRequestDTO? bestuurder, bool isGeblokkeerd) {
            Id = id;
            Kaartnummer = kaartnummer;
            Vervaldatum = vervaldatum;
            Pincode = pincode;
            IsGeblokkeerd = isGeblokkeerd;
            GeldigVoorBrandstoffen = geldigvoorbrandstoffen;
            Bestuurder = bestuurder;
		}
    }
}
