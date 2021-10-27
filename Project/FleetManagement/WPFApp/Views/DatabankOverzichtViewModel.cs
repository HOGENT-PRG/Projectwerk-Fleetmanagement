using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFApp.Interfaces;
using WPFApp.Views.MVVM;

namespace WPFApp.Views {
        internal sealed class DatabankOverzichtViewModel : Presenteerder, IPaginaViewModel {

        private ICommuniceer _communicatieKanaal;

        public DatabankOverzichtViewModel(ICommuniceer comm) {
            _communicatieKanaal = comm;
        }

        public string Naam {
                get {
                    return "Databank info";
                }
            }
        }
    }
