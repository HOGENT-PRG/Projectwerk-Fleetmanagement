using BusinessLaag;
using BusinessLaag.Interfaces;
using BusinessLaag.Managers;
using Xunit;

namespace xUnitTesting.Managers
{
    // Collection tag is belangrijk!
    // Zonder deze tag worden de tests tegelijk gerunt,
    // dit willen we niet aangezien we met een databank werken.
    [Collection("DisableParallelTests")]
    public class BestuurderManagerTests
    {
        BestuurderManager _bestuurderManager = Gemeenschappelijk._testFleetManager.BestuurderManager;
        TestDatabankConfigureerder _beheerDatabank = Gemeenschappelijk.TestDatabankConfigureerder;

        // Zet hieronder wat uitgevoerd moet worden voor het starten van de tests
        

        // Zet hieronder je unit tests en eventuele interacties met db (naar een vorige state bijvoorbeeld)

        
        
    }
}
