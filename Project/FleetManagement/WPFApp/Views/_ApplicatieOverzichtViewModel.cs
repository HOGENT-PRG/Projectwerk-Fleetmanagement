using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using WPFApp.Interfaces;
using WPFApp.Views.MVVM;

namespace WPFApp.Views {
    internal sealed class ApplicatieOverzichtViewModel : Presenteerder, IPaginaViewModel {

        public string Naam => "Applicatie Overzicht";

        public SolidColorBrush TabbladTekstKleur => (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF312F2F"));
        public SolidColorBrush ActiefTabbladKleur => (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFFFF"));
        public SolidColorBrush InactiefTabbladKleur => (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFE1E1E1"));
        public SolidColorBrush TabbladOnderlijningKleur => (SolidColorBrush)(new BrushConverter().ConvertFrom("#00000000"));

        private ICommand _veranderPaginaCommand;
        private IPaginaViewModel _huidigePaginaViewModel;
        private List<IPaginaViewModel> _paginaViewModels;

        private ICommuniceer _communicatieKanaal = new CommunicatieRelay().CommunicatieKanaal;

        public ApplicatieOverzichtViewModel() {

            PaginaViewModels.Add(new AdresOverzichtViewModel(_communicatieKanaal));
            PaginaViewModels.Add(new BestuurderOverzichtViewModel(_communicatieKanaal));
            PaginaViewModels.Add(new TankkaartOverzichtViewModel(_communicatieKanaal));
            PaginaViewModels.Add(new VoertuigOverzichtViewModel(_communicatieKanaal));
            PaginaViewModels.Add(new DatabankOverzichtViewModel(_communicatieKanaal));

            HuidigePaginaViewModel = PaginaViewModels[0];
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

        public List<IPaginaViewModel> PaginaViewModels {
            get {
                if (_paginaViewModels == null)
                    _paginaViewModels = new List<IPaginaViewModel>();

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

        private void VeranderViewModel(IPaginaViewModel viewModel) {
            if (!PaginaViewModels.Contains(viewModel)) {
                PaginaViewModels.Add(viewModel);
            }

            HuidigePaginaViewModel = PaginaViewModels
                .FirstOrDefault(vm => vm == viewModel);
        }
    }
}