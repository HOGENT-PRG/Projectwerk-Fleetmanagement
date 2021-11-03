using WPFApp.Interfaces;
using WPFApp.Model;
using WPFApp.Model.Communiceerders;

namespace WPFApp {

    internal class CommunicatieRelay {
        public ICommuniceer CommunicatieKanaal;
        public Zoekmachine Zoekmachine;

        public readonly bool GEBRUIK_API;
        public readonly string API_BASIS_PAD;

        public CommunicatieRelay(bool GebruikApi = false, string ApiBasisPad = "http://localhost:5000") {
            API_BASIS_PAD = ApiBasisPad;
            GEBRUIK_API = GebruikApi;
            Zoekmachine = new Zoekmachine();

            if (GEBRUIK_API) {
                CommunicatieKanaal = new ApiCommuniceerder(API_BASIS_PAD);
            } else {
                CommunicatieKanaal = new BusinessCommuniceerder();
            }

            
        }

    }

}

// In tegenstelling tot de FleetManager die
// - initieert en beschikbaar stelt (managers & databankConfigureerder)
// fungeert de relay als funnel naar de door de relay geselecteerde communiceerder.
//
// Indien de applicatie in een later stadium losgekoppeld zou worden
// van de business laag, om een api in de plaats te gebruiken,
// zal dit de plaats zijn om wijzigingen aan te brengen.
//
// De implementaties van ICommuniceerder zouden in theorie, door middel van DTO gebruik
// hetzelfde moeten retourneren. De reden voor opsplitsing ligt hem in het verschil in de
// manier van gebruiken van de content.
//
// De applicatie maakt gebruik van RequestDTO's bij het aanspreken van een communiceerder,
// de ApiCommuniceerder kan deze gebruiken om om te zetten naar Json en het verzoek aan te maken.
// De BusinessCommuniceerder gebruikt deze om parameters ervan te gebruiken om 
// business laag functies aan te roepen.
// In beide gevallen wordt de DTO nooit als DTO object overgedragen naar een andere laag,
// maar gebruikt en/of geconverteerd.
// 
// Dit is geen uitgebreide klasse maar laat toe om adhoc over te schakelen naar een
// andere databron. Overschakelen doe je door de default waarde van de parameter GebruikApi
// te veranderen.


