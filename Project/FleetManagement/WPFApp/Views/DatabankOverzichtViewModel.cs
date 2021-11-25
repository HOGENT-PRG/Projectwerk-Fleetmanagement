using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFApp.Interfaces;
using WPFApp.Model.Response;
using WPFApp.Views.MVVM;

namespace WPFApp.Views {
        internal sealed class DatabankOverzichtViewModel : Presenteerder, IPaginaViewModel {

        public string Naam => "Databank info";  // naam van het tabblad
        public Action<object> StuurSnackbar { get; private set; }

        private ICommuniceer _communicatieKanaal;

        private bool _ConnectieSuccesvol;
		private bool _DatabaseBestaat;
		private bool _AlleTabellenBestaan;
		private int _AantalTabellen;
		private bool _SequentieDoorlopen;
		private DateTime _LaatsteUpdate = DateTime.Now;

        public bool ConnectieSuccesvol { get => _ConnectieSuccesvol; set => Update(ref _ConnectieSuccesvol, value); } 
        public bool DatabaseBestaat { get => _DatabaseBestaat; set => Update(ref _DatabaseBestaat, value); }
        public bool AlleTabellenBestaan { get => _AlleTabellenBestaan; set => Update(ref _AlleTabellenBestaan, value); }
        public int AantalTabellen { get => _AantalTabellen; set => Update(ref _AantalTabellen, value); }
        public bool SequentieDoorlopen { get => _SequentieDoorlopen; set => Update(ref _SequentieDoorlopen, value); }
        public DateTime LaatsteUpdate { get => _LaatsteUpdate; set => Update(ref _LaatsteUpdate, value); }

        private void UpdateStaat(DatabankStatusResponseDTO res) {
            ConnectieSuccesvol = res.ConnectieSuccesvol ?? false;
            DatabaseBestaat = res.DatabaseBestaat ?? false;
            AlleTabellenBestaan = res.AlleTabellenBestaan ?? false;
            AantalTabellen = res.AantalTabellen ?? 0;
            SequentieDoorlopen = res.SequentieDoorlopen ?? false;
            LaatsteUpdate = DateTime.Now;
        }

        public ICommand VernieuwStatus {
            get {
                return new Command(_ => UpdateStaat(_communicatieKanaal.GeefDatabankStatus()));
            }
        }

        public DatabankOverzichtViewModel(ICommuniceer comm, Action<object> stuurSnackbar) {
            _communicatieKanaal = comm;
            StuurSnackbar = stuurSnackbar;
            VernieuwStatus.Execute(null);
        }

        
    }
}
