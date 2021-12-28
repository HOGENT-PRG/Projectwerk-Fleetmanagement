using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WPFApp.Interfaces;
using WPFApp.Model.Response;
using WPFApp.Model;
using WPFApp.Interfaces.MVVM;
using System.Windows.Input;
using WPFApp.Helpers;
using System.ComponentModel;
using System.Linq;
using PropertyChanged;

// Maakt gebruik van PropertyChanged.Fody
namespace WPFApp.Interfaces {
        internal sealed class BestuurderOverzichtViewModel : Presenteerder, IPaginaViewModel {

        public string Naam => "Bestuurders";
        private readonly ICommuniceer CommunicatieKanaal;
        private readonly Zoekmachine Zoekmachine = new();
        public Action<object> StuurSnackbar { get; init; }

        private List<string> BlacklistZoekfilters { get; init; } = new() {
            "Chars", "Length", "Bestuurder", "GeldigVoorBrandstoffen"
        };

        public ObservableCollection<string> BestuurderZoekfilters { get; private set; }

        public ObservableCollection<BestuurderResponseDTO> Bestuurders { get; set; } = new();

        public BestuurderResponseDTO HighlightedBestuurder { get; set; } = null;

        public string GeselecteerdeZoekfilter { get; set; }
        public string ZoekveldRegular { get; set; }
        public DateTime ZoekveldDate { get; set; } = DateTime.Now;


        public BestuurderOverzichtViewModel(ICommuniceer comm, Action<object> stuurSnackbar) {
            CommunicatieKanaal = comm;
            StuurSnackbar = stuurSnackbar;

            _initialiseerZoekfilters();

			PropertyChanged += ViewModel_PropertyChanged;
        }

        // Indien je wilt abonneren op property changes
		private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e) {
			switch (e.PropertyName) {
                default:
                    break;
			}
		}

		// Bij het daadwerkelijk renderen gaan we data opvragen en weergeven (dus niet bij initialisatie van de View+VM, aangezien dat op de achtergrond bij opstart voor alle VM's tegelijk plaatsvindt)
		private void _startupRoutine() {
            try {
                _resetZoekFilter();
			} catch(Exception e) {
                StuurSnackbar(e);
			}
		}

        private void _initialiseerZoekfilters() {
            BestuurderZoekfilters = new(Zoekmachine.GeefZoekfilterVelden(typeof(BestuurderResponseDTO), BlacklistZoekfilters));
        }

        private void _resetZoekFilter() {
            List<BestuurderResponseDTO> res = CommunicatieKanaal.GeefBestuurders();
            Bestuurders = new(res);
		}

        private void _zoekMetFilter() {
            object zoekterm;
            string zoekfilter = GeselecteerdeZoekfilter;

            Func<object, object, bool> vergelijker = null;
            List<Func<List<BestuurderResponseDTO>>> dataCollectieActieBestuurders = new() {
                new Func<List<BestuurderResponseDTO>>(CommunicatieKanaal.GeefBestuurders)
            };

			switch (GeselecteerdeZoekfilter) {
                case string s when s.Contains("GeboorteDatum") || s.Contains("Vervaldatum"):
                    zoekterm = ZoekveldDate;
                    vergelijker = this.DatumVergelijker;
                    break;
                default:
                    zoekterm = ZoekveldRegular;
                    break;
			}


            Bestuurders = new ObservableCollection<BestuurderResponseDTO>(
                Zoekmachine.ZoekMetFilter<BestuurderResponseDTO>(dataCollectieActieBestuurders, zoekfilter, zoekterm, vergelijker).ToList()
            );
            

            ZoekveldDate = DateTime.Now;
            ZoekveldRegular = "";
        }

        // Vergelijkt datums - zonder tijd 
        // Geen datetime.parse, dat vertraagt de boel enorm (ook met parseexact)
        // Tevens wordt hier vergelijkt op basis van datum, de tijd die eventueel aanwezig is wordt niet in acht genomen, WPF voegt namelijk by default tijd toe wat niet wenselijk is
        public bool DatumVergelijker(object r1, object r2) {
            try {
                if (r1.GetType() == r2.GetType() && r1.GetType() == typeof(DateTime)) {
                    DateTime r1_conv = (DateTime)r1;
                    DateTime r2_conv = (DateTime)r2;

                    return r1_conv.Date == r2_conv.Date;
                } else {
                    return false;
                }
            } catch (Exception e) {
                Exception exc = new Exception("Datumvergelijker: Kon datums niet vergelijken.\n" + e.Message);
                StuurSnackbar(exc);
                return false;
            }
        }

        private void _verwijderHighlightedBestuurder() {
            if(HighlightedBestuurder?.Id is not null) {
                CommunicatieKanaal.VerwijderBestuurder((int)HighlightedBestuurder.Id);
                _resetZoekFilter();
                StuurSnackbar("Bestuurder succesvol verwijderd.");
			} else {
                StuurSnackbar("Bestuurder kon niet bepaald worden. Probeer deze eerst te selecteren alvorens te verwijderen.");
            }
		}

        public ICommand ZoekMetFilter {
            get {
                return new RelayCommand(
                    p => _zoekMetFilter(),
                    p => p is not null
                );
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

        public ICommand VerwijderHighlightedBestuurder {
            get {
                return new RelayCommand(
                    p => _verwijderHighlightedBestuurder(),
                    p => p is not null
                );
            }
        }

        public ICommand ResetZoekfilter {
            get {
                return new RelayCommand(
                    p => _resetZoekFilter(),
                    p => p is not null
                );
            }
        }

    }
}
