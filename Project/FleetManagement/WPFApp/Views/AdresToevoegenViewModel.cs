using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using WPFApp.Interfaces;
using WPFApp.Model.Hosts;
using WPFApp.Views.MVVM;
using System.ComponentModel;
using System.Reflection;
using WPFApp.Model.Response;
using WPFApp.Model.Request;
using System.Windows.Input;
using WPFApp.Helpers;
using WPFApp.Model.Mappers;

namespace WPFApp.Views
{
    class AdresToevoegenViewModel : FilterDialogs , IPaginaViewModel
    {
        public string Naam => "Adres Toevoegen";
        public ICommuniceer _communicatieKanaal;
        public Action<object> StuurSnackbar { get; init; }
        public ObservableCollection<AdresResponseDTO> Adressen { get; set; } = new();

        public string Straatnaam { get; set; } = "";
        public string Huisnummer { get; set; } = "";
        public string Postcode { get; set; } = "";
        public string Plaatsnaam { get; set; } = "";
        public string Provincie { get; set; } = "";
        public string Land { get; set; } = "";
        public AdresToevoegenViewModel(ICommuniceer communicatiekanaal,Action<object> stuurSnackbar)
        {
            _communicatieKanaal = communicatiekanaal;
            StuurSnackbar = stuurSnackbar;
        }

        private void _startupRoutine()
        {
            try
            {
                _resetAdresFilters();
            }
            catch (Exception e)
            {
                StuurSnackbar(e);
            }
        }
        private void _resetAdresFilters()
        {
            Adressen = new ObservableCollection<AdresResponseDTO>(_communicatieKanaal.GeefAdressen());
        }


        private bool _controleerVeldenVoldaanVoorToevoegen()
        {

            bool voldaan = Straatnaam.Length > 0
                          && Huisnummer.Length > 0
                          && Postcode.Length > 0
                          && Plaatsnaam.Length > 0
                          && Provincie.Length > 0
                          && Land.Length > 0;
                       

            if (!voldaan)
            {
                StuurSnackbar("Adres voldoet niet aan vereisten.\nGelieve de velden in te vullen.");
            }

            return voldaan;
        }
        private void _voegAdresToe()
        {
            if (_controleerVeldenVoldaanVoorToevoegen())
            {
                try
                {  // TODO
                    AdresRequestDTO adres = new(null,Straatnaam,Huisnummer,Postcode,Plaatsnaam,Provincie,Land);

                    int id = _communicatieKanaal.VoegAdresToe(adres);
                    StuurSnackbar($"Succesvol toegevoegd met id {id}");

                    Straatnaam = "";
                    Huisnummer = "";
                    Postcode = "";
                    Plaatsnaam = "";
                    Provincie = "";
                    Land = "";
                  


                }
                catch (Exception e)
                {
                    StuurSnackbar(e);
                }
            }
        }
        public ICommand StartupRoutine
        {
            get
            {
                return new RelayCommand(
                    p => _startupRoutine(),
                    p => p is not null
                );

            }
        }
        public ICommand VoegAdresToe
        {
            get
            {
                return new RelayCommand(
                    p => _voegAdresToe(),
                    p => p is not null
                );
            }
        }
        public ICommand ResetAdresFilters
        {
            get
            {
                return new RelayCommand(
                    p => _resetAdresFilters(),
                    p => p is not null
                );
            }
        }
    }
}
