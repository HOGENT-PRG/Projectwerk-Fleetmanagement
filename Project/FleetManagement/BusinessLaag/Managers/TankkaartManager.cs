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

        public Tankkaart GeefTankkaartDetail(int id)
        {

            KeyValuePair<int?, Tankkaart> opslagResultaat = _opslag.GeefTankkaartDetail(id);
                if (opslagResultaat.Value is null) { return null; }

                Tankkaart tankkaartresultaat = opslagResultaat.Value;
                if (opslagResultaat.Key is not null)
                {
                    Bestuurder bestuurder = _fleetManager.BestuurderManager.GeefBestuurderZonderRelaties((int)opslagResultaat.Key) ?? throw new ("Tankkaart bevat een referentie naar een bestuurder, deze opvragen was echter niet succesvol.");
                    tankkaartresultaat.zetBestuurder(bestuurder);
                }

                return tankkaartresultaat;
            }

        public IEnumerable<Tankkaart> geefTankkaarten()
        {
            List<Tankkaart> tankkaartRes = new();
            List<KeyValuePair<int?, Tankkaart>> opslagResultaat = _opslag.GeefTankkaarten();

            foreach (KeyValuePair<int?, Tankkaart> tankkaart in opslagResultaat)
            {
                Tankkaart geselecteerdTankkaart = tankkaart.Value;
                if (tankkaart.Key is not null)
                {
                    Bestuurder bestuurder = _fleetManager.BestuurderManager.GeefBestuurderZonderRelaties((int)tankkaart.Key) ?? throw new TankkaartOpslagException("Tankkaart bevat een referentie naar een bestuurder, deze opvragen was echter niet succesvol.");
                    geselecteerdTankkaart.zetBestuurder(bestuurder);
                }
                opslagResultaat.Add(tankkaart);
            }

            return (IEnumerable<Tankkaart>)opslagResultaat;
        }
        private Tankkaart _zoekTankkaartMetKolomnaam(string kolomnaam, string waarde)
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
            return _zoekTankkaartMetKolomnaam("Kaartnummer", kaartnummer);
        }

        public void updateTankkaart(Tankkaart tankkaart)
        {
            if (tankkaart?.Id is null) { throw new TankkaartException("Kan tankkaart niet updaten zonder id. Er werden geen wijzigingen aangebracht."); }
            Tankkaart GemuteerdBestaandTankkaart = this.GeefTankkaartDetail((int)tankkaart.Id);



            if (GemuteerdBestaandTankkaart is null) { throw new TankkaartOpslagException("Kan geen tankkaart vinden voor opgegeven id. Er werden geen wijzigingen aangebracht."); }
            if (GemuteerdBestaandTankkaart.Kaartnummer != tankkaart.Kaartnummer)
            {
                Tankkaart KaartnummerCheck = this.ZoekTankkaartMetKaartnummer(tankkaart.Kaartnummer);
                if (KaartnummerCheck is not null)
                {
                    if (KaartnummerCheck.Id != tankkaart.Id)
                    {
                        throw new TankkaartOpslagException("Er bestaat reeds een tankkaart met dit kaartnummer. Er werden geen wijzigingen aangebracht.");
                    }
                }

                GemuteerdBestaandTankkaart.zetKaartnummer(tankkaart.Kaartnummer);
            }
           
        }

            public void verwijderTankkaart(Tankkaart tankkaart)
            {
            if (tankkaart?.Id is null) { throw new TankkaartOpslagException("Het opgegeven tankkaart of zijn id is null."); }
            Tankkaart volledigTankkaart = this.GeefTankkaartDetail((int)tankkaart.Id);

            try
            {
                if (volledigTankkaart.Bestuurder is not null)
                {
                    Bestuurder b = _fleetManager.BestuurderManager.GeefBestuurderDetail((int)volledigTankkaart.Bestuurder.Id);
                    b.zetTankkaart(null);
                    _fleetManager.BestuurderManager.UpdateBestuurder(b);
                }

                _opslag.verwijderTankkaart(tankkaart);

            }
            catch (TankkaartOpslagException e)
            {
                throw new TankkaartException($"De verwijzing naar het tankkaart bij de bestuurder kon niet verwijderd worden. Er is niks gewijzigd. {e.GetType().Name}", e);
            }
            catch (TankkaartException e)
            {
                throw new TankkaartException("De tankkaart kon niet verwijderd worden, een eventuele relatie tussen een bestuurder en het tankkaart werd verwijderd.", e);
            }
            catch (Exception e)
            {
                throw new TankkaartException($"Er is een onverwachte fout opgetreden: {e.GetType().Name}", e);
            }
        }

        // nog een functie brandstof toevoegen/verwijderen, dmv tussentabel <-------------
   
        public void voegTankkaartToe(Tankkaart tankkaart)
            {
            if (ZoekTankkaartMetKaartnummer(tankkaart.Kaartnummer) is not null) { throw new TankkaartOpslagException("kaartnummer is reeds toegewezen aan een tankkaart."); }
     

            Tankkaart opgeslagenTankkaart = null;

            try
            {
                opgeslagenTankkaart = this.GeefTankkaartZonderRelatie(_opslag.voegTankkaartToe(tankkaart.Id));

                if (tankkaart.Bestuurder is not null)
                {
                    if (tankkaart.Bestuurder.Id is null)
                    {
                        throw new TankkaartOpslagException("Tankkaart werd toegevoegd aan de databank, echter kon de relatie met de bestuurder niet gelegd worden aangezien de bestuurder zijn id niet meegegeven werd.");
                    }
                    else
                    {
                        Bestuurder b = _fleetManager.BestuurderManager.GeefBestuurderDetail((int)tankkaart.Bestuurder.Id);
                        b.zetTankkaart(opgeslagenTankkaart); 
                        _fleetManager.BestuurderManager.UpdateBestuurder(b);
                    }
                }

            }
            catch (TankkaartOpslagException e)
            {
                throw new TankkaartException("De tankkaart kon niet opgeslaan worden, er zijn geen wijzigingen aangebracht.", e);
            }
            catch (BestuurderManagerException e)
            {
                throw new TankkaartException($"De tankkaart werd opgeslagen (id={opgeslagenTankkaart?.Id}) maar de relatie met de bestuurder kon niet tot stand gebracht worden.", e);
            }
            catch (Exception e)
            {
                throw new TankkaartException("Er is een onverwachte fout opgetreden.", e);
            }
        }

        private Tankkaart GeefTankkaartZonderRelatie(int v)
        {
            KeyValuePair<int?, Tankkaart> opslagResultaat = _opslag.GeefTankkaartDetail(v);
            return opslagResultaat.Value;
        }

        public IEnumerable<Tankkaart> zoekTankkaarten()
        {
            throw new NotImplementedException();
        }

        // functie veranderblokkeringsstatus
    }
    }
