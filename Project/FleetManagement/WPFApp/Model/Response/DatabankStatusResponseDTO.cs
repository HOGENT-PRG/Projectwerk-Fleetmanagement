using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFApp.Model.Response {
    public class DatabankStatusResponseDTO {
        public bool? ConnectieSuccesvol { get;  set; }
        public bool? DatabaseBestaat { get; set; }
        public bool? AlleTabellenBestaan { get; set; }
        public int? AantalTabellen { get; set; }
        public bool? SequentieDoorlopen { get; set; }
    }
}
