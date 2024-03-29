﻿using System;
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
  internal sealed  class AdresWijzigenViewModel : AdresToevoegenViewModel, IPaginaViewModel, IWijzigViewModel {

        public string Naam { get; set; } = "Adres Wijzigen";
    
        
        public AdresResponseDTO IngeladenAdresResponse { get; set; } = null;
        public AdresRequestDTO IngeladenAdresRequest { get; set; } = null;
        public AdresWijzigenViewModel(ICommuniceer communicatieKanaal,Action<object> stuurSnackbar) : base(communicatieKanaal, stuurSnackbar) { }

        public void BereidModelVoor(IResponseDTO responseDTO, bool isReset = false)
        {
            AdresResponseDTO teBehandelenAdres = responseDTO as AdresResponseDTO;

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
                    p => BereidModelVoor(this.IngeladenAdresResponse, true),
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

