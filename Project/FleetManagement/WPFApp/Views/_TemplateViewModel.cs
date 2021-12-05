using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using WPFApp.Interfaces;
using WPFApp.Model.Hosts;
using WPFApp.Views.MVVM;
using System.ComponentModel;
using System.Reflection;
using WPFApp.Model.Response;
using WPFApp.Model.Request;
using System.Windows.Input;

namespace WPFApp.Views {
    // Dit is een template vanuit de noodzaak "Toevoegen", minder geschikt als basis voor overzichten dus
    // bij BestuurderToevoegen is deze niet sealed

    // Kijk even naar FilterDialogs, die properties erf je over en kan je hierin gebruiken.
    // Die properties, indien ze veranderen, worden gebruikt door FilterDialogs_PropertyChangedHandler
    internal sealed class _TemplateViewModel : FilterDialogs, IPaginaViewModel {
        private ICommuniceer _communicatieKanaal;
        public Action<object> StuurSnackbar { get; init; }

        public string Naam => "Bestuurder toevoegen";

        public ObservableCollection<TankkaartResponseDTO> Tankkaarten { get; set; } = new(); // kan gefilterd zijn
        public TankkaartResponseDTO HighlightedTankkaart { get; set; } = null;
        public TankkaartResponseDTO GeselecteerdeTankaart { get; set; } = null;

        //public DateTime Datum { get; set; } = DateTime.Now;

        public _TemplateViewModel(ICommuniceer communicatieKanaal, Action<object> stuurSnackbar) {
            _communicatieKanaal = communicatieKanaal;
            StuurSnackbar = stuurSnackbar;

            PropertyChanged += FilterDialogs_PropertyChangedHandler;
        }

        private void _startupRoutine() {
            try {
                _resetTankkaartFilters();
            } catch (Exception e) {
                StuurSnackbar(e);
            }
        }

        // Hier wordt er in de gaten gehouden of er een filter die we overerven van FilterDialogs wijzigt, indien dat het geval is gaan we zoeken met de filter
        private void FilterDialogs_PropertyChangedHandler(object sender, PropertyChangedEventArgs e) {
            switch (e.PropertyName) {
                case string s when s.StartsWith("TankkaartFilter"):
                    _zoekTankkaartMetFilters();
                    break;
                //case string s when s.StartsWith("VoertuigFilter"):
                //    _zoekVoertuigMetFilters();
                //    break;
                //case string s when s.StartsWith("BestuurderFilter"):
                //    _zoekBestuurderMetFilters();
                //default:
                //    break;
            }
        }

        // ! Wellicht gaan er nog enkele functies toegevoegd moeten worden aan de managers/ICommuniceer
        // Voorbeeld van een func signatuur uit voetbaltrui, welke vereist is om dit hier toe te passen
        // Wilt ook zeggen dat de FilterDialogs klasse de kolomnamen matcht met de databank
        // public List<Klant> ZoekKlantFilter(List<string> kolomnamen, List<object> zoektermen, bool likeWildcard = true)
        private void _zoekTankkaartMetFilters() {
            List<string> zoekfilters = new();
            List<object> zoektermen = new();
            List<PropertyInfo> p = this.GetType().GetProperties().Where(x => x.Name.StartsWith("TankkaartFilter")).ToList();
            foreach (PropertyInfo prop in p) {
                // Voor bestuurder zal voor de adresid,.. extra check moeten gedaan worden != null
                if (prop.GetValue(this).ToString().Length > 0 && !prop.Name.Contains("Staat")) {
                    zoekfilters.Add(prop.Name.Replace("TankkaartFilter", ""));
                    zoektermen.Add(prop.GetValue(this));
                }
            }
            if (zoekfilters.Count > 0) {
                // Om dit te kunnen doen
                // Tankkaarten = new ObservableCollection<Tankkaart>(_communicatieKanaal.ZoekTankkaartFilter(zoekfilters, zoektermen, likeWildcard: true));
            }
        }

        private void _resetTankkaartFilters() {
            Tankkaarten = new ObservableCollection<TankkaartResponseDTO>(_communicatieKanaal.GeefTankkaarten());
        }

        private void _selecteerHighlightedTankkaart() {
            try {
                if (HighlightedTankkaart is null) {
                    throw new Exception("Je hebt geen tankkaart geselecteerd.");
                }

                GeselecteerdeTankaart = HighlightedTankkaart;
            } catch (Exception e) {
                StuurSnackbar(e.Message);
            }
        }

        // Hier controle uitvoeren, is alles ingevuld, zijn de waarden ok, etc,
        // hierbij dat van voetbaltrui ter referentie maar zal er anders uit gaan zien.
        private bool _controleerVeldenVoldaanVoorToevoegen() {
            //bool voldaan = GeselecteerdeKlant is not null
            //               && Datum < DateTime.Now
            //               && GekozenProducten.Count > 0
            //               && Prijs > 0
            //               && !(GekozenProducten.Any(x => x.Value <= 0)); //aantal=0
            bool voldaan = false;

            if (!voldaan) {
                StuurSnackbar("Bestuurder voldoet niet aan vereisten.\nGelieve de velden in te vullen.");
            }

            return voldaan;
        }

        // Bij toevoegen aanmaken RequestDTO en waarden instellen
        private void _voegBestuurderToe() {
            if (_controleerVeldenVoldaanVoorToevoegen()) {
                try {
                    BestuurderRequestDTO bestuurder = new();
                    // Hier worden alle waarden ingesteld
                    // ...
                    // ...
                    // ...

                    int id = _communicatieKanaal.VoegBestuurderToe(bestuurder);
                    StuurSnackbar($"Succesvol toegevoegd met id {id}");

                    _resetTankkaartFilters();
                } catch (Exception e) {
                    StuurSnackbar(e);
                }
            }
        }

        // Aangeroepen in codebehind door UserControl Loaded event (dus niet bij initialisatie),
        // pas bij daadwerkelijk renderen van de usercontrol
        // Wordt ook aangeroepen bij aanmaak van een nieuwe viewmodel door
        // ApplicatieOverzichtViewModel.MaakNieuwViewModel dmv reflectie
        public ICommand StartupRoutine {
            get {
                return new RelayCommand(
                    p => _startupRoutine(),
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

        public ICommand ResetTankkaartFilters {
            get {
                return new RelayCommand(
                    p => _resetTankkaartFilters(),
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
