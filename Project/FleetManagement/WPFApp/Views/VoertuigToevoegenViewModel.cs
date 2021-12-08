﻿using System;
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
    class VoertuigToevoegenViewModel : FilterDialogs, IPaginaViewModel
    {
        public string Naam => "Voertuig toevoegen";
        private ICommuniceer _communicatieKanaal;
        public Action<object> StuurSnackbar { get; init; }
        public ObservableCollection<BestuurderResponseDTO> Bestuurders { get; set; } = new();

        public int Id { get; set; }
        public string Merk { get; set; } = "";
        public string Model { get; set; } = "";
        public string Nummerplaat { get; set; } = "";
        public string Brandstof { get; set; } = "";
        public string Voertuigsoort { get; set; } = "";
        public string Kleur { get; set; } = "";
        public int AantalDeuren { get; set; }
        public BestuurderRequestDTO GeselecteerdBestuurder { get; set; } = null;
        public string Chassisnummer { get; set; }
        public BestuurderResponseDTO HighlitedBestuurder { get; set; } = null;
        public VoertuigToevoegenViewModel(ICommuniceer communicatieKanaal, Action<object> stuurSnackbar)
        {
            _communicatieKanaal = communicatieKanaal;
            StuurSnackbar = stuurSnackbar;

            PropertyChanged += FilterDialogs_PropertyChangedHandler;


        }
        private void _startupRoutine()
        {
            try
            {
                _resetBestuurderFilters();
            }
            catch (Exception e)
            {
                StuurSnackbar(e);
            }
        }

        private void FilterDialogs_PropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case string s when s.StartsWith("BestuurderFilter"):
                    _zoekBestuurderMetFilters();
                    break;
                default:
                    break;
            }
        }

        private void _zoekBestuurderMetFilters()
        {
            List<string> zoekfilters = new();
            List<object> zoektermen = new();
            List<PropertyInfo> p = this.GetType().GetProperties().Where(x => x.Name.StartsWith("BestuurderFilter")).ToList();
            foreach (PropertyInfo prop in p)
            {

               
                if (prop.GetValue(this).ToString().Length > 0)
                {
                    zoekfilters.Add(prop.Name.Replace("BestuurderFilter", ""));
                    zoektermen.Add(prop.GetValue(this));
                }
            }
            if (zoekfilters.Count > 0)
            {
                Bestuurders = new ObservableCollection<BestuurderResponseDTO>(_communicatieKanaal.ZoekBestuurders(zoekfilters, zoektermen, likeWildcard: true));
            }
        }
        private bool _controleerVeldenVoldaanVoorToevoegen()
        {

            bool voldaan = Id.ToString().Length > 0
                           && Merk.Length > 0
                          && Model.Length > 0
                          && Brandstof.Length > 0
                          && Nummerplaat.Length > 0
                          && Voertuigsoort.Length > 0
                          && Kleur.Length > 0
                          && AantalDeuren.ToString().Length > 0;               

            if (!voldaan)
            {
                StuurSnackbar("Voertuig voldoet niet aan vereisten.\nGelieve de velden in te vullen.");
            }

            return voldaan;
        }
        private void _resetBestuurderFilters()
        {
            Bestuurders  = new ObservableCollection<BestuurderResponseDTO>(_communicatieKanaal.GeefBestuurders());
        }

        private void _selecteerHighlightedBestuurder()
        {
            try
            {
                if (HighlitedBestuurder is null)
                {
                    throw new Exception("Je hebt geen bestuurder geselecteerd.");
                }

                GeselecteerdBestuurder = DTONaarDTO.ResponseNaarRequest<BestuurderRequestDTO>(HighlitedBestuurder);
            }
            catch (Exception e)
            {
                StuurSnackbar(e.Message);
            }
        }
        private void _voegVoertuigToe()
        {
            if (_controleerVeldenVoldaanVoorToevoegen())
            {
                try
                {
                    VoertuigRequestDTO voertuig = new(null, Merk, Model, Nummerplaat, Brandstof, Voertuigsoort, Kleur, AantalDeuren, GeselecteerdBestuurder, Chassisnummer);
                    int id = _communicatieKanaal.VoegVoertuigToe(voertuig);
                    StuurSnackbar($"Succesvol toegevoegd met id {id}");

                    _resetBestuurderFilters();

                    Merk = "";
                    Model = "";
                    Nummerplaat = "";
                    Brandstof = "";
                    Voertuigsoort = "";
                    Kleur = "";
                    Chassisnummer = "";
                    AantalDeuren = 0;
                    GeselecteerdBestuurder = null;
                    

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
        public ICommand SelecteerHighlitedBestuurder
        {
            get
            {
                return new RelayCommand(
                    p => _selecteerHighlightedBestuurder(),
                    p => p is not null
                );
            }
        }
        public ICommand ResetBestuurderFilters
        {
            get
            {
                return new RelayCommand(
                    p => _resetBestuurderFilters(),
                    p => p is not null
                );
            }
        }
        public ICommand ResetGeselecteerdeBestuurder
        {
            get
            {
                return new RelayCommand(
                    p => { GeselecteerdBestuurder = null; },
                    p => p is not null
                );
            }
        }
        public ICommand VoegVoertuigToe
        {
            get
            {
                return new RelayCommand(
                    p => _voegVoertuigToe(),
                    p => p is not null
                );
            }
        }
    }
}
