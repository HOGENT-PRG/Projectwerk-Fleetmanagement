using BusinessLaag;
using BusinessLaag.Interfaces;
using BusinessLaag.Managers;
using BusinessLaag.Model;
using System.Collections.Generic;
using Xunit;

// Werd niet uitgewerkt, zie hoofdstuk Unit testing in documentatie
namespace xUnitTesting.Managers
{
    // Collection tag is belangrijk!
    // Zonder deze tag worden de tests tegelijk gerunt,
    // dit willen we niet aangezien we met een databank werken.
    [Collection("DisableParallelTests")]
    public class VoertuigManagerTests
    {
        VoertuigManager _voertuigManager = Gemeenschappelijk._testFleetManager.VoertuigManager;
        TestDatabankConfigureerder _beheerDatabank = Gemeenschappelijk.TestDatabankConfigureerder;

        // Zet hier wat uitgevoerd moet worden voor het starten van de tests

        

        //---------------------------------------------------------
    }
}
