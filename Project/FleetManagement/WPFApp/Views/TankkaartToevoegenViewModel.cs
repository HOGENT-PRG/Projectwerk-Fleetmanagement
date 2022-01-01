using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFApp.Views;
using WPFApp.Model.Mappers;
using WPFApp.Model.Request;
using WPFApp.Model.Response;
using WPFApp.Views.MVVM;
using PropertyChanged;
using WPFApp.Model.Hosts;
using WPFApp.Interfaces;

namespace WPFApp.Views {
    internal class TankkaartToevoegenViewModel : FilterDialogs, IPaginaViewModel {
        public string Naam => "Tankkaart toevoegen";
        protected ICommuniceer _communicatieKanaal;
        public Action<object> StuurSnackbar { get; init; }

        public List<string> TankkaartBrandstoffen { get; init; } = new() {
            "Diesel", "Benzine", "CNG", "Elektrisch"
        };

        public DateTime VervaltTenVroegste { get; init; } = DateTime.Now.AddDays(2);

        public ObservableCollection<BestuurderResponseDTO> Bestuurders { get; set; } = new();

        public string Kaartnummer { get; set; } = "";
        public DateTime Vervaldatum { get; set; } = DateTime.Now.AddDays(2);
        public string Pincode { get; set; } = "";
        public bool IsGeblokkeerd { get; set; } = false;
        public ObservableCollection<string> GekozenBrandstoffen { get; private set; } = new();
        public string GeselecteerdeBrandstof { get; set; } = "";

        public BestuurderRequestDTO GeselecteerdBestuurder { get; set; } = null;
        public BestuurderResponseDTO HighlightedBestuurder { get; set; } = null;

        public TankkaartToevoegenViewModel(ICommuniceer communicatieKanaal, Action<object> stuurSnackbar) {
            _communicatieKanaal = communicatieKanaal;
            StuurSnackbar = stuurSnackbar;

            PropertyChanged += FilterDialogs_PropertyChangedHandler;
        }

        protected void _startupRoutine() {
            try {
                _resetBestuurderFilters();
            } catch (Exception e) {
                StuurSnackbar(e);
            }
        }

        protected void FilterDialogs_PropertyChangedHandler(object sender, PropertyChangedEventArgs e) {
            switch (e.PropertyName) {
                case string s when s.StartsWith("BestuurderFilter"):
                    _zoekBestuurderMetFilters();
                    break;
                default:
                    break;
            }
        }

        protected void _zoekBestuurderMetFilters() {
            List<string> zoekfilters = new();
            List<object> zoektermen = new();
            List<PropertyInfo> p = this.GetType().GetProperties().Where(x => x.Name.StartsWith("BestuurderFilter")).ToList();
            foreach (PropertyInfo prop in p) {
                if (prop.GetValue(this).ToString().Length > 0) {
                    zoekfilters.Add(prop.Name.Replace("BestuurderFilter", ""));
                    zoektermen.Add(prop.GetValue(this));
                }
            }
            if (zoekfilters.Count > 0) {
                Bestuurders = new ObservableCollection<BestuurderResponseDTO>(_communicatieKanaal.ZoekBestuurders(zoekfilters, zoektermen, likeWildcard: true));
            }
        }
        
        protected void _resetBestuurderFilters() {
            Bestuurders = new ObservableCollection<BestuurderResponseDTO>(_communicatieKanaal.GeefBestuurders());
        }

        protected void _selecteerHighlightedBestuurder() {
            try {
                if (HighlightedBestuurder is null) {
                    throw new Exception("Je hebt geen bestuurder geselecteerd.");
                }

                GeselecteerdBestuurder = DTONaarDTO.ResponseNaarRequest<BestuurderRequestDTO>(HighlightedBestuurder);
            } catch (Exception e) {
                StuurSnackbar(e.Message);
            }
        }

        protected void _voegGeselecteerdeBrandstofToe() {
            if (GeselecteerdeBrandstof.Length > 0) {
                if (!GekozenBrandstoffen.Contains(GeselecteerdeBrandstof)) {
                    GekozenBrandstoffen.Add(GeselecteerdeBrandstof);
                } else {
                    StuurSnackbar("Deze brandstof maakt reeds deel uit van de gekozen brandstoffen.");
                }
            } else {
                StuurSnackbar("Gelieve eerst een brandstof te selecteren.");
            }
        }

        protected void _verwijderGeselecteerdeBrandstof() {
            if (GekozenBrandstoffen.Contains(GeselecteerdeBrandstof)) {
                GekozenBrandstoffen.Remove(GeselecteerdeBrandstof);
            } else {
                StuurSnackbar("Deze brandstof maakt geen deel uit van de gekozen brandstoffen.");
            }
        }

        private bool _controleerVeldenVoldaanVoorToevoegen() {

            bool voldaan = !(Kaartnummer.Length < 5 || Kaartnummer.Length > 60)
                            && !(Pincode.Length is not 4 || Pincode.ToCharArray().Any(c => !Char.IsDigit(c)))
                            && GekozenBrandstoffen.Distinct().Count() == GekozenBrandstoffen.Count;

            if (!voldaan) {
                StuurSnackbar("Tankkaart voldoet niet aan de vereisten.\nGelieve de velden in te vullen.");
            }

            return voldaan;
        }

        private void _voegTankkaartToe() {
            if (_controleerVeldenVoldaanVoorToevoegen()) {
                try {
                    TankkaartRequestDTO tankkaart = new(null, Kaartnummer, Vervaldatum, Pincode, GekozenBrandstoffen.ToList(), GeselecteerdBestuurder, IsGeblokkeerd);

                    int id = _communicatieKanaal.VoegTankkaartToe(tankkaart);
                    StuurSnackbar($"Succesvol nieuwe tankkaart toegevoegd met id {id}");

                    _resetBestuurderFilters();
                    Kaartnummer = "";
                    Vervaldatum = DateTime.Now.AddDays(2);
                    Pincode = "";
                    GekozenBrandstoffen = new();
                    GeselecteerdBestuurder = null;
                } catch (Exception e) {
                    StuurSnackbar(e);
                }
            }
        }

        

        public ICommand StartupRoutine {
            get {
                return new RelayCommand(
                    p => _startupRoutine(),
                    p => p is not null
                );

            }
        }

		public ICommand VoegGeselecteerdeBrandstofToe {
			get {
				return new RelayCommand(
					p => _voegGeselecteerdeBrandstofToe(),
					p => p is not null
				);
			}
		}

		public ICommand VerwijderGeselecteerdeBrandstof {
			get {
				return new RelayCommand(
					p => _verwijderGeselecteerdeBrandstof(),
					p => p is not null
				);
			}
		}

		public ICommand SelecteerHighlightedBestuurder {
            get {
                return new RelayCommand(
                    p => _selecteerHighlightedBestuurder(),
                    p => p is not null
                );
            }
        }
        public ICommand ResetBestuurderFilters {
            get {
                return new RelayCommand(
                    p => _resetBestuurderFilters(),
                    p => p is not null
                );
            }
        }
        public ICommand ResetGeselecteerdeBestuurder {
            get {
                return new RelayCommand(
                    p => { GeselecteerdBestuurder = null; },
                    p => p is not null
                );
            }
        }
        public ICommand VoegTankkaartToe {
            get {
                return new RelayCommand(
                    p => _voegTankkaartToe(),
                    p => p is not null
                );
            }
        }


    }
}
