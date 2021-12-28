using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WPFApp.Interfaces;

namespace WPFApp
{

    public sealed partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);

            ApplicatieOverzicht applicatie = new ApplicatieOverzicht();
            ApplicatieOverzichtViewModel context = new ApplicatieOverzichtViewModel();
            applicatie.DataContext = context;
            applicatie.Show();
        }
    }
}
