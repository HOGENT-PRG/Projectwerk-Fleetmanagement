using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFApp.Helpers;
using WPFApp.Views;
using WPFApp.Model.Response;
using WPFApp.Views.MVVM;
using PropertyChanged;
using WPFApp.Interfaces;

// Maakt gebruik van PropertyChanged.Fody, elke property is observable.
namespace WPFApp.Views {
        internal sealed class VoertuigOverzichtViewModel : Presenteerder, IPaginaViewModel {

        public string Naam => "Voertuigen";
        public Action<object> StuurSnackbar { get; private set; }

        private readonly ICommuniceer CommunicatieKanaal;
        private readonly Zoekmachine Zoekmachine = new();

        private List<string> BlacklistZoekfilters { get; init; } = new() {
            "Chars",
            "Length"
        };

        public ObservableCollection<string> VoertuigZoekfilters { get; private set; }

        public ObservableCollection<VoertuigResponseDTO> Voertuigen { get; set; } = new();

        public VoertuigResponseDTO HighlightedVoertuig { get; set; } = null;

        public string GeselecteerdeZoekfilter { get; set; }
        public string ZoekveldRegular { get; set; }

        public VoertuigOverzichtViewModel(ICommuniceer comm, Action<object> stuurSnackbar) {
            CommunicatieKanaal = comm;
            StuurSnackbar = stuurSnackbar;

            _initialiseerZoekfilters();
        }


        // Bij het daadwerkelijk renderen gaan we data opvragen en weergeven (dus niet bij initialisatie van de View+VM, aangezien dat op de achtergrond bij opstart voor alle VM's tegelijk plaatsvindt)
        private void _startupRoutine() {
            try {
                _resetZoekFilter();
            } catch (Exception e) {
                StuurSnackbar(e);
            }
        }

        private void _initialiseerZoekfilters() {
            VoertuigZoekfilters = new(Zoekmachine.GeefZoekfilterVelden(typeof(VoertuigResponseDTO), BlacklistZoekfilters));
        }

        private void _resetZoekFilter() {
            List<VoertuigResponseDTO> res = CommunicatieKanaal.GeefVoertuigen();

            Voertuigen = new(res);
        }

        private void _zoekMetFilter() {
            Func<object, object, bool> vergelijker = null;
            List<Func<List<VoertuigResponseDTO>>> dataCollectieActiesVoertuigen = new() {
                new Func<List<VoertuigResponseDTO>>(CommunicatieKanaal.GeefVoertuigen)
            };

            Voertuigen = new ObservableCollection<VoertuigResponseDTO>(
                Zoekmachine.ZoekMetFilter<VoertuigResponseDTO>(dataCollectieActiesVoertuigen, GeselecteerdeZoekfilter, ZoekveldRegular, vergelijker).ToList()
            );
            
            ZoekveldRegular = "";
        }

        private void _verwijderHighlightedVoertuig() {
            if (HighlightedVoertuig?.Id is not null) {
                CommunicatieKanaal.VerwijderVoertuig((int)HighlightedVoertuig.Id);
                _resetZoekFilter();
                StuurSnackbar("Voertuig succesvol verwijderd.");
            } else {
                StuurSnackbar("Voertuig kon niet bepaald worden. Probeer deze eerst te selecteren alvorens te verwijderen.");
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

        public ICommand VerwijderHighlightedVoertuig {
            get {
                return new RelayCommand(
                    p => _verwijderHighlightedVoertuig(),
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
