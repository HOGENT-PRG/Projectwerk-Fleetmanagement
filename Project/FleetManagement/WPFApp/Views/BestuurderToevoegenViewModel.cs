using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using WPFApp.Views;
using WPFApp.Model.Hosts;
using WPFApp.Views.MVVM;
using System.ComponentModel;
using System.Reflection;
using WPFApp.Model.Response;
using WPFApp.Model.Request;
using System.Windows.Input;
using WPFApp.Helpers;
using WPFApp.Model.Mappers;
using WPFApp.Interfaces;

namespace WPFApp.Views {
    // Niet sealed in dit geval, BestuurderWijzigen kan veel ontlenen.
    // Maakt gebruik van Fody.PropertyChanged
    internal class BestuurderToevoegenViewModel : FilterDialogs, IPaginaViewModel {
        public string Naam => "Bestuurder toevoegen";

        protected ICommuniceer _communicatieKanaal;
        public Action<object> StuurSnackbar { get; init; }

        public List<string> RijbewijsOpties { get; init; } = new() {
            "AM", "A", "B", "C", "D", "G"
        };

        public ObservableCollection<AdresResponseDTO> Adressen { get; set; } = new(); 
        public ObservableCollection<TankkaartResponseDTO> Tankkaarten { get; set; } = new();
        public ObservableCollection<VoertuigResponseDTO> Voertuigen { get; set; } = new();

        public AdresResponseDTO HighlightedAdres { get; set; } = null;
		public TankkaartResponseDTO HighlightedTankkaart { get; set; } = null;
        public VoertuigResponseDTO HighlightedVoertuig { get; set; } = null;

        public string Achternaam { get; set; } = "";
        public string Voornaam { get; set; } = "";
        public DateTime GeboorteDatum { get; set; } = DateTime.Now;
        public string RijksRegisterNummer { get; set; } = "";
        public string RijbewijsSoort { get; set; } = ""; //enum
        public AdresRequestDTO GeselecteerdAdres { get; set; } = null;
        public TankkaartRequestDTO GeselecteerdeTankkaart { get; set; } = null;
        public VoertuigRequestDTO GeselecteerdeVoertuig { get; set; } = null;

        public BestuurderToevoegenViewModel(ICommuniceer communicatieKanaal, Action<object> stuurSnackbar) {
            _communicatieKanaal = communicatieKanaal;
            StuurSnackbar = stuurSnackbar;

            PropertyChanged += FilterDialogs_PropertyChangedHandler;
			PropertyChanged += Self_PropertyChanged;
        }

		protected void _startupRoutine() {
            try {
                _resetAdresFilters();
                _resetTankkaartFilters();
                _resetVoertuigFilters();
            } catch (Exception e) {
                StuurSnackbar(e);
            }
        }

        protected void Self_PropertyChanged(object sender, PropertyChangedEventArgs e) {
			switch (e.PropertyName) {
                case "RijksRegisterNummer":
                    if (RijksRegisterNummer.Length > 0) {
                        if (!_communicatieKanaal.ValideerRRN(RijksRegisterNummer)) {
                            StuurSnackbar("Het ingevoerde Rijksregisternummer is ongeldig.");
                        }
                    }
                    break;
                default:
                    break;
			}
        }

        // Hier wordt er in de gaten gehouden of er een filter property die we overerven van
        // FilterDialogs wijzigt, indien dat het geval is gaan we zoeken met de filter
        protected void FilterDialogs_PropertyChangedHandler(object sender, PropertyChangedEventArgs e) {
            switch (e.PropertyName) {
                case string s when s.StartsWith("AdresFilter"):
                    _zoekAdresMetFilters();
                    break;
                case string s when s.StartsWith("TankkaartFilter"):
                    _zoekTankkaartMetFilters();
                    break;
				case string s when s.StartsWith("VoertuigFilter"):
					_zoekVoertuigMetFilters();
					break;
				default:
					break;
			}
        }

        #region Zoek met filters, reset filters, selecteer highlighted

