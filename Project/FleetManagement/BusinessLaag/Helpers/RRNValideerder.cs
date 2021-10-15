using System;
using BusinessLaag.Exceptions;
using System.Globalization;
using System.Threading;

namespace BusinessLaag.Helpers
{
    public class RRNValideerder
    {
        public string Rijksregisternummer { get; set; }
        public bool Valideer(string rijksregnummer)
        {
            // Controle van lengte (anders kan int.Parse mogelijk exception throwen)
            if(rijksregnummer.Length != 11)
            {
                return false;
            }

            //* Wat nog ontbrak is de controle of getallen 6,7,8 niet hoger zijn dan 998
            // "... Het is de dagteller van de geboortes. Voor een man van 001 tot 997 en voor een vrouw van 002 tot 998."
            int dagteller = int.Parse(rijksregnummer.Substring(6, 3));

            /// 998 en 999 zijn dus ongeldig
            if(dagteller >= 998)
            {
                return false;
            }


            //* Aangezien het eindjaar (TwoDigitYearMax) voor de kalender standaard staat ingesteld op 7 jaar in de toekomst dan zou dat betekenen 
            //* dat we kans hebben foutief een jaar in de toekomst toe te kennen, 22-29 wordt met default configuratie 2022-2029 ipv 1922-1929
            //* Ter preventie stellen we het jaar (TwoDigitYearMax) in op het huidige jaar, zodat we deze niet kunnen overschrijden
            int geboortejaar = int.Parse(rijksregnummer.Substring(0, 2));
            var clone = Thread.CurrentThread.CurrentCulture.Clone() as CultureInfo;
            clone.Calendar.TwoDigitYearMax = DateTime.Now.Year;
            int geconverteerdGeboortejaar = clone.Calendar.ToFourDigitYear(geboortejaar);


            //* "Indien de persoon vluchteling is en de geboortedatum niet gekend is, wordt de geboortemaand op 00 gezet en de geboortedag op 00 gezet."
            //* In dat geval zal DateTime echter een exception throwen, maar we hebben het toch niet meer nodig door de cultureinfo kalender hierboven.
            // DateTime a = new DateTime(e, geboorteMaand, geboortedag);
            // Output: [System.ArgumentOutOfRangeException: Year, Month, and Day parameters describe an un-representable DateTime.]

            int controleCijfer = 0;

            if (geconverteerdGeboortejaar >= 2000)
            {
                int restNaFractie = 97 - (int.Parse("2" + rijksregnummer.Substring(0, 9)) % 97);
                controleCijfer += restNaFractie;
            }
            else
            {
                int restNaFractie = 97 - (int.Parse(rijksregnummer.Substring(0, 9)) % 97);
                controleCijfer += restNaFractie;
            }

            if (controleCijfer == int.Parse(rijksregnummer.Substring(9, 2)))
            {
                // Validatie gelukt
                Rijksregisternummer = rijksregnummer;
                return true;
            }

            // Validatie mislukt
            return false;
        }
    }
    }
