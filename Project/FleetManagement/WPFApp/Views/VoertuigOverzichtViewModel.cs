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

        private ICommuniceer _communicatieKanaal;

        public VoertuigOverzichtViewModel(ICommuniceer comm) {
            _communicatieKanaal = comm;
        }

    }
}
