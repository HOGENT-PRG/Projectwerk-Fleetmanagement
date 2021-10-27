using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFApp.Interfaces;
using WPFApp.Views.MVVM;

namespace WPFApp.Views {
    internal sealed class TankkaartOverzichtViewModel : Presenteerder, IPaginaViewModel {

        private ICommuniceer _communicatieKanaal;

        public TankkaartOverzichtViewModel(ICommuniceer comm) {
            _communicatieKanaal = comm;
        }


        public string Naam {
                get {
                    return "Tankkaarten";
                }
            }
        }
    }
