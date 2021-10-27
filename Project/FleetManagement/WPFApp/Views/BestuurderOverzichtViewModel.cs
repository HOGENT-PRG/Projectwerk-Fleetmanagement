using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFApp.Interfaces;
using WPFApp.Views.MVVM;

namespace WPFApp.Views {
        internal sealed class BestuurderOverzichtViewModel : Presenteerder, IPaginaViewModel {

        private ICommuniceer _communicatieKanaal;

        public BestuurderOverzichtViewModel(ICommuniceer comm) {
            _communicatieKanaal = comm;
        }

        public string Naam {
                get {
                    return "Bestuurders";
                }
            }
        }
    }
