using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFApp.Interfaces;

namespace WPFApp.Model.Response {
    public class AdresResponseDTO : IResponseDTO {
        public int? Id { get; set; }
        public string Straatnaam { get; set; }
        public string Huisnummer { get; set; }
        public string Postcode { get; set; }
        public string Plaatsnaam { get; set; }
        public string Provincie { get; set; }
        public string Land { get; set; }
    }
}
