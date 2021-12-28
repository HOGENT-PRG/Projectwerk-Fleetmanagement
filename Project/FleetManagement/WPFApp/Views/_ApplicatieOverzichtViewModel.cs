using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using WPFApp.Interfaces;
using WPFApp.Model.Response;
using WPFApp.Views.Hosts;
using WPFApp.Views.MVVM;

<<<<<<< HEAD
/* Deze klasse dient opgekuist te worden ivm props & de brushes die hier in staan */
namespace WPFApp.Views {
=======
namespace WPFApp.Views {
    /*
        Deze klasse fungeert als beheerder van de overige Views en ViewModels.
        De View ApplicatieOverzicht bevat een ContentControl waar de HuidigePaginaViewModel 
        als Content ingesteld staat.
        Indien HuidigePaginaViewModel wijzigt zal de weergegeven View ook wijzigen.
    */
>>>>>>> parent of 87a59f3 (Fix requestDTOnaarDomein enum parsing, verplaatsen interface, RRNValideerder soft error, extra check bestuurdermgr, overbodige vpp files weg)
    internal sealed class ApplicatieOverzichtViewModel : NotificatieModule, IPaginaViewModel {

        public string Naam => "Applicatie Overzicht";

        private ICommand _veranderPaginaCommand;
        private IPaginaViewModel _huidigePaginaViewModel;
        private Dictionary<string, IPaginaViewModel> _paginaViewModels;

        private ICommuniceer _communicatieKanaal = new CommunicatieRelay().CommunicatieKanaal;
<<<<<<< HEAD

        // Constructie en instelling als datacontext van ApplicatieOverzicht in App.xaml code behind
        public ApplicatieOverzichtViewModel() {
=======

        // Constructie en instelling als datacontext van ApplicatieOverzicht in App.xaml code behind
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
            PaginaViewModels.Add(nameof(BestuurderWijzigen), new BestuurderWijzigenViewModel(_communicatieKanaal, this.StuurSnackbar));
            PaginaViewModels.Add(nameof(AdresWijzigen), new AdresWijzigenViewModel(_communicatieKanaal, this.StuurSnackbar));
            PaginaViewModels.Add(nameof(VoertuigWijzigen), new VoertuigWijzigenViewModel(_communicatieKanaal, this.StuurSnackbar));
            PaginaViewModels.Add(nameof(TankkaartWijzigen), new TankkaartWijzigenViewModel(_communicatieKanaal, this.StuurSnackbar));
>>>>>>> parent of 1c00aff (ApplicatieOverzichtViewModel herwerken + inline commentaar toevoegen in meerdere files)

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

            /* Wijzigen, overige via dialogs */
            PaginaViewModels.Add(nameof(BestuurderWijzigen), new BestuurderWijzigenViewModel(_communicatieKanaal, this.StuurSnackbar));
            PaginaViewModels.Add(nameof(AdresWijzigen), new AdresWijzigenViewModel(_communicatieKanaal, this.StuurSnackbar));
            PaginaViewModels.Add(nameof(VoertuigWijzigen), new VoertuigWijzigenViewModel(_communicatieKanaal, this.StuurSnackbar));
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

        /* Wijzigen */
<<<<<<< HEAD
=======
        private void WijzigAdres(AdresResponseDTO a) {
            VeranderViewModel(PaginaViewModels[nameof(AdresWijzigen)]);
            AdresWijzigenViewModel awvm = (AdresWijzigenViewModel)PaginaViewModels[nameof(AdresWijzigen)];
            awvm.BereidModelVoorAdres(a);
        }

        public ICommand WijzigAdresCommand {
            get {
                return new RelayCommand(
                    p => WijzigAdres((AdresResponseDTO)p),
                    p => p is not null);
            }
        }
>>>>>>> parent of 1c00aff (ApplicatieOverzichtViewModel herwerken + inline commentaar toevoegen in meerdere files)

        private void WijzigBestuurder(BestuurderResponseDTO b) {
            VeranderViewModel(PaginaViewModels[nameof(BestuurderWijzigen)]);
            BestuurderWijzigenViewModel bwvm = (BestuurderWijzigenViewModel)PaginaViewModels[nameof(BestuurderWijzigen)];
            bwvm.BereidModelVoorMetBestuurder(b);
        }
<<<<<<< HEAD
        private void WijzigVoertuig(VoertuigResponseDTO v)
        {
            VeranderViewModel(PaginaViewModels[nameof(VoertuigWijzigen)]);
            VoertuigWijzigenViewModel vwvm = (VoertuigWijzigenViewModel)PaginaViewModels[nameof(VoertuigWijzigen)];
            vwvm.BereidModelVoorMetVoertuig(v);
        }
=======
>>>>>>> parent of 1c00aff (ApplicatieOverzichtViewModel herwerken + inline commentaar toevoegen in meerdere files)
        public ICommand WijzigBestuurderCommand {
            get {
                return new RelayCommand(
                    p => WijzigBestuurder((BestuurderResponseDTO)p),
                    p => p is not null
                );
            }
        }
<<<<<<< HEAD
=======

        private void WijzigVoertuig(VoertuigResponseDTO v)
        {
            VeranderViewModel(PaginaViewModels[nameof(VoertuigWijzigen)]);
            VoertuigWijzigenViewModel vwvm = (VoertuigWijzigenViewModel)PaginaViewModels[nameof(VoertuigWijzigen)];
            vwvm.BereidModelVoorMetVoertuig(v);
        }
        
>>>>>>> parent of 1c00aff (ApplicatieOverzichtViewModel herwerken + inline commentaar toevoegen in meerdere files)
        public ICommand WijzigVoertuigCommand
        {
            get
            {
                return new RelayCommand(
                    p => WijzigVoertuig((VoertuigResponseDTO)p),
                    p => p is not null
                );
            }
        }
<<<<<<< HEAD
        public ICommand WijzigAdresCommand
        {
            get
            {
                return new RelayCommand(
                    p => WijzigAdres((AdresResponseDTO)p),
                    p => p is not null);
            }
        }
        private void WijzigAdres(AdresResponseDTO a)
        {
            VeranderViewModel(PaginaViewModels[nameof(AdresWijzigen)]);
            AdresWijzigenViewModel awvm = (AdresWijzigenViewModel)PaginaViewModels[nameof(AdresWijzigen)];
            awvm.BereidModelVoorAdres(a);
=======

        private void WijzigTankkaart(TankkaartResponseDTO t) {
            VeranderViewModel(PaginaViewModels[nameof(TankkaartWijzigen)]);
            TankkaartWijzigenViewModel twvm = (TankkaartWijzigenViewModel)PaginaViewModels[nameof(TankkaartWijzigen)];
            twvm.BereidModelVoorMetTankkaart(t);
        }

        public ICommand WijzigTankkaartCommand {
            get {
                return new RelayCommand(
                    p => WijzigTankkaart((TankkaartResponseDTO)p),
                    p => p is not null
                );
            }
>>>>>>> parent of 1c00aff (ApplicatieOverzichtViewModel herwerken + inline commentaar toevoegen in meerdere files)
        }

        /* Wijzigen einde */

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