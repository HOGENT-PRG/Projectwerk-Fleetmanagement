using MaterialDesignThemes.Wpf;
using System;
using WPFApp.Views.MVVM;

namespace WPFApp.Views.Hosts {

    // Deze module wordt gebruikt in het ApplicatieOverzicht
    // De ApplicatieOverzichtViewModel geeft de StuurSnackbar property mee in hun constructors
    // Dit laat alle viewmodels toe om snackbars inclusief dialoog te versturen
    // De snackbar bevat een knop 'VERBERG', die het dialoog verbergt, daarnaast kan er gedubbelklikt worden op
    // de snackbar om alle details / te lange content te kunnen zien

    // Het opnemen van de snackbar en dialoghost xaml in het ApplicatieOverzichtViewModel
    // voorkomt het herhalen van de xaml in elke view model
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
