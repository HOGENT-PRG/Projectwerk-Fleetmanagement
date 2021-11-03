using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFApp.Interfaces;
using WPFApp.Model.Response;
using WPFApp.Model;
using WPFApp.Views.MVVM;

namespace WPFApp.Views {
        internal sealed class BestuurderOverzichtViewModel : Presenteerder, IPaginaViewModel {

        private ICommuniceer _communicatieKanaal;
        private List<Func<List<BestuurderResponseDTO>>> _dataCollectieActiesBestuurderDTOs;

        public string Naam => "Bestuurders";
        public ObservableCollection<string> Zoekfilters { get; private set; }

        public BestuurderOverzichtViewModel(ICommuniceer comm) {
            _communicatieKanaal = comm;
            _dataCollectieActiesBestuurderDTOs = new() { 
                new Func<List<BestuurderResponseDTO>>(_communicatieKanaal.geefBestuurders) 
            };

            Zoekfilters = new ObservableCollection<string>(
                new CommunicatieRelay().Zoekmachine.GeefZoekfilterVelden(typeof(BestuurderResponseDTO))
            );
        }

    }
}