        protected void _zoekAdresMetFilters() {
            List<string> zoekfilters = new();
            List<object> zoektermen = new();
            List<PropertyInfo> p = this.GetType().GetProperties().Where(x => x.Name.StartsWith("AdresFilter")).ToList();
            foreach (PropertyInfo prop in p) {

                /*TODO: opnemen of weg, liefst geen Staat opnemen daarin, werd gebruikt bij Voetbaltrui maar zorgt voor verwarring indien wijzigdialog niet apart is:
                                                              && !prop.Name.Contains("Staat")*/
                if (prop.GetValue(this).ToString().Length > 0) {
                    zoekfilters.Add(prop.Name.Replace("AdresFilter", ""));
                    zoektermen.Add(prop.GetValue(this));
                }
            }
            if (zoekfilters.Count > 0) {
                Adressen = new ObservableCollection<AdresResponseDTO>(_communicatieKanaal.ZoekAdressen(zoekfilters, zoektermen, likeWildcard: true));
            }
        }

        protected void _resetAdresFilters() {
            Adressen = new ObservableCollection<AdresResponseDTO>(_communicatieKanaal.GeefAdressen());
        }

        protected void _selecteerHighlightedAdres() {
            try {
                if (HighlightedAdres is null) {
                    throw new Exception("Je hebt geen adres geselecteerd.");
                }

                GeselecteerdAdres = DTONaarDTO.ResponseNaarRequest<AdresRequestDTO>(HighlightedAdres);
            } catch (Exception e) {
                StuurSnackbar(e.Message);
            }
        }


        protected void _zoekTankkaartMetFilters() {
            List<string> zoekfilters = new();
            List<object> zoektermen = new();
            List<PropertyInfo> p = this.GetType().GetProperties().Where(x => x.Name.StartsWith("TankkaartFilter")).ToList();
            foreach (PropertyInfo prop in p) {
                if (prop.GetValue(this).ToString().Length > 0) {
                    zoekfilters.Add(prop.Name.Replace("TankkaartFilter", ""));
                    zoektermen.Add(prop.GetValue(this));
                }
            }
            if (zoekfilters.Count > 0) {
				Tankkaarten = new ObservableCollection<TankkaartResponseDTO>(_communicatieKanaal.ZoekTankkaarten(zoekfilters, zoektermen, likeWildcard: true));
            }
        }

        protected void _resetTankkaartFilters() {
            Tankkaarten = new ObservableCollection<TankkaartResponseDTO>(_communicatieKanaal.GeefTankkaarten());
        }

        protected void _selecteerHighlightedTankkaart() {
            try {
                if (HighlightedTankkaart is null) {
                    throw new Exception("Je hebt geen tankkaart geselecteerd.");
                }

                GeselecteerdeTankkaart = DTONaarDTO.ResponseNaarRequest<TankkaartRequestDTO>(HighlightedTankkaart);
            } catch (Exception e) {
                StuurSnackbar(e.Message);
            }
        }


        protected void _zoekVoertuigMetFilters() {
            List<string> zoekfilters = new();
            List<object> zoektermen = new();
            List<PropertyInfo> p = this.GetType().GetProperties().Where(x => x.Name.StartsWith("VoertuigFilter")).ToList();
            foreach (PropertyInfo prop in p) {
                if (prop.GetValue(this).ToString().Length > 0) {
                    zoekfilters.Add(prop.Name.Replace("VoertuigFilter", ""));
                    zoektermen.Add(prop.GetValue(this));
                }
            }
            if (zoekfilters.Count > 0) {
                Voertuigen = new ObservableCollection<VoertuigResponseDTO>(_communicatieKanaal.ZoekVoertuigen(zoekfilters, zoektermen, likeWildcard: true));
            }
        }

        protected void _resetVoertuigFilters() {
            Voertuigen = new ObservableCollection<VoertuigResponseDTO>(_communicatieKanaal.GeefVoertuigen());
        }

        protected void _selecteerHighlightedVoertuig() {
            try {
                if (HighlightedVoertuig is null) {
                    throw new Exception("Je hebt geen voertuig geselecteerd.");
                }

                GeselecteerdeVoertuig = DTONaarDTO.ResponseNaarRequest<VoertuigRequestDTO>(HighlightedVoertuig);
            } catch (Exception e) {
                StuurSnackbar(e.Message);
            }
        }

        #endregion

