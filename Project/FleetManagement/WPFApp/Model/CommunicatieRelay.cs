using WPFApp.Helpers;
using WPFApp.Interfaces;
using WPFApp.Model;
using WPFApp.Model.Communiceerders;

// Wat ) Bepaalt welke ICommuniceer implementatie by default beschikbaar gesteld wordt als CommunicatieKanaal, welke de viewmodels gebruiken. Fungeert als "passthrough" voor communicatie.

// Hoe ) Het is aan te raden om omschakelingen qua ICommuniceer implementatie hier af te handelen, ookal gebeurt instantiering van deze klasse op een centrale locatie, het ApplicatieOverzichtViewModel (waarna het verdeelt wordt naar andere viewmodels), indien dat niet het geval zou zijn dient er bij elke instantiering de waarde aangepast te worden.

// Eigenschap ) Bovenstaande gegeven laat toe om per viewmodel te instantieren indien dat wenselijk is en om zo een gedeelte van de viewmodels een ander CommunicatieKanaal te laten gebruiken.

// Zonder ) Indien deze klasse niet zou bestaan dient men een Business- of ApiCommuniceerder te instantieren, wat, indien het vaak voorkomt, extra werk met zich meebrengt indien er op dat vlak wijzigingen zouden plaatsvinden.
namespace WPFApp {

    internal class CommunicatieRelay {
        public readonly ICommuniceer CommunicatieKanaal;

        public readonly bool GEBRUIK_API;
        public readonly string API_BASIS_PAD;

        public CommunicatieRelay(bool GebruikApi = false, string ApiBasisPad = "http://localhost:5000") {
            API_BASIS_PAD = ApiBasisPad;
            GEBRUIK_API = GebruikApi;

            if (GEBRUIK_API) {
                CommunicatieKanaal = new ApiCommuniceerder(API_BASIS_PAD);
            } else {
                CommunicatieKanaal = new BusinessCommuniceerder();
            }

            
        }

    }

}


