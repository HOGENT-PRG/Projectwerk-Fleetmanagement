using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using WPFApp.Views;
using WPFApp.Model.Hosts;
using WPFApp.Views.MVVM;
using System.ComponentModel;
using System.Reflection;
using WPFApp.Model.Response;
using WPFApp.Model.Request;
using System.Windows.Input;
using WPFApp.Helpers;
using WPFApp.Model.Mappers;
using WPFApp.Interfaces;

namespace WPFApp.Views
{
    class AdresToevoegenViewModel : FilterDialogs , IPaginaViewModel
    {
        public string Naam => "Adres Toevoegen";
        public ICommuniceer _communicatieKanaal;
        public Action<object> StuurSnackbar { get; init; }

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


        private bool _controleerVeldenVoldaanVoorToevoegen()
        {

            bool voldaan = !(string.IsNullOrEmpty(Straatnaam) || Straatnaam.Any(char.IsDigit) || Straatnaam.Length > 150)
                          && !(string.IsNullOrEmpty(Huisnummer) || !(Huisnummer.Any(char.IsDigit)) || Huisnummer.Length > 50)
                          && !(string.IsNullOrEmpty(Postcode) || !(Postcode.Any(char.IsDigit)) || Postcode.Length < 4 || Postcode.Length > 49)
                          && !(string.IsNullOrEmpty(Plaatsnaam) || Plaatsnaam.Any(char.IsDigit) || Plaatsnaam.Length > 150)
                          && !(string.IsNullOrEmpty(Provincie) || Provincie.Any(char.IsDigit) || Provincie.Length > 150)
                          && !(string.IsNullOrEmpty(Land) || Land.Any(char.IsDigit) || Land.Length > 100);
                       
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
                {
                    AdresRequestDTO adres = new(null, Straatnaam, Huisnummer, Postcode, Plaatsnaam, Provincie, Land);

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

    }
}
