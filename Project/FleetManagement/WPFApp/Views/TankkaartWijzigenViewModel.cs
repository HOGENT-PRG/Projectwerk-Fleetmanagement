using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFApp.Interfaces;
using WPFApp.Model.Mappers;
using WPFApp.Model.Request;
using WPFApp.Model.Response;
using WPFApp.Views.MVVM;

namespace WPFApp.Views {
	internal sealed class TankkaartWijzigenViewModel : TankkaartToevoegenViewModel, IWijzigViewModel {
		#pragma warning disable CS0108
		public string Naam { get; set; } = "Tankkaart wijzigen";
		#pragma warning restore CS0108

		public TankkaartResponseDTO IngeladenTankkaartResponse { get; set; } = null;
		public TankkaartRequestDTO IngeladenTankkaartRequest { get; set; } = null;

		public TankkaartWijzigenViewModel(ICommuniceer communicatieKanaal, Action<object> stuurSnackbar) : base(communicatieKanaal, stuurSnackbar) { }

        public void BereidModelVoor(IResponseDTO responseDTO, bool isReset = false) {
            TankkaartResponseDTO teBehandelenTankkaart = responseDTO as TankkaartResponseDTO;

            if (teBehandelenTankkaart is null) {
                StuurSnackbar("Kon de tankkaart niet inladen aangezien deze null is.");
            } else {

                foreach(string brandstof in teBehandelenTankkaart.GeldigVoorBrandstoffen) {
					int idx = -1;
					if (int.TryParse(brandstof, out idx)) {
                        GekozenBrandstoffen.Add(TankkaartBrandstoffen[idx]);
					} else {
                        StuurSnackbar($"Kon brandstof {brandstof} niet bepalen. Data mismatch en inconsistentie ten opzichte van opgeslagen tankkaart aanwezig, nazicht vereist.");
					}
				}

                if (teBehandelenTankkaart.Bestuurder is not null) {
                    GeselecteerdBestuurder = DTONaarDTO.ResponseNaarRequest<BestuurderRequestDTO>(teBehandelenTankkaart.Bestuurder);
                }
                Kaartnummer = teBehandelenTankkaart.Kaartnummer;
                Vervaldatum = teBehandelenTankkaart.Vervaldatum;
                Pincode = teBehandelenTankkaart.Pincode;

                IngeladenTankkaartResponse = teBehandelenTankkaart;
                IngeladenTankkaartRequest = DTONaarDTO.ResponseNaarRequest<TankkaartRequestDTO>(teBehandelenTankkaart);

                Naam = $"Tankkaart {teBehandelenTankkaart.Id} wijzigen";
                if (isReset) {
                    StuurSnackbar("Weergegeven tankkaart werd lokaal hersteld naar de oorspronkelijke staat.");
                }
            }
        }


        private bool _controleerVeldenVoldaanVoorWijzigen() {
            bool voldaan = IngeladenTankkaartRequest is not null
                            && IngeladenTankkaartResponse is not null
                            && !(Kaartnummer.Length < 5 || Kaartnummer.Length > 50)
                            && !(Pincode.Length is not 4 || Pincode.ToCharArray().Any(c => !Char.IsDigit(c)));

            if (!voldaan) {
                StuurSnackbar("Tankkaart voldoet niet aan de vereisten.\nGelieve de velden in te vullen.");
            }

            return voldaan;
        }

        private void _wijzigTankkaart() {
            if (_controleerVeldenVoldaanVoorWijzigen()) {
                try {
                    TankkaartRequestDTO t = new(IngeladenTankkaartResponse.Id, Kaartnummer, Vervaldatum, Pincode, GekozenBrandstoffen.ToList(), GeselecteerdBestuurder);

                    _communicatieKanaal.UpdateTankkaart(t);
                    StuurSnackbar($"Tankkaart met id {t.Id} werd succesvol gewijzigd.");
                } catch (Exception e) {
                    StuurSnackbar(e);
                }
            }
        }


        public ICommand BevestigWijzigTankkaart {
            get {
                return new RelayCommand(
                    p => _wijzigTankkaart(),
                    p => p is not null
                );
            }
        }

        public ICommand ResetNaarOrigineel {
            get {
                return new RelayCommand(
                    p => BereidModelVoor(this.IngeladenTankkaartResponse, true),
                    p => IngeladenTankkaartResponse is not null
                );
            }
        }

        // Normaal kan dit niet aangeroepen worden vanuit TankkaartWijzigen, als redundancy overriden we toch het overgeerfde command.
        #pragma warning disable CS0108
        public ICommand VoegTankkaartToe {
            get {
                return new RelayCommand(
                    p => StuurSnackbar(new NotImplementedException("Tankkaart toevoegen is niet toegelaten vanuit deze context.")),
                    p => p is not null
                );
            }
        }
        #pragma warning restore CS0108



    }
}
