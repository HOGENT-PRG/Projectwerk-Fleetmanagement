using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLaag.Interfaces;
using BusinessLaag.Model;
using BusinessLaag.Exceptions;

namespace BusinessLaag.Managers
{
    public class TankkaartManager : ITankkaartManager
    {
        private static FleetManager _fleetManager;
        private ITankkaartOpslag _opslag;

        public TankkaartManager(FleetManager fleetmanager, ITankkaartOpslag repository)
        {
            _fleetManager = fleetmanager;
            _opslag = repository;
        }

        public Tankkaart geefTankkaartDetail(int id)
        {

            KeyValuePair<int?, Tankkaart> opslagResultaat = _opslag.geefTankkaartDetail(id);
                if (opslagResultaat.Value is null) { return null; }

                Tankkaart tankkaartresultaat = opslagResultaat.Value;
                if (opslagResultaat.Key is not null)
                {
                    Bestuurder bestuurder = _fleetManager.BestuurderManager.GeefBestuurderZonderRelaties((int)opslagResultaat.Key) ?? throw new ("Voertuig bevat een referentie naar een bestuurder, deze opvragen was echter niet succesvol.");
                    tankkaartresultaat.zetBestuurder(bestuurder);
                }

                return tankkaartresultaat;
            }

        public IEnumerable<Tankkaart> geefTankkaarten()
        {
            List<Tankkaart> tankkaartRes = new();
            List<KeyValuePair<int?, Tankkaart>> opslagResultaat = _opslag.geefTankkaarten();

            foreach (KeyValuePair<int?, Tankkaart> tankkaart in opslagResultaat)
            {
                Tankkaart geselecteerdTankkaart = tankkaart.Value;
                if (tankkaart.Key is not null)
                {
                    Bestuurder bestuurder = _fleetManager.BestuurderManager.GeefBestuurderZonderRelaties((int)tankkaart.Key) ?? throw new VoertuigManagerException("Tankkaart bevat een referentie naar een bestuurder, deze opvragen was echter niet succesvol.");
                    geselecteerdTankkaart.zetBestuurder(bestuurder);
                }
                opslagResultaat.Add(geselecteerdTankkaart);
            }

            return opslagResultaat;
        }
        private Tankkaart _zoekVoertuigMetKolomnaam(string kolomnaam, string waarde)
        {
            Tankkaart t = (Tankkaart)_opslag.zoekTankkaarten(kolomnaam, waarde);
            if (t?.Id is not null)
            {
                Bestuurder bestuurder = _fleetManager.BestuurderManager.GeefBestuurderZonderRelaties((int)t.Id);
                if (bestuurder is not null) { t.zetBestuurder(bestuurder); }
            }

            return t;
        }
        public Tankkaart ZoekTankkaartMetKaartnummer(string kaartnummer)
        {
            return _zoekVoertuigMetKolomnaam("Chasisnummer", kaartnummer);
        }

        public void updateTankkaart(Tankkaart tankkaart)
        {
            if (tankkaart?.Id is null) { throw new TankkaartException("Kan tankkaart niet updaten zonder voertuig of id. Er werden geen wijzigingen aangebracht."); }
            Tankkaart GemuteerdBestaandTankkaart = this.geefTankkaartDetail((int)tankkaart.Id);

            // Het BestaandVoertuig wordt na elke controle aangepast en op het einde gebruikt om de update functie van
            // VoertuigOpslag te callen.

            if (GemuteerdBestaandTankkaart is null) { throw new VoertuigManagerException("Kan geen tankkaart vinden voor opgegeven id. Er werden geen wijzigingen aangebracht."); }
            if (GemuteerdBestaandTankkaart.Kaartnummer != tankkaart.Kaartnummer)
            {
                Tankkaart KaartnummerCheck = this.ZoekTankkaartMetKaartnummer(tankkaart.Kaartnummer);
                if (KaartnummerCheck is not null)
                {
                    if (KaartnummerCheck.Id != tankkaart.Id)
                    {
                        throw new VoertuigManagerException("Er bestaat reeds een tankkaart met dit kaartnummer. Er werden geen wijzigingen aangebracht.");
                    }
                }

                GemuteerdBestaandTankkaart.zetKaartnummer(tankkaart.Kaartnummer);
            }
           
        }

