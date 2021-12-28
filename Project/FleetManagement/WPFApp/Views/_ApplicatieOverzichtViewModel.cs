using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using WPFApp.Helpers;
using WPFApp.Interfaces;
using WPFApp.Model.Response;
using WPFApp.Views.Hosts;
using WPFApp.Views.MVVM;

namespace WPFApp.Views {
    /*
        Deze klasse fungeert als beheerder van de overige Views en ViewModels.
        De View ApplicatieOverzicht bevat een ContentControl waar de HuidigePaginaViewModel 
        als Content ingesteld staat.
        Indien HuidigePaginaViewModel wijzigt zal de weergegeven View ook wijzigen.
    */
    internal sealed class ApplicatieOverzichtViewModel : NotificatieModule, IPaginaViewModel {
        // Het ViewModel welke momenteel actief is, door declaratie ervan in ApplicatieOverzicht.xaml in de Window.Resources wordt er automatisch overgeschakeld naar diens View
        public IPaginaViewModel HuidigePaginaViewModel { get; private set; }

        // ViewModels die beschikbaar zijn. Key=View naam, Value=ViewModel
        // Zie constructor
        public Dictionary<string, IPaginaViewModel> PaginaViewModels { get; init; }

        // Eenmalige initialisatie van CommunicatieRelay, de ViewModels worden voorzien van deze implementatie middels hun constructor
        private ICommuniceer CommunicatieKanaal = new CommunicatieRelay().CommunicatieKanaal;

        // Alle views en viewmodels toevoegen aan de dict
        // Aanroepen van de constructor van OverzichtViewModels wilt echter niet zeggen dat er al data ingeladen wordt, anders zou dat tijdens initialisatie te intensief / verspilling van resources zijn.
        // Inladen van data gebeurt pas bij het renderen van de view dmv het Loaded event die de StartupRoutine command welke beschikbaar gesteld wordt door het ViewModel aanroept.
        public ApplicatieOverzichtViewModel() {

            // Het meegeven van de StuurSnackbar functie is mogelijk door overerving van de NotificatieModule
            PaginaViewModels = new() {
                /* Overzichten */
                { nameof(AdresOverzicht), 
                    new AdresOverzichtViewModel(CommunicatieKanaal, StuurSnackbar) 
                },
                { nameof(BestuurderOverzicht),
                    new BestuurderOverzichtViewModel(CommunicatieKanaal, StuurSnackbar)
                },
                { nameof(TankkaartOverzicht),
                    new TankkaartOverzichtViewModel(CommunicatieKanaal, StuurSnackbar)
                },
                { nameof(VoertuigOverzicht),
                    new VoertuigOverzichtViewModel(CommunicatieKanaal, StuurSnackbar)
                },
                { nameof(DatabankOverzicht),
                    new DatabankOverzichtViewModel(CommunicatieKanaal, StuurSnackbar)
                },
                /* Toevoegen */
                { nameof(AdresToevoegen),
                    new AdresToevoegenViewModel(CommunicatieKanaal, StuurSnackbar)
                },
                { nameof(BestuurderToevoegen),
                    new BestuurderToevoegenViewModel(CommunicatieKanaal, StuurSnackbar)
                },
                { nameof(TankkaartToevoegen),
                    new TankkaartToevoegenViewModel(CommunicatieKanaal, StuurSnackbar)
                },
                { nameof(VoertuigToevoegen),
                    new VoertuigToevoegenViewModel(CommunicatieKanaal, StuurSnackbar)
                },
                /* Wijzigen */
                { nameof(BestuurderWijzigen),
                    new BestuurderWijzigenViewModel(CommunicatieKanaal, StuurSnackbar)
                },
                { nameof(AdresWijzigen),
                    new AdresWijzigenViewModel(CommunicatieKanaal, StuurSnackbar)
                },
                { nameof(VoertuigWijzigen),
                    new VoertuigWijzigenViewModel(CommunicatieKanaal, StuurSnackbar)
                },
                { nameof(TankkaartWijzigen),
                    new TankkaartWijzigenViewModel(CommunicatieKanaal, StuurSnackbar)
                }
            };

            /* ViewModel dat gebruikt wordt bij opstart applicatie: AdresOverzicht */
            HuidigePaginaViewModel = PaginaViewModels[nameof(AdresOverzicht)];
        }

        private object MaakNieuwViewModel(IPaginaViewModel viewModel) {
            // In de veronderstelling dat elke ViewModel constructor deze 2 argumenten aanvaard, het zou mogelijk kunnen zijn om in de abstracte klasse Presenteerder een constructor aan te maken met deze 2 argumenten zodat aanwezigheid gewaarborgd is
            var p = new object[] { CommunicatieKanaal, this.StuurSnackbar };