        private bool _controleerVeldenVoldaanVoorToevoegen() {
            // Adres, Tankkaart, Voertuig mag null zijn
            bool voldaan = !(string.IsNullOrEmpty(Achternaam) 
                             || Achternaam.Length < 2 
                             || Achternaam.Length > 70 
                             || Achternaam.Any(c => Char.IsDigit(c)))
                           && !(string.IsNullOrEmpty(Voornaam) 
                                || Voornaam.Length < 2 
                                || Voornaam.Length > 70 
                                || Voornaam.Any(c => Char.IsDigit(c)))
                           && (DateTime.Compare(DateTime.Now.AddYears(-120), GeboorteDatum) < 0)
                                && (DateTime.Compare(DateTime.Now, GeboorteDatum) > 0)
                           && RijksRegisterNummer.Length > 0 // validatie bij Self_ prop event handler
                           && RijbewijsSoort.Length > 0; // validatie dmv dropdown

            if (!voldaan) {
                StuurSnackbar("Bestuurder voldoet niet aan de vereisten.\nGelieve de velden in te vullen.");
            }

            return voldaan;
        }

        // Bij toevoegen aanmaken RequestDTO en waarden instellen
        private void _voegBestuurderToe() {
            if (_controleerVeldenVoldaanVoorToevoegen()) {
                try {
                    BestuurderRequestDTO bestuurder = new(null, Achternaam, Voornaam, RijksRegisterNummer, RijbewijsSoort, GeboorteDatum, GeselecteerdAdres, GeselecteerdeVoertuig, GeselecteerdeTankkaart);

                    int id = _communicatieKanaal.VoegBestuurderToe(bestuurder);
                    StuurSnackbar($"Succesvol toegevoegd met id {id}");

                    _resetAdresFilters();
                    _resetTankkaartFilters();
                    _resetVoertuigFilters();

                    Achternaam = "";
                    Voornaam = "";
                    GeboorteDatum = DateTime.Now;
                    RijksRegisterNummer = "";
                    RijbewijsSoort = "";

                    GeselecteerdAdres = null;
                    GeselecteerdeTankkaart = null;
                    GeselecteerdeVoertuig = null;
                } catch (Exception e) {
                    StuurSnackbar(e);
                }
            }
        }

        // Aangeroepen in codebehind door UserControl Loaded event (dus niet bij initialisatie),
        // pas bij daadwerkelijk renderen van de usercontrol
        // Wordt ook aangeroepen bij aanmaak van een nieuw viewmodel door
        // ApplicatieOverzichtViewModel.MaakNieuwViewModel dmv reflectie
        public ICommand StartupRoutine {
            get {
                return new RelayCommand(
                    p => _startupRoutine(),
                    p => p is not null
                );

            }
        }

        public ICommand SelecteerHighlightedAdres {
            get {
                return new RelayCommand(
                    p => _selecteerHighlightedAdres(),
                    p => p is not null
                );
            }
        }

        public ICommand SelecteerHighlightedTankkaart {
            get {
                return new RelayCommand(
                    p => _selecteerHighlightedTankkaart(),
                    p => p is not null
                );
            }
        }

        public ICommand SelecteerHighlightedVoertuig {
            get {
                return new RelayCommand(
                    p => _selecteerHighlightedVoertuig(),
                    p => p is not null
                );
            }
        }

        public ICommand ResetAdresFilters {
            get {
                return new RelayCommand(
                    p => _resetAdresFilters(),
                    p => p is not null
                );
            }
        }

        public ICommand ResetTankkaartFilters {
            get {
                return new RelayCommand(
                    p => _resetTankkaartFilters(),
                    p => p is not null
                );
            }
        }

        public ICommand ResetVoertuigFilters {
            get {
                return new RelayCommand(
                    p => _resetVoertuigFilters(),
                    p => p is not null
                );
            }
        }

        public ICommand ResetGeselecteerdAdres {
            get {
                return new RelayCommand(
                    p => { GeselecteerdAdres = null; },
                    p => p is not null
                );
            }
        }

        public ICommand ResetGeselecteerdeTankkaart {
            get {
                return new RelayCommand(
                    p => { GeselecteerdeTankkaart = null; },
                    p => p is not null
                );
            }
        }

        public ICommand ResetGeselecteerdeVoertuig {
            get {
                return new RelayCommand(
                    p => { GeselecteerdeVoertuig = null; },
                    p => p is not null
                );
            }
        }

        public ICommand VoegBestuurderToe {
            get {
                return new RelayCommand(
                    p => _voegBestuurderToe(),
                    p => p is not null
                );
            }
        }


    }
}

