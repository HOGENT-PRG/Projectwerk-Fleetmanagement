using System;
using System.Collections.Generic;
using BusinessLaag.Exceptions;
using BusinessLaag.Interfaces;
using BusinessLaag.Model;

namespace BusinessLaag.Managers {
    public class VoertuigManager : IVoertuigManager {
        private static FleetManager _fleetManager;
        private IVoertuigOpslag _opslag;

        public VoertuigManager(FleetManager fleetmanager, IVoertuigOpslag opslag) {
            _fleetManager = fleetmanager;
            _opslag = opslag;
        }

        // ------ create
        public void VoegVoertuigToe(Voertuig voertuig) {
            if (ZoekVoertuigMetChassisnummer(voertuig.Chassisnummer) is not null) { throw new VoertuigManagerException("Chassisnummer is reeds toegewezen aan een voertuig."); }
            if (ZoekVoertuigMetNummerplaat(voertuig.Nummerplaat) is not null) { throw new VoertuigManagerException("Nummerplaat is reeds toegewezen aan een voertuig."); }

            Voertuig opgeslagenVoertuig = null;

            try {
                opgeslagenVoertuig = _opslag.VoegVoertuigToe(voertuig);

                if (voertuig.Bestuurder is not null) {
                    if (voertuig.Bestuurder.Id is null) { throw new VoertuigManagerException("Voertuig werd toegevoegd aan de databank, echter kon de relatie met de bestuurder niet gelegd worden aangezien de bestuurder id niet meegegeven werd."); }

                    Bestuurder b = _fleetManager.BestuurderManager.GeefBestuurderDetail((int)voertuig.Bestuurder.Id);
                    b.zetVoertuig(opgeslagenVoertuig); // indien al een voertuig ingesteld staat zal het overschreven worden
                    _fleetManager.BestuurderManager.UpdateBestuurder(b);
                }

            } catch (VoertuigOpslagException e) {
                throw new VoertuigManagerException("Het voertuig kon niet opgeslaan worden, er zijn geen wijzigingen aangebracht.", e);
            } catch (BestuurderManagerException e) {
                throw new VoertuigManagerException($"Het voertuig werd opgeslagen (id={opgeslagenVoertuig?.Id}) maar de relatie met de bestuurder kon niet tot stand gebracht worden.", e);
            } catch (Exception e) {
                throw new VoertuigManagerException("Er is een onverwachte fout opgetreden.", e);
            }
        }

        // ------ read
        public Voertuig GeefVoertuigZonderRelaties(int id) {
            KeyValuePair<int?, Voertuig> opslagResultaat = _opslag.GeefVoertuigDetail(id);
            return opslagResultaat.Value;
        }

        // Stelt indien mogelijk de bestuurder in, retourneert Voertuig
        public Voertuig GeefVoertuigDetail(int id) {
            KeyValuePair<int?, Voertuig> opslagResultaat = _opslag.GeefVoertuigDetail(id);
            if (opslagResultaat.Value is null) { return null; }

            Voertuig voertuigResultaat = opslagResultaat.Value;
            if (opslagResultaat.Key is not null) {
                Bestuurder bestuurder = _fleetManager.BestuurderManager.GeefBestuurderZonderRelaties((int)opslagResultaat.Key) ?? throw new VoertuigManagerException("Voertuig bevat een referentie naar een bestuurder, deze opvragen was echter niet succesvol.");
                voertuigResultaat.zetBestuurder(bestuurder);
            }

            return voertuigResultaat;
        }

