using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFApp.Helpers;
using WPFApp.Interfaces;
using WPFApp.Model.Response;
using WPFApp.Views.MVVM;

namespace WPFApp.Views {
    internal sealed class TankkaartOverzichtViewModel : Presenteerder, IPaginaViewModel {

        public string Naam => "Tankkaarten";
        public Action<object> StuurSnackbar { get; private set; }

        private readonly ICommuniceer CommunicatieKanaal;
        private readonly Zoekmachine Zoekmachine = new();

        private List<string> BlacklistZoekfilters { get; init; } = new() {
            "Chars",
            "Length"
        };

        public ObservableCollection<string> TankkaartZoekfilters { get; private set; }

        public ObservableCollection<TankkaartResponseDTO> Tankkaarten { get; set; } = new();

        public TankkaartResponseDTO HighlightedTankkaart { get; set; } = null;

        public string GeselecteerdeZoekfilter { get; set; }
        public string ZoekveldRegular { get; set; }
        public DateTime ZoekveldDate { get; set; }

        public TankkaartOverzichtViewModel(ICommuniceer comm, Action<object> stuurSnackbar) {
            CommunicatieKanaal = comm;
            StuurSnackbar = stuurSnackbar;

            _initialiseerZoekfilters();
        }

        // Bij het daadwerkelijk renderen gaan we data opvragen en weergeven (dus niet bij initialisatie van de View+VM, aangezien dat op de achtergrond bij opstart voor alle VM's tegelijk plaatsvindt)
        private void _startupRoutine() {
            try {

            } catch (Exception e) {
                StuurSnackbar(e);
            }
        }

        private void _initialiseerZoekfilters() {

        }

        private void _resetZoekFilter() {

        }

        // switch en gebruik datumvergelijker
        private void _zoekMetFilter() {
            // ...

            ZoekveldRegular = "";
            ZoekveldDate = DateTime.Now;
        }

        private void _verwijderHighlightedTankkaart() {

        }

        // Vergelijkt datums - zonder tijd 
        // Geen datetime.parse, dat vertraagt de boel enorm (ook met parseexact)
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

        public ICommand VerwijderHighlightedTankkaart {
            get {
                return new RelayCommand(
                    p => _verwijderHighlightedTankkaart(),
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
