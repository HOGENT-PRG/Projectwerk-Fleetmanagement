using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFApp.Interfaces {
	// Minimum vereisten van een request dto
	// Marker interface welke omgang vergemakkelijkt in DTOnaarDTO klasse
	internal interface IRequestDTO {
		public int? Id { get; set; }
	}
}