        // Stelt bij elk voertuig indien mogelijk de bestuurder in, retourneert lijst met voertuigen
        public List<Voertuig> GeefVoertuigen() {
            List<Voertuig> voertuigenResultaat = new();
            Dictionary<int?, Voertuig> opslagResultaat = _opslag.GeefVoertuigen();

            foreach (KeyValuePair<int?, Voertuig> voertuig in opslagResultaat) {
                Voertuig geselecteerdVoertuig = voertuig.Value;
                if (voertuig.Key is not null) {
                    Bestuurder bestuurder = _fleetManager.BestuurderManager.GeefBestuurderZonderRelaties((int)voertuig.Key) ?? throw new VoertuigManagerException("Voertuig bevat een referentie naar een bestuurder, deze opvragen was echter niet succesvol.");
                    geselecteerdVoertuig.zetBestuurder(bestuurder);
                }
                voertuigenResultaat.Add(geselecteerdVoertuig);
            }

            return voertuigenResultaat;
        }

        private Voertuig _zoekVoertuigMetKolomnaam(string kolomnaam, string waarde) {
            Voertuig v = _opslag.ZoekVoertuig(kolomnaam, waarde);
            if (v?.Id is not null) {
                Bestuurder bestuurder = _fleetManager.BestuurderManager.GeefBestuurderZonderRelaties((int)v.Id);
                if (bestuurder is not null) { v.zetBestuurder(bestuurder); }
            }

            return v;
        }

        public Voertuig ZoekVoertuigMetChassisnummer(string chassisnummer) {
            return _zoekVoertuigMetKolomnaam("Chasisnummer", chassisnummer);
        }

        public Voertuig ZoekVoertuigMetNummerplaat(string nummerplaat) {
            return _zoekVoertuigMetKolomnaam("Nummerplaat", nummerplaat);
        }

        // ------ update
        public void UpdateVoertuig(Voertuig NieuweVoertuigVersie, bool negeerBestuurder = false) {
            if (NieuweVoertuigVersie.Id is null) { throw new VoertuigManagerException("Kan voertuig niet updaten zonder id. Er werden geen wijzigingen aangebracht."); }
            Voertuig GemuteerdBestaandVoertuig = this.GeefVoertuigDetail((int)NieuweVoertuigVersie.Id);

            // Het BestaandVoertuig wordt na elke controle aangepast en op het einde gebruikt om de update functie van
            // VoertuigOpslag te callen.

            if (GemuteerdBestaandVoertuig is null) { throw new VoertuigManagerException("Kan geen voertuig vinden voor opgegeven id. Er werden geen wijzigingen aangebracht."); }
            if (GemuteerdBestaandVoertuig.Chassisnummer != NieuweVoertuigVersie.Chassisnummer) {
                Voertuig chassisCheckVoertuig = this.ZoekVoertuigMetChassisnummer(NieuweVoertuigVersie.Chassisnummer);
                if (chassisCheckVoertuig is not null) {
                    if (chassisCheckVoertuig.Id != NieuweVoertuigVersie.Id) {
                        throw new VoertuigManagerException("Er bestaat reeds een voertuig met dit chassisnummer. Er werden geen wijzigingen aangebracht.");
                    }
                }

                GemuteerdBestaandVoertuig.zetChassisnummer(NieuweVoertuigVersie.Chassisnummer);
            }

            if (GemuteerdBestaandVoertuig.Nummerplaat != NieuweVoertuigVersie.Nummerplaat) {
                Voertuig nummerplaatCheckVoertuig = this.ZoekVoertuigMetNummerplaat(NieuweVoertuigVersie.Nummerplaat);
                if (nummerplaatCheckVoertuig is not null) {
                    if (nummerplaatCheckVoertuig.Id != NieuweVoertuigVersie.Id) {
                        throw new VoertuigManagerException("Er bestaat reeds een voertuig met deze nummerplaat. Er werden geen wijzigingen aangebracht.");
                    }
                }

                GemuteerdBestaandVoertuig.zetNummerplaat(NieuweVoertuigVersie.Nummerplaat);
            }

            bool bestuurderRelatieAangepast = false;

            // Hier worden calls gemaakt naar de bestuurdermanager om de
            // voertuig <-> bestuurder relatie aan te passen.
            // De tabel Voertuig heeft namelijk geen referentie naar de Bestuurder, maar omgekeerd.
            if ((GemuteerdBestaandVoertuig.Bestuurder?.Id != NieuweVoertuigVersie.Bestuurder?.Id) && !negeerBestuurder) {

                // Aangezien we een nieuwe bestuurder instellen, huidige referentie naar het
                // voertuig bij de bestuurder op null zetten.
                if (GemuteerdBestaandVoertuig.Bestuurder is not null) {
                    try {
                        Bestuurder b = _fleetManager.BestuurderManager.GeefBestuurderDetail((int)GemuteerdBestaandVoertuig.Bestuurder.Id);
                        if (b.Voertuig is not null) {
                            b.zetVoertuig(null);
                            _fleetManager.BestuurderManager.UpdateBestuurder(b);
                        }
                    } catch (BestuurderManagerException e) {
                        throw new VoertuigManagerException("Kon de bestaande bestuurder niet loskoppelen van het huidige voertuig. Er werden geen wijzigingen aangebracht.", e);
                    }
                }

                // Louter illustratief, het bestaande voertuig heeft niet langer een bestuurder door bovenstaande.
                GemuteerdBestaandVoertuig.zetBestuurder(null);

                // Indien de nieuwe bestuurder een bestuurder is, wordt zijn eventuele relatie met een
                // ander voertuig vervangen door dit voertuig
                if (NieuweVoertuigVersie.Bestuurder is not null) {
                    try {
                        Bestuurder b = _fleetManager.BestuurderManager.GeefBestuurderDetail((int)NieuweVoertuigVersie.Bestuurder.Id);

                        b.zetVoertuig(GemuteerdBestaandVoertuig);
                        _fleetManager.BestuurderManager.UpdateBestuurder(b);

                        // Louter illustratief, het bestaande voertuig heeft nu een nieuwe bestuurder
                        GemuteerdBestaandVoertuig.zetBestuurder(NieuweVoertuigVersie.Bestuurder);

                    } catch (BestuurderManagerException e) {
                        throw new VoertuigManagerException("Kon de nieuwe bestuurder niet koppelen aan het voertuig. Een eventuele vooraf aangewezen bestuurder werd wel losgekoppeld van het voertuig.", e);
                    }
                }

                bestuurderRelatieAangepast = true;

            }

            try {
                _opslag.UpdateVoertuig(GemuteerdBestaandVoertuig);
            } catch (Exception e) {
                if (bestuurderRelatieAangepast) {
                    throw new VoertuigManagerException("De bestuurder relatie werd gewijzigd, overige waarden konden echter niet aangepast worden.", e);
                } else { throw new VoertuigManagerException("Kon het voertuig niet updaten, er werd niks gewijzigd.", e); }
            }

        }

