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

            private void _stuurSnackbar(object msg) {
                SnackbarMessage s = new();
                s.ActionContent = "VERBERG";
                s.Content = "";

                if (!msg.GetType().FullName.Contains("Exception")) {
                    PopupDialoogTitel = "Informatie";
                    (PopupDialoogContent, s.Content) = ((string)msg, (string)msg);
                } else {
                    Exception exc = msg as Exception;
                    PopupDialoogTitel = exc.GetType().Name;
                    (PopupDialoogContent, s.Content) = (exc.Message, exc.Message);
                    if (exc.InnerException != null) {
                        PopupDialoogTitel += " > " + exc.InnerException.GetType().Name;
                        PopupDialoogContent += $"\n{exc.InnerException.GetType().Name}\n{exc.InnerException.Message}";
                        s.Content += PopupDialoogContent;
                    }
                }

                s.Content += "\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t";

                SnackbarWachtrij.Enqueue(s);
            }
        }
    }
