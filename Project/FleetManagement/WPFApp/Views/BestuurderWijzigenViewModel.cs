﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFApp.Helpers;
using WPFApp.Interfaces;
using WPFApp.Model.Mappers;
using WPFApp.Model.Request;
using WPFApp.Model.Response;
using WPFApp.Views.MVVM;

namespace WPFApp.Views {
	internal sealed class BestuurderWijzigenViewModel : BestuurderToevoegenViewModel {
        #pragma warning disable CS0108
		public string Naam { get; set; } = "Bestuurder wijzigen";
        #pragma warning restore CS0108

        // Response wordt gebruikt bij reset, request bij vergelijken of er wijzigingen zijn
        public BestuurderResponseDTO IngeladenBestuurderResponse { get; set; } = null;
		public BestuurderRequestDTO IngeladenBestuurderRequest { get; set; } = null;

		public BestuurderWijzigenViewModel(ICommuniceer communicatieKanaal, Action<object> stuurSnackbar) : base(communicatieKanaal, stuurSnackbar) { }

        // Wordt gebruikt om te vergelijken of er wijzigingen aangebracht werden
        private static (Formatting, JsonSerializerSettings) FJSS {
            get {
                return (Formatting.None, new JsonSerializerSettings {
                    ObjectCreationHandling = ObjectCreationHandling.Replace,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            }
            set {  _ = value; }
        }

        public void BereidModelVoorMetBestuurder(BestuurderResponseDTO teBehandelenBestuurder, bool isReset = false) {
            if (teBehandelenBestuurder is null) {
                StuurSnackbar("Kon de bestuurder niet inladen aangezien deze null is.");
            } else {
                Achternaam = teBehandelenBestuurder.Naam;
                Voornaam = teBehandelenBestuurder.Voornaam;
                GeboorteDatum = teBehandelenBestuurder.GeboorteDatum;
                RijksRegisterNummer = teBehandelenBestuurder.Rijksregisternummer;

                int idx = -1;
                if(int.TryParse(teBehandelenBestuurder.Rijbewijssoort, out idx)) {
                    RijbewijsSoort = RijbewijsOpties[idx];
				} else {
                    RijbewijsSoort = "";
				}

                GeselecteerdAdres = DTONaarDTO.ResponseNaarRequest<AdresRequestDTO>(teBehandelenBestuurder.Adres);
                GeselecteerdeTankkaart = DTONaarDTO.ResponseNaarRequest<TankkaartRequestDTO>(teBehandelenBestuurder.Tankkaart);
                GeselecteerdeVoertuig = DTONaarDTO.ResponseNaarRequest<VoertuigRequestDTO>(teBehandelenBestuurder.Voertuig);

                IngeladenBestuurderResponse = teBehandelenBestuurder;
                IngeladenBestuurderRequest = DTONaarDTO.ResponseNaarRequest<BestuurderRequestDTO>(teBehandelenBestuurder);

                Naam = $"Bestuurder {teBehandelenBestuurder.Id} wijzigen";
                if (isReset) {
                    StuurSnackbar("Weergegeven bestuurder werd hersteld naar de huidige opgeslagen staat.");
                }
            }
        }

        // Hetzelfde als bij toevoegen, met extra null check voor ingeladenbestuurder en
        // duidelijke naam
        private bool _controleerVeldenVoldaanVoorWijzigen() {
            // Adres, Tankkaart, Voertuig mag null zijn
            bool voldaan = IngeladenBestuurderRequest is not null
                           && IngeladenBestuurderResponse is not null
                           && !(string.IsNullOrEmpty(Achternaam)
                             || Achternaam.Length < 2
                             || Achternaam.Length > 70
                             || Achternaam.Any(c => Char.IsDigit(c)))
                           && !(string.IsNullOrEmpty(Voornaam)
                                || Voornaam.Length < 2
                                || Voornaam.Length > 70
                                || Voornaam.Any(c => Char.IsDigit(c)))
                           && (DateTime.Compare(DateTime.Now.AddYears(-120), GeboorteDatum) < 0)
                                && (DateTime.Compare(DateTime.Now, GeboorteDatum) > 0)
                           && RijksRegisterNummer.Length > 0 // validatie bij Self_ prop event handler
                           && RijbewijsSoort.Length > 0; // validatie dmv dropdown

            if (!voldaan) {
                StuurSnackbar("Bestuurder voldoet niet aan de vereisten.\nGelieve de velden in te vullen.");
            }

            return voldaan;
        }

        private void _wijzigBestuurder() {
			if (_controleerVeldenVoldaanVoorWijzigen()) {
                try {
                    BestuurderRequestDTO b = new(IngeladenBestuurderResponse.Id, Achternaam, Voornaam, RijksRegisterNummer, RijbewijsSoort, GeboorteDatum, GeselecteerdAdres, GeselecteerdeVoertuig, GeselecteerdeTankkaart);

                    if(JsonConvert.SerializeObject(b, FJSS.Item1, FJSS.Item2)
                        == JsonConvert.SerializeObject(IngeladenBestuurderRequest, FJSS.Item1, FJSS.Item2)) {
                        throw new WarningException("Er zijn geen wijzigingen aangebracht, aldus kan er niet geupdate worden.");
					}

                    _communicatieKanaal.UpdateBestuurder(b);
                    StuurSnackbar($"De bestuurder met id {b.Id} werd succesvol gewijzigd.");
				} catch (Exception e) {
                    StuurSnackbar(e);
				}
			}
		}

        // todo commandos in xaml
        public ICommand BevestigWijzigBestuurder {
            get {
                return new RelayCommand(
                    p => _wijzigBestuurder(),
                    p => p is not null
                );
            }
        }

        public ICommand ResetNaarOrigineel {
            get {
                return new RelayCommand(
                    p => BereidModelVoorMetBestuurder(this.IngeladenBestuurderResponse, true),
                    p => IngeladenBestuurderResponse is not null
                );
            }
        }

        // Normaal kan dit niet aangeroepen worden vanuit BestuurderWijzigen, als redundancy overriden we toch het overgeerfde command.
        #pragma warning disable CS0108
        public ICommand VoegBestuurderToe {
			get {
                return new RelayCommand(
                    p => StuurSnackbar(new NotImplementedException("Bestuurder toevoegen is niet toegelaten vanuit deze context.")),
                    p => p is not null
                );
            }
        }
        #pragma warning restore CS0108



    }
}
