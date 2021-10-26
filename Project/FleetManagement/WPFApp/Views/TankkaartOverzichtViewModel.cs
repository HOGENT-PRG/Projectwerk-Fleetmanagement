using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFApp.Views.MVVM;

namespace WPFApp.Views {
    internal sealed class TankkaartOverzichtViewModel : Presenteerder, IPaginaViewModel {
            public string Naam {
                get {
                    return "Tankkaarten";
                }
            }
        }
    }
