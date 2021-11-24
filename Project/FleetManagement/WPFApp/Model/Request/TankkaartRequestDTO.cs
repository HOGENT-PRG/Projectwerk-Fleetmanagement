using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFApp.Interfaces;

namespace WPFApp.Model.Request {
    public class TankkaartRequestDTO : IRequestDTO {
        public int? Id { get; set; }
        public string? Kaartnummer { get; set; }
        public DateTime? Vervaldatum { get; set; }
        public string? Pincode { get; set; }
        public List<string>? GeldigVoorBrandstoffen { get; set; }
        public BestuurderRequestDTO? Bestuurder { get; set; }
    }
}
