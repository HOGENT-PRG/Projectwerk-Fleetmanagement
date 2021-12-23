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
  internal sealed  class AdresWijzigenViewModel : AdresToevoegenViewModel, IPaginaViewModel
    {

        public string Naam { get; set; } = "Adres Wijzigen";
    
        
        public AdresResponseDTO IngeladenAdresResponse { get; set; } = null;
        public AdresRequestDTO IngeladenAdresRequest { get; set; } = null;
        public AdresWijzigenViewModel(ICommuniceer communicatieKanaal,Action<object> stuurSncakbar) : base(communicatieKanaal, stuurSncakbar)
        {

        }
       
        public void BereidModelVoorAdres(AdresResponseDTO teBehandelenAdres,bool isReset = false)
        {
            if(teBehandelenAdres is null)
            {
                StuurSnackbar("Kon de adres niet inladen aangezien deze null is.");
            }
            else
            {
                
                Straatnaam = teBehandelenAdres.Straatnaam;
                Huisnummer = teBehandelenAdres.Huisnummer;
                Postcode = teBehandelenAdres.Postcode;
                Plaatsnaam = teBehandelenAdres.Plaatsnaam;
                Provincie = teBehandelenAdres.Provincie;
                Land = teBehandelenAdres.Land;
                IngeladenAdresResponse = teBehandelenAdres;
                IngeladenAdresRequest = DTONaarDTO.ResponseNaarRequest<AdresRequestDTO>(teBehandelenAdres);
                Naam = $"Adres {teBehandelenAdres.Id} wijzigen";
                if (isReset)
                {
                    StuurSnackbar("Weergegeven adres werd lokaal hersteld naar de oorspronkelikke staat.");
                }
            }
        }
        private bool _controleerVeldenVoldaanVoorWijzigen()
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
        private void _wijzigAdres()
        {
            if (_controleerVeldenVoldaanVoorWijzigen())
            {
                try
                {
                    AdresRequestDTO a = new(IngeladenAdresResponse.Id, Straatnaam, Huisnummer, Postcode, Plaatsnaam, Provincie, Land);
                    _communicatieKanaal.UpdateAdres(a);
                    StuurSnackbar($"Adres met id {a.Id} werd succesvol gewijzigd");
                  
                }catch(Exception ex)
                {
                    StuurSnackbar(ex);
                }
            }
        }
        public ICommand BevestigWijzigAdres
        {
            get
            {
                return new RelayCommand(
                    p => _wijzigAdres(),
                    p => p is not null
                );
            }
        }
        public ICommand ResetNaarOrigineel
        {
            get
            {
                return new RelayCommand(
                    p => BereidModelVoorAdres(this.IngeladenAdresResponse, true),
                    p => IngeladenAdresResponse is not null
                );
            }
        }
#pragma warning disable CS0108 
        public ICommand VoegAdresToe

        {
            get
            {
                return new RelayCommand(
                    p => StuurSnackbar(new NotImplementedException("Adres toevoegen is niet toegelaten vanuit deze context.")),
                    p => p is not null
                );
            }
        }
      
    }
}

