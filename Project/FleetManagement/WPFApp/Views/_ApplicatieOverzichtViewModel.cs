using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using WPFApp.Interfaces;
using WPFApp.Views.Hosts;
using WPFApp.Views.MVVM;

/* Deze klasse dient opgekuist te worden ivm props & de brushes die hier in staan */
namespace WPFApp.Views {
    internal sealed class ApplicatieOverzichtViewModel : NotificatieModule, IPaginaViewModel {

        public string Naam => "Applicatie Overzicht";

        private ICommand _veranderPaginaCommand;
        private IPaginaViewModel _huidigePaginaViewModel;
        private Dictionary<string, IPaginaViewModel> _paginaViewModels;

        private ICommuniceer _communicatieKanaal = new CommunicatieRelay().CommunicatieKanaal;

        // Constructie en instelling als datacontext van ApplicatieOverzicht in App.xaml.cs
        public ApplicatieOverzichtViewModel() {

            /* Overzichten */
            PaginaViewModels.Add(nameof(AdresOverzicht), new AdresOverzichtViewModel(_communicatieKanaal, this.StuurSnackbar));
            PaginaViewModels.Add(nameof(BestuurderOverzicht), new BestuurderOverzichtViewModel(_communicatieKanaal, this.StuurSnackbar));
            PaginaViewModels.Add(nameof(TankkaartOverzicht), new TankkaartOverzichtViewModel(_communicatieKanaal, this.StuurSnackbar));
            PaginaViewModels.Add(nameof(VoertuigOverzicht), new VoertuigOverzichtViewModel(_communicatieKanaal, this.StuurSnackbar));
            PaginaViewModels.Add(nameof(DatabankOverzicht), new DatabankOverzichtViewModel(_communicatieKanaal, this.StuurSnackbar));

			/* Toevoegen */
			PaginaViewModels.Add(nameof(AdresToevoegen), new AdresToevoegenViewModel(_communicatieKanaal, this.StuurSnackbar));
			PaginaViewModels.Add(nameof(BestuurderToevoegen), new BestuurderToevoegenViewModel(_communicatieKanaal, this.StuurSnackbar));
			PaginaViewModels.Add(nameof(TankkaartToevoegen), new TankkaartToevoegenViewModel(_communicatieKanaal, this.StuurSnackbar));
			PaginaViewModels.Add(nameof(VoertuigToevoegen), new VoertuigToevoegenViewModel(_communicatieKanaal, this.StuurSnackbar));

			/* Wijzigen */

			/* ViewModel dat gebruikt wordt bij opstart applicatie: AdresOverzicht */
			HuidigePaginaViewModel = PaginaViewModels[nameof(AdresOverzicht)];
        }


        private object MaakNieuwViewModel(IPaginaViewModel viewModel) {
            // in de veronderstelling dat elke view deze 2 argumenten bevat
            var p = new object[] { _communicatieKanaal, this.StuurSnackbar };

            try {
                // aanmaken instantie
                object o = Activator.CreateInstance(viewModel.GetType(), p);

                // indien het aangemaakte viewmodel een startuproutine command bevat wordt deze aangeroepen
                // (bij first run gebeurt dit door Loaded event)    
                if (o.GetType().GetProperty("StartupRoutine") != null) {
                    var x = o.GetType().GetProperty("StartupRoutine").GetGetMethod(true).Invoke(o, Array.Empty<object>());
                    var y = x.GetType().GetMethod("Execute").Invoke(x, new object[1] { "" });
                }

                return o;
            } catch (Exception e) { StuurSnackbar(e); }

            // retourneren om toe te voegen aan PaginaViewModels
            return null;
        }

        private void ResetViewModel(IPaginaViewModel viewModel) {
            PaginaViewModels[viewModel.GetType().Name.Replace("ViewModel", "")] = (IPaginaViewModel)MaakNieuwViewModel(viewModel);
        }

        private void VeranderViewModel(IPaginaViewModel viewModel) {
            string naam = viewModel.GetType().Name.Replace("ViewModel", "");
            if (!PaginaViewModels.Keys.Contains(naam)) {
                PaginaViewModels.Add(naam, (IPaginaViewModel)MaakNieuwViewModel(viewModel));
            }

            HuidigePaginaViewModel = PaginaViewModels.FirstOrDefault(vm => vm.Key == naam).Value;
        }

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
                if (_veranderPaginaCommand == null) {
                    _veranderPaginaCommand = new RelayCommand(
                        p => VeranderViewModel((IPaginaViewModel)p),
                        p => p is IPaginaViewModel);
                }

                return _veranderPaginaCommand;
            }
        }

        public Dictionary<string, IPaginaViewModel> PaginaViewModels {
            get {
                if (_paginaViewModels == null)
                    _paginaViewModels = new Dictionary<string, IPaginaViewModel>();

                return _paginaViewModels;
            }
        }

        public IPaginaViewModel HuidigePaginaViewModel {
            get {
                return _huidigePaginaViewModel;
            }
            set {
                Update(ref _huidigePaginaViewModel, value);
            }
        }

        public SolidColorBrush TabbladTekstKleur => (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF312F2F"));
        public SolidColorBrush ActiefTabbladKleur => (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFFFF"));
        public SolidColorBrush InactiefTabbladKleur => (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFE1E1E1"));
        public SolidColorBrush TabbladOnderlijningKleur => (SolidColorBrush)(new BrushConverter().ConvertFrom("#00000000"));
    }
}