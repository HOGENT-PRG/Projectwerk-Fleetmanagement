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
        private Dictionary<string, IPaginaViewModel> _paginaViewModels;

        private ICommuniceer _communicatieKanaal = new CommunicatieRelay().CommunicatieKanaal;

        public ApplicatieOverzichtViewModel() {

            PaginaViewModels.Add(typeof(AdresOverzicht).Name, new AdresOverzichtViewModel(_communicatieKanaal));
            PaginaViewModels.Add(typeof(BestuurderOverzicht).Name, new BestuurderOverzichtViewModel(_communicatieKanaal));
            PaginaViewModels.Add(typeof(TankkaartOverzicht).Name, new TankkaartOverzichtViewModel(_communicatieKanaal));
            PaginaViewModels.Add(typeof(VoertuigOverzicht).Name, new VoertuigOverzichtViewModel(_communicatieKanaal));
            PaginaViewModels.Add(typeof(DatabankOverzicht).Name, new DatabankOverzichtViewModel(_communicatieKanaal));

            HuidigePaginaViewModel = PaginaViewModels[typeof(AdresOverzicht).Name];
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

        private void VeranderViewModel(IPaginaViewModel viewModel) {
            if (!PaginaViewModels.Values.Contains(viewModel)) {
                PaginaViewModels.Add(viewModel.GetType().Name.Replace("ViewModel", ""), viewModel);
            }

            HuidigePaginaViewModel = PaginaViewModels
                .FirstOrDefault(vm => vm.Value == viewModel).Value;
        }
    }
}