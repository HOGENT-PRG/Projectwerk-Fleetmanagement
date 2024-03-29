﻿using System;
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
using WPFApp.Interfaces;

namespace WPFApp.Views {
    internal sealed class AdresOverzichtViewModel : Presenteerder, IPaginaViewModel {

        public string Naam => "Adressen";
        public Action<object> StuurSnackbar { get; private set; }

        private readonly ICommuniceer CommunicatieKanaal;
        private readonly Zoekmachine Zoekmachine = new();

        private List<string> BlacklistZoekfilters { get; init; } = new() {
            "Chars",
            "Length"
        };

        public ObservableCollection<string> AdresZoekfilters { get; private set; }

        public ObservableCollection<AdresResponseDTO> Adressen { get; set; } = new();

        public AdresResponseDTO HighlightedAdres { get; set; } = null;

        public string GeselecteerdeZoekfilter { get; set; }
        public string ZoekveldRegular { get; set; }

        public AdresOverzichtViewModel(ICommuniceer comm, Action<object> stuurSnackbar) {
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
            AdresZoekfilters = new(Zoekmachine.GeefZoekfilterVelden(typeof(AdresResponseDTO), BlacklistZoekfilters));
		}

        private void _resetZoekFilter() {
            Adressen = new(CommunicatieKanaal.GeefAdressen());
		}

		// Geen speciale gevallen hier, kan gewoon zonder switch 
		private void _zoekMetFilter() {
			Func<object, object, bool> vergelijker = null;
            List<Func<List<AdresResponseDTO>>> dataCollectieActieAdres = new() {
                new Func<List<AdresResponseDTO>>(CommunicatieKanaal.GeefAdressen)
            };

            Adressen = new ObservableCollection<AdresResponseDTO>(
                    Zoekmachine.ZoekMetFilter<AdresResponseDTO>(dataCollectieActieAdres, GeselecteerdeZoekfilter, ZoekveldRegular, vergelijker).ToList()
            );

		}

		private void _verwijderHighlightedAdres() {
            if (HighlightedAdres?.Id is not null)
            {
                CommunicatieKanaal.VerwijderAdres((int)HighlightedAdres.Id);
                _resetZoekFilter();
                StuurSnackbar("Adres succesvol verwijderd.");
            }
            else
            {
                StuurSnackbar("Adres kon niet bepaald worden. Probeer deze eerst te selecteren alvorens te verwijderen.");
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

        public ICommand VerwijderHighlightedAdres {
            get {
                return new RelayCommand(
                    p => _verwijderHighlightedAdres(),
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
