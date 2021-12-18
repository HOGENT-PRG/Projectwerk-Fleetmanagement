using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFApp.Views.MVVM {
    // "Marker" interface, vergemakkelijkt omgang in ApplicatieOverzichtViewModel
    public interface IPaginaViewModel {
        string Naam { get; }
    }
}
