using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using WPFApp.Views.MVVM;

namespace WPFApp.Model.Hosts {

	// De :Presenteerder overerving zorgt dat ViewModels die hiervan
	// overerven nog steeds de overerving van Presenteerder behouden.

	// Komt overeen met db model, filter prefixen worden van de property namen af gedaan door
	// de betreffende viewmodel (bv AdresFilter eraf)

	// Enums zijn string waarde (zoals in db)
	internal class FilterDialogs : Presenteerder {
		public string AdresFilterStraatnaam { get; set; } = "";
		public string AdresFilterHuisnummer { get; set; } = "";
		public string AdresFilterPostcode { get; set; } = "";
		public string AdresFilterPlaatsnaam { get; set; } = "";
		public string AdresFilterProvincie { get; set; } = "";
		public string AdresFilterLand { get; set; } = "";

		// Zou apart in viewmodel filteren mbt tankkaartbrandstof (1:M)
		public string TankkaartFilterKaartnummer { get; set; } = "";
		public DateTime TankkaartFilterVervaldatum { get; set; } = DateTime.Now;
		public string TankkaartFilterPincode { get; set; } = "";

		public string VoertuigFilterMerk { get; set; } = "";
		public string VoertuigFilterModel { get; set; } = "";
		public string VoertuigFilterNummerplaat { get; set; } = "";
		public string VoertuigFilterChasisnummer { get; set; } = "";
		public string VoertuigFilterBrandstof { get; set; } = "";
		public string VoertuigFilterType { get; set; } = "";
		public string VoertuigFilterKleur { get; set; } = "";
		public string VoertuigFilterAantalDeuren { get; set; } = "";

		public string BestuurderFilterNaam { get; set; } = "";
		public string BestuurderFilterVoornaam { get; set; } = "";
		public DateTime BestuurderFilterGeboortedatum { get; set; } = DateTime.Now;
		public string BestuurderFilterRijksregisternummer { get; set; } = "";
		public string BestuurderFilterRijbewijssoort { get; set; } = "";
		public int? BestuurderFilterAdresId { get; set; } = null;
		public int? BestuurderFilterVoertuigId { get; set; } = null;
		public int? BestuurderFilterTankkaartId { get; set; } = null;


	}
}
