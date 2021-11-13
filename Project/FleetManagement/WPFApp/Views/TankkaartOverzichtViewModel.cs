using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFApp.Interfaces;
using WPFApp.Views.MVVM;

namespace WPFApp.Views {
    internal sealed class TankkaartOverzichtViewModel : Presenteerder, IPaginaViewModel {

        public string Naam => "Tankkaarten";
        public Action<object> StuurSnackbar { get; private set; }

        private ICommuniceer _communicatieKanaal;

        public TankkaartOverzichtViewModel(ICommuniceer comm, Action<object> stuurSnackbar) {
            _communicatieKanaal = comm;
            StuurSnackbar = stuurSnackbar;
        }


    }
}