            try {
                // Aanmaken instantie
                object o = Activator.CreateInstance(viewModel.GetType(), p);

                // Indien het aangemaakte ViewModel een startuproutine command bevat wordt deze aangeroepen 
                if (o.GetType().GetProperty("StartupRoutine") != null) {
                    var x = o.GetType().GetProperty("StartupRoutine").GetGetMethod(true).Invoke(o, Array.Empty<object>());
                    var y = x.GetType().GetMethod("Execute").Invoke(x, new object[1] { "" });
                }

                // Retourneren om toe te voegen aan PaginaViewModels
                return o;
            } catch (Exception e) { StuurSnackbar(e); }

            // Aanmaken was niet mogelijk
            StuurSnackbar("Waarschuwing: aanmaken van een ViewModel is mislukt, debugging is aangewezen.");
            return null;
        }

        #region Helpers
        // Wordt gebruikt om te verzekeren dat een IPaginaViewModel implementatie IWijzigViewModel implementeert in ActiveerWijzigenContext
        private static bool ImplementeertTypeInterface(Type t, Type verwachtteInterface) {
            return t.GetInterfaces().Any(i => i.GetType() == verwachtteInterface.GetType());
        }
        #endregion

        #region ViewModel beheer
        // Wordt aangeroepen middels Reset knoppen in ToevoegenViewModels, het is eenvoudiger en globaal toepasbaar om de ViewModel te vervangen met een nieuwe, in plaats van in elke ViewModel alle velden / properties manueel te resetten met risico op fouten
        private void ResetViewModel(IPaginaViewModel viewModel) {
            PaginaViewModels[viewModel.GetType().Name.Replace("ViewModel", "")] = (IPaginaViewModel)MaakNieuwViewModel(viewModel);
        }

        // Veranderen van ViewModel, door middel van een tabblad knop om van Overzicht te veranderen of vanuit een Overzicht om naar Toevoegen/Wijzigen te gaan
        private void VeranderViewModel(IPaginaViewModel viewModel) {
            string naam = viewModel.GetType().Name.Replace("ViewModel", "");
            if (!PaginaViewModels.Keys.Contains(naam)) {
                PaginaViewModels.Add(naam, (IPaginaViewModel)MaakNieuwViewModel(viewModel));
            }

            HuidigePaginaViewModel = PaginaViewModels.FirstOrDefault(vm => vm.Key == naam).Value;
        }

        // Wordt aangeroepen vanuit een Overzicht datagrid row met als binding de datasource van die row, welke een IResponseDTO is. 
        // Na enkele checks wordt de Wijzigen view weergegeven en de BereidModelVoor functie aangeroepen welke het WijzigenViewModel zal voorzien van de responseDTO.
        private void ActiveerWijzigenContext(IResponseDTO responseDTO) {
            var castedDTO = Convert.ChangeType(responseDTO, responseDTO.GetType());
            string dtoTypeNaam = responseDTO.GetType().Name;
            string xamlName = dtoTypeNaam.Replace("ResponseDTO", "Wijzigen");

            if (!dtoTypeNaam.EndsWith("ResponseDTO")) {
                StuurSnackbar($"Weergave van wijzigen context niet mogelijk, een implementatie van IResponseDTO dient als naamgeving steeds te eindigen op 'ResponseDTO', echter werd '{dtoTypeNaam}' ontvangen. ");
                return;
			}

			if (!PaginaViewModels.ContainsKey(xamlName)) {
                StuurSnackbar("Weergave van wijzigen context niet mogelijk aangezien correcte ViewModel niet gevonden kan worden.");
                return;
			}

            IPaginaViewModel geselecteerdeViewModel = PaginaViewModels[xamlName];

            if(!ImplementeertTypeInterface(geselecteerdeViewModel.GetType(), typeof(IWijzigViewModel))) {
                StuurSnackbar("Er werd een ViewModel geselecteerd, echter implementeert deze IWijzigViewModel niet terwijl dat wel een vereiste is.");
                return;
			}

            VeranderViewModel(PaginaViewModels[xamlName]);
            IWijzigViewModel wijzigViewModel = PaginaViewModels[xamlName] as IWijzigViewModel;
            wijzigViewModel.BereidModelVoor(responseDTO);
        }

        #endregion

        // Commando's die callable zijn door ApplicatieOverzicht en zijn descendants (alle Views dus)
        public ICommand ResetViewModelCommand {
            get {
                return new RelayCommand(
                    p => ResetViewModel((IPaginaViewModel)p),
                    p => p is IPaginaViewModel
                );
            }
        }

        public ICommand VeranderPaginaCommand {
            get {
                return new RelayCommand(
                        p => VeranderViewModel((IPaginaViewModel)p),
                        p => p is IPaginaViewModel
                );
            }
        }

        public ICommand WijzigItemCommand {
            get {
                return new RelayCommand(
                    p => ActiveerWijzigenContext((IResponseDTO)p),
                    p => p is not null
                );
            }
        }

    }
}