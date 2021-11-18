using BusinessLaag;
using BusinessLaag.Exceptions;
using BusinessLaag.Managers;
using System.Collections.Generic;
using System.Diagnostics;

namespace DataLaag.Helpers {
	// Aangezien CAS deprecated is op .NET niveau maken we een eigen variant:
	//		"The.NET Framework provides a mechanism for the enforcement of varying levels of trust
	//		on different code running in the same application called Code Access Security(CAS).
	//		CAS is not supported in .NET Core, .NET 5, or later versions.CAS is
	//		not supported by versions of C# later than 7.0.
	//		CAS in .NET Framework should not be used as a mechanism for enforcing
	//		security boundaries based on code origination or other identity aspects."
	//		https://docs.microsoft.com/en-us/previous-versions/dotnet/framework/code-access-security/securing-method-access

	// Nut van de klasse) Aangezien de controles plaatsvinden in de managers willen we voor bepaalde functies (de nonqueries)
	//					  dat enkel klasses die gemachtigd zijn deze functies daadwerkelijk aanroepen.
	//
	// Alternatief 1) De managers en de opslag klasses samen in een (aparte) assembly te steken en de opslag klasses internal te maken
	// Alternatief 2) Een friend assembly in te stellen dmv InternalsVisibleToAttribute en de opslag klasses internal te maken,
	//				  echter beperkt dit enkel op assembly niveau, niet op class niveau
	//				  https://docs.microsoft.com/en-us/dotnet/standard/assembly/friend
	//
	// Gedrag waarnemen) Een fiddle is hier terug te vinden https://dotnetfiddle.net/pTJ1PT
	//					 In ons geval werken we "nested":
	//						- de opslag methode roept deze functie aan, dat is niveau 1 van de stackframe
	//						- de oproeper van de opslagmethode, de manager, bevindt zich 1 niveau hoger, op niveau 2

	public static class OproepControleur {

		// Nameof van de klasses ipv string, dan verandert de naam mee bij eventuele refactoring
		// en kan bij het builden geasserteert worden of de waarde nog op iets tastbaar steunt
		public static List<string> GemachtigdeOproepers = new() {
			nameof(BestuurderManager),
			nameof(TankkaartManager),
			nameof(VoertuigManager),
			nameof(FleetManager)
		};

		public static void ControleerOproeperGemachtigd(string extra="") {
			bool matchGevonden = false;

			string OproeperTeValideren = new StackFrame(2).GetMethod().DeclaringType.Name.Trim();

			foreach (string oproeper in GemachtigdeOproepers) {
				if(OproeperTeValideren == extra.Trim()) {
					matchGevonden = true;
					break;
				}

				if (OproeperTeValideren == oproeper.Trim()) {
					matchGevonden = true;
				}
			}

			if (!matchGevonden) {
				throw new OproepControleurException("Oproeper is niet gemachtigd om deze functie aan te roepen.");
			}
		}
	}
}
