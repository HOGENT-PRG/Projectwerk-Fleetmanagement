using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFApp.Views.MVVM;

namespace WPFApp.Views {
    internal sealed class ApplicatieOverzichtViewModel : Presenteerder, IPaginaViewModel {

        private ICommand _veranderPaginaCommand;
        private IPaginaViewModel _huidigePaginaViewModel;
        private List<IPaginaViewModel> _paginaViewModels;

        public ApplicatieOverzichtViewModel() {

            PaginaViewModels.Add(new AdresOverzichtViewModel());
            PaginaViewModels.Add(new BestuurderOverzichtViewModel());
            PaginaViewModels.Add(new TankkaartOverzichtViewModel());
            PaginaViewModels.Add(new VoertuigOverzichtViewModel());
            PaginaViewModels.Add(new DatabankOverzichtViewModel());

            // Set starting page
            HuidigePaginaViewModel = PaginaViewModels[0];
        }

        public string Naam { get { return "Applicatie Overzicht"; } }


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

                //if (_currentPageViewModel != value) {
                //    _currentPageViewModel = value;
                //    OnPropertyChanged("CurrentPageViewModel");
                //}
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