            public void verwijderTankkaart(Tankkaart tankkaart)
            {
            if (tankkaart?.Id is null) { throw new VoertuigManagerException("Het opgegeven tankkaart of zijn id is null."); }
            Tankkaart volledigTankkaart = this.geefTankkaartDetail((int)tankkaart.Id);

            try
            {
                if (volledigTankkaart.Bestuurder is not null)
                {
                    Bestuurder b = _fleetManager.BestuurderManager.GeefBestuurderDetail((int)volledigTankkaart.Bestuurder.Id);
                    b.zetVoertuig(null);
                    _fleetManager.BestuurderManager.UpdateBestuurder(b);
                }

                _opslag.verwijderTankkaart(tankkaart);

            }
            catch (BestuurderManagerException e)
            {
                throw new VoertuigManagerException($"De verwijzing naar het tankkaart bij de bestuurder kon niet verwijderd worden. Er is niks gewijzigd. {e.GetType().Name}", e);
            }
            catch (VoertuigOpslagException e)
            {
                throw new VoertuigManagerException("De tankkaart kon niet verwijderd worden, een eventuele relatie tussen een bestuurder en het tankkaart werd verwijderd.", e);
            }
            catch (Exception e)
            {
                throw new VoertuigManagerException($"Er is een onverwachte fout opgetreden: {e.GetType().Name}", e);
            }
        }

        // nog een functie brandstof toevoegen/verwijderen, dmv tussentabel <-------------
        public Tankkaart GeefTankkaartZonderRelatie(int id)
        {
            KeyValuePair<int?, Voertuig> opslagResultaat = _opslag.geefTankkaartDetail(id);
            return opslagResultaat.Value;
        }
        public void voegTankkaartToe(Tankkaart tankkaart)
            {
            if (ZoekTankkaartMetKaartnummer(tankkaart.Kaartnummer) is not null) { throw new VoertuigManagerException("kaartnummer is reeds toegewezen aan een tankkaart."); }
     

            Tankkaart opgeslagenTankkaart = null;

            try
            {
                opgeslagenTankkaart = this.GeefTankkaartZonderRelatie(_opslag.voegTankkaartToe(tankkaart));

                if (tankkaart.Bestuurder is not null)
                {
                    if (tankkaart.Bestuurder.Id is null)
                    {
                        throw new VoertuigManagerException("Tankkaart werd toegevoegd aan de databank, echter kon de relatie met de bestuurder niet gelegd worden aangezien de bestuurder zijn id niet meegegeven werd.");
                    }
                    else
                    {
                        Bestuurder b = _fleetManager.BestuurderManager.GeefBestuurderDetail((int)tankkaart.Bestuurder.Id);
                        b.zetTankkaart(opgeslagenTankkaart); // indien al een voertuig ingesteld staat zal het overschreven worden
                        _fleetManager.BestuurderManager.UpdateBestuurder(b);
                    }
                }

            }
            catch (VoertuigOpslagException e)
            {
                throw new VoertuigManagerException("De tankkaart kon niet opgeslaan worden, er zijn geen wijzigingen aangebracht.", e);
            }
            catch (BestuurderManagerException e)
            {
                throw new VoertuigManagerException($"De tankkaart werd opgeslagen (id={opgeslagenTankkaart?.Id}) maar de relatie met de bestuurder kon niet tot stand gebracht worden.", e);
            }
            catch (Exception e)
            {
                throw new VoertuigManagerException("Er is een onverwachte fout opgetreden.", e);
            }
        }

        public IEnumerable<Tankkaart> zoekTankkaarten()
        {
            throw new NotImplementedException();
        }

        // functie veranderblokkeringsstatus
    }
    }
