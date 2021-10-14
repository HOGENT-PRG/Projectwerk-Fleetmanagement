using System;
using BusinessLaag.Exceptions;
using System.Globalization;
namespace BusinessLaag.Helpers
{
    public class RRNValideerder
    {
        public RRNValideerder(string rijksregisternummer)
        {
            Valideer(rijksregisternummer);
        }
        public string Rijksregisternummer { get; set; }
        public bool Valideer(string rijksregnummer)
        {
            Rijksregisternummer = rijksregnummer;
            int geboortejaar = int.Parse(rijksregnummer.Substring(0, 2));
            int geboorteMaand = int.Parse(rijksregnummer.Substring(2, 2));
            int geboortedag = int.Parse(rijksregnummer.Substring(4, 2));
            int e = CultureInfo.CurrentUICulture.Calendar.ToFourDigitYear(geboortejaar);
            DateTime a = new DateTime(e, geboorteMaand, geboortedag);
            //  string format = "dd/MM/yyyy";
            int Controlecijfer = 0;
            // Console.WriteLine(a.ToString(format));
            if (a.Year >= 2000)
            {
                int divide = 97 - (int.Parse("2" + rijksregnummer.Substring(0, 9)) % 97);
                Console.WriteLine(divide);
                Controlecijfer += divide;
            }
            else
            {
                int divide = 97 - (int.Parse(rijksregnummer.Substring(0, 9)) % 97);
                //  Console.WriteLine(divide);
                Controlecijfer += divide;
            }
            if (Controlecijfer == int.Parse(rijksregnummer.Substring(9, 2)))
            {
                // ("Rijksregnummer geldig");
                return true;

            }
            else
            {
                // ("Rijksregnummer ongeldig");

                throw new BestuurderException("rijksregisternummer is niet geldig");
            }

        }
    }
    }