        // ------ delete
        public void VerwijderVoertuig(Voertuig voertuig) {
            if (voertuig is null) { throw new VoertuigManagerException("Het opgegeven voertuig is null."); }
            if (voertuig.Id is null) { throw new VoertuigManagerException("Het opgegeven voertuig bevat geen id."); }
            Voertuig volledigVoertuig = this.GeefVoertuigDetail((int)voertuig.Id);

            try {
                if (volledigVoertuig.Bestuurder is not null) {
                    Bestuurder b = _fleetManager.BestuurderManager.GeefBestuurderDetail((int)volledigVoertuig.Bestuurder.Id);
                    b.zetVoertuig(null);
                    _fleetManager.BestuurderManager.UpdateBestuurder(b);
                }

                _opslag.VerwijderVoertuig(voertuig);

            } catch (BestuurderManagerException e) {
                throw new VoertuigManagerException($"De verwijzing naar het voertuig bij de bestuurder kon niet verwijderd worden. Er is niks gewijzigd. {e.GetType().Name}", e);
            } catch (VoertuigOpslagException e) {
                throw new VoertuigManagerException("Het voertuig kon niet verwijderd worden, een eventuele relatie tussen een bestuurder en het voertuig werd verwijderd.", e);
            } catch (Exception e) {
                throw new VoertuigManagerException($"Er is een onverwachte fout opgetreden: {e.GetType().Name}", e);
            }

        }


    }
}
