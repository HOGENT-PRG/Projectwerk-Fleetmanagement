using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPFApp.Helpers;
using WPFApp.Interfaces;
using WPFApp.Model.Mappers;
using WPFApp.Model.Request;
using WPFApp.Model.Response;
using WPFApp.Views.MVVM;
namespace WPFApp.Views
{
    internal sealed class VoertuigWijzigenViewModel : VoertuigToevoegenViewModel
    {
#pragma warning disable CS0108
        public string Naam { get; set; } = "Voertuig wijzigen";
#pragma warning restore CS0108

        // Response wordt gebruikt bij reset, request bij vergelijken of er wijzigingen zijn
        public VoertuigResponseDTO IngeladenVoertuigResponse { get; set; } = null;
        public VoertuigRequestDTO IngeladenVoertuigRequest { get; set; } = null;
        public ObservableCollection<string?> GekozenMerk { get; private set; } = new();
        //public ObservableCollection<string> MeegegevenSoort { get; private set; } = new();
       public ObservableCollection<string?> MeegegevenBrandstof { get; private set; } = new();

        public VoertuigWijzigenViewModel(ICommuniceer communicatieKanaal, Action<object> stuurSnackbar) : base(communicatieKanaal, stuurSnackbar) { }

        public void BereidModelVoorMetVoertuig(VoertuigResponseDTO teBehandelenVoertuig, bool isReset = false)
        {
            if (teBehandelenVoertuig is null)
            {
                StuurSnackbar("Kon het voertuig niet inladen aangezien deze null is.");
            }
            else
            {
            
                Kleur = teBehandelenVoertuig.Kleur;
                Merk = teBehandelenVoertuig.Merk;
                Model = teBehandelenVoertuig.Model;
                Nummerplaat = teBehandelenVoertuig.Nummerplaat;
                Voertuigsoort =teBehandelenVoertuig.Voertuigsoort;
                Brandstof = teBehandelenVoertuig.Brandstof;
                Chassisnummer = teBehandelenVoertuig.Chassisnummer;
                AantalDeuren = (int)teBehandelenVoertuig.AantalDeuren;
                int idx = 0;
               
                if (int.TryParse(teBehandelenVoertuig.Voertuigsoort, out idx))
                {
                    Voertuigsoort = VoertuigSoorten[idx];
                   
                   
                }
                else
                {
                    Voertuigsoort = "";
                }
                if (int.TryParse(teBehandelenVoertuig.Merk, out idx))
                {
                    Merk = VoertuigMerken[idx];
                  //  GekozenMerk.Add(VoertuigMerken[idx]);
                }
                else
                {
                    Merk = "";
                }
                if (int.TryParse(teBehandelenVoertuig.Brandstof, out idx))
                {
                    Brandstof = VoertuigBrandstoffen[idx];
                 //   MeegegevenBrandstof.Add(VoertuigBrandstoffen[idx]);

                }
                else
                {
                    Brandstof = "";
                }

              
            }


            GeselecteerdBestuurder = DTONaarDTO.ResponseNaarRequest<BestuurderRequestDTO>(teBehandelenVoertuig.Bestuurder);


            IngeladenVoertuigResponse = teBehandelenVoertuig;

            IngeladenVoertuigRequest = DTONaarDTO.ResponseNaarRequest<VoertuigRequestDTO>(teBehandelenVoertuig);
            Naam = $"Voertuig {teBehandelenVoertuig.Id} wijzigen";
            if (isReset)
            {
                StuurSnackbar("Weergegeven voertuig werd lokaal hersteld naar de oorspronkelijke staat.");
            }
        }
    
        // Hetzelfde als bij toevoegen, met extra null check voor ingeladenbestuurder en
        // duidelijke naam
        private bool _controleerVeldenVoldaanVoorWijzigen()
        {

            bool voldaan = (!string.IsNullOrEmpty(Nummerplaat) && !string.IsNullOrWhiteSpace(Nummerplaat) && Nummerplaat.Length < 20
             && !string.IsNullOrEmpty(Model) && !string.IsNullOrWhiteSpace(Model) && Model.Length < 20
            && !string.IsNullOrEmpty(Chassisnummer) && !string.IsNullOrWhiteSpace(Chassisnummer) && Chassisnummer.Length == 17)
            && (!string.IsNullOrEmpty(Kleur) && !string.IsNullOrWhiteSpace(Kleur) && Kleur.Length < 40)
            && (AantalDeuren > 0 && AantalDeuren < 21);


            if (!voldaan)
            {
                StuurSnackbar("Voertuig voldoet niet aan vereisten.\nGelieve de velden in te vullen.");
            }

            return voldaan;
        }
        private void _wijzigVoertuig()
        {
            if (_controleerVeldenVoldaanVoorWijzigen())
            {
             
                try
                {
                    VoertuigRequestDTO v = new VoertuigRequestDTO(IngeladenVoertuigResponse.Id, Merk, Model, Nummerplaat, Brandstof, Voertuigsoort, Kleur, AantalDeuren, Chassisnummer, GeselecteerdBestuurder);

                    _communicatieKanaal.UpdateVoertuig(v);
                    StuurSnackbar($"Het voertuig  met id {v.Id} werd succesvol gewijzigd.");
                }
                catch (Exception e)
                {
                    StuurSnackbar(e);
                }
            }
        }

        // todo commandos in xaml
        public ICommand BevestigWijzigVoertuig
        {
            get
            {
                return new RelayCommand(
                    p => _wijzigVoertuig(),
                    p => p is not null
                );
            }
        }

        public ICommand ResetNaarOrigineel
        {
            get
            {
                return new RelayCommand(
                    p => BereidModelVoorMetVoertuig(this.IngeladenVoertuigResponse, true),
                    p => IngeladenVoertuigResponse is not null
                );
            }
        }

        // Normaal kan dit niet aangeroepen worden vanuit BestuurderWijzigen, als redundancy overriden we toch het overgeerfde command.
#pragma warning disable CS0108
        public ICommand VoegVoertuigToe
        {
            get
            {
                return new RelayCommand(
                    p => StuurSnackbar(new NotImplementedException("Voertuig toevoegen is niet toegelaten vanuit deze context.")),
                    p => p is not null
                );
            }
        }




    }
}
