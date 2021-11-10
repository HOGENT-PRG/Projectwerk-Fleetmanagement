using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WPFApp.Interfaces;
using WPFApp.Model.Response;
using WPFApp.Model;
using WPFApp.Views.MVVM;
using System.Windows.Input;
using WPFApp.Helpers;

namespace WPFApp.Views {
        internal sealed class BestuurderOverzichtViewModel : Presenteerder, IPaginaViewModel {

        private ICommuniceer _communicatieKanaal;
        private List<Func<List<BestuurderResponseDTO>>> _dataCollectieActiesBestuurderDTOs;
        private List<BestuurderResponseDTO> _zoekResultaten;
        private Zoekmachine _zoekmachine = new CommunicatieRelay().Zoekmachine;

        private string _geselecteerdeZoekfilter = "";
        private object _zoekveld = "";

        public string Naam => "Bestuurders";
        public ObservableCollection<string> Zoekfilters { get; private set; }

        public BestuurderOverzichtViewModel(ICommuniceer comm) {
            _communicatieKanaal = comm;
            _dataCollectieActiesBestuurderDTOs = new() { 
                new Func<List<BestuurderResponseDTO>>(_communicatieKanaal.geefBestuurders) 
            };

            Zoekfilters = new ObservableCollection<string>(
                _zoekmachine.GeefZoekfilterVelden(typeof(BestuurderResponseDTO))
            );
        }

        public ICommand Zoek {
            get {
                return new RelayCommand(_ => Update(ref _zoekResultaten, _zoekmachine.ZoekMetFilter<BestuurderResponseDTO>(_dataCollectieActiesBestuurderDTOs, _geselecteerdeZoekfilter, _zoekveld)), p => p == p);
            }
        }

    }
}
