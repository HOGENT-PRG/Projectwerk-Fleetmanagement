using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPFApp.Views.MVVM;

namespace WPFApp.Views.Hosts {

    // Deze module wordt gebruikt in het ApplicatieOverzicht
    // De viewmodel geeft de StuurSnackbar property mee in hun constructors
    // dit laat alle viewmodels toe om snackbars inclusief dialoog te versturen
    // naar de frontend
    // De snackbar bevat een knop 'MEER INFO', die het dialoog opent om alle details / te lange content 
    // te kunnen zien

    // Het opnemen ervan in de main viewmodel voorkomt het herhalen van de bijbehorende xaml code in elke view model
    internal abstract class NotificatieModule : Presenteerder {
        // Snackbar & bijhorend dialog box
        // https://material.io/components/snackbars
        // https://material.io/components/dialogs

        public int SnackbarLifetimeMs = 7000;
        public SnackbarMessageQueue SnackbarWachtrij { get; set; }
        public string PopupDialoogTitel { get; set; } = "Informatie";
        public string PopupDialoogContent { get; set; } = "";
        public bool DialoogInitiatieIndicator { get; set; } = false;
        public Action<object> StuurSnackbar { get; }

        public NotificatieModule() {
            SnackbarWachtrij = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(SnackbarLifetimeMs)) {
                DiscardDuplicates = true
            };
            StuurSnackbar = _stuurSnackbar;
        }

        protected void ToonNotificatie(object sender, RoutedEventArgs e) {
            DialoogInitiatieIndicator = !DialoogInitiatieIndicator;
        }

        private void _stuurSnackbar(object msg) {
            SnackbarMessage s = new();
            string t;
            s.ActionContent = "MEER INFO";
            s.ActionClick += ToonNotificatie;

            if (!msg.GetType().FullName.Contains("Exception")) {
                t = (string)msg;
                PopupDialoogTitel = "Informatie";
                PopupDialoogContent = t;
                s.Content = t;
            } else {
                Exception exc = (Exception)msg;
                string geneste_exc_naam = "fin";
                string geneste_exc_waarde = "fin";
                if (exc.InnerException != null) {
                    geneste_exc_naam = exc.InnerException.GetType().Name;
                    geneste_exc_waarde = exc.InnerException.Message;
                }
                t = exc.GetType().Name + "\n" + exc.Message;
                PopupDialoogTitel = exc.GetType().Name + (geneste_exc_naam == "fin" ? "" : $" > {geneste_exc_naam}");
                PopupDialoogContent = exc.Message + (geneste_exc_naam == "fin" ? "" : $"\n\n{geneste_exc_naam}\n{geneste_exc_waarde}");
                s.Content = t;
            }

            SnackbarWachtrij.Enqueue(s);
        }
    }
}
