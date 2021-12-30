using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFApp.Views;
using WPFApp.Model.Response;
using WPFApp.Views.MVVM;
using WPFApp.Interfaces;

namespace WPFApp.Views {
        internal sealed class DatabankOverzichtViewModel : Presenteerder, IPaginaViewModel {

        public string Naam => "Databank info";  // naam van het tabblad
        private Action<object> StuurSnackbar { get; set; }

        private ICommuniceer CommunicatieKanaal;

        public bool ConnectieSuccesvol { get; private set; } = false; 
        public bool DatabaseBestaat { get; private set; } = false;
        public bool AlleTabellenBestaan { get; private set; } = false;
        public int AantalTabellen { get; private set; } = -1;
        public bool SequentieDoorlopen { get; private set; } = false;
        public DateTime LaatsteUpdate { get; private set; } = DateTime.UnixEpoch;

        public DatabankOverzichtViewModel(ICommuniceer comm, Action<object> stuurSnackbar) {
            CommunicatieKanaal = comm;
            StuurSnackbar = stuurSnackbar;
            VernieuwStatus.Execute("");
        }

        private void UpdateStaat(DatabankStatusResponseDTO res) {
            if(res is null) {
                StuurSnackbar("DatabankStatusResponse instantie werd verwacht maar null werd ontvangen, nazicht vereist.");
                return;
			}

            ConnectieSuccesvol = res.ConnectieSuccesvol ?? false;
            DatabaseBestaat = res.DatabaseBestaat ?? false;
            AlleTabellenBestaan = res.AlleTabellenBestaan ?? false;
            AantalTabellen = res.AantalTabellen ?? 0;
            SequentieDoorlopen = res.SequentieDoorlopen ?? false;
            LaatsteUpdate = DateTime.Now;
        }

        public ICommand VernieuwStatus {
            get {
                return new RelayCommand(
                    p => UpdateStaat(CommunicatieKanaal.GeefDatabankStatus()),
                    p => p is not null
                ); ;
            }
        }

        
    }
}
