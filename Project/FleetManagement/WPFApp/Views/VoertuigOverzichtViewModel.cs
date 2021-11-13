using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFApp.Interfaces;
using WPFApp.Views.MVVM;

namespace WPFApp.Views {
        internal sealed class VoertuigOverzichtViewModel : Presenteerder, IPaginaViewModel {

        public string Naam => "Voertuigen";
        public Action<object> StuurSnackbar { get; private set; }

        private ICommuniceer _communicatieKanaal;

        public VoertuigOverzichtViewModel(ICommuniceer comm, Action<object> stuurSnackbar) {
            _communicatieKanaal = comm;
            StuurSnackbar = stuurSnackbar;
        }

    }
}
