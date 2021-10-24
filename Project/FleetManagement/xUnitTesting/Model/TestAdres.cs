using BusinessLaag.Exceptions;
using BusinessLaag.Model;
using System;
using Xunit;

namespace xUnitTesting.Model
{
    public class TestAdres
    {
        /// AAA pattern 

        private static int validId = 1;
        private static string validStraat = "Leliestraat";
        private static string validHuisnummer = "1/3";
        private static string validPostcode = "9000";
        private static string validPlaatsnaam = "Gent";
        private static string validProvincie = "Oost-vlaanderen";
        private static string validLand = "Belgium";
        private static Adres validAdres = new Adres(validId, validStraat, validHuisnummer, validPostcode, validPlaatsnaam, validProvincie, validLand);

        [Theory]
        [InlineData(null)] // dit omvat tevens de valid Zet methodes (ctor gebruikt deze)
        [InlineData(1)]
        public void Test_Ctor_valid(int? id)
        {
            Adres adr = new Adres(id, validStraat, validHuisnummer, validPostcode, validPlaatsnaam, validProvincie, validLand);

            if (id is null) Assert.Null(adr.Id);
            else Assert.Equal(id, adr.Id);

            Assert.Equal(validHuisnummer, adr.Huisnummer);
            Assert.Equal(validPostcode, adr.Postcode);
            Assert.Equal(validPlaatsnaam, adr.Plaatsnaam);
            Assert.Equal(validProvincie, adr.Provincie);
            Assert.Equal(validLand, adr.Land);
        }


        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        [InlineData(int.MinValue)]
        public void Test_Ctor_InvalidId(int id)
        {
            Assert.Throws<AdresException>(() => new Adres(id, validStraat, validHuisnummer, validPostcode, validPlaatsnaam, validProvincie, validLand));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(10000)]
        [InlineData(int.MaxValue)]
        public void Test_Setter_ZetId_valid(int id) {
            validAdres.zetId(null);
            Assert.Null(validAdres.Id);
            validAdres.zetId(id);
            Assert.Equal(id, validAdres.Id);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public void Test_Setter_ZetId_invalid(int id)
        {
            Assert.Throws<AdresException>(() => validAdres.zetId(id));
        }

        [Theory]
        [InlineData("B")]
        [InlineData("Leliestraat")]
        [InlineData("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA")] // 150
        public void Test_Setter_ZetStraat_valid(string x) {
            Assert.NotEqual(x, validAdres.Straatnaam);
            validAdres.zetStraatnaam(x);
            Assert.Equal(x, validAdres.Straatnaam);
        }

        [Theory]
        [InlineData("")]
        [InlineData("9Leliestraat")]
        [InlineData("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAB")] // 151
        public void Test_Setter_ZetStraat_invalid(string x)
        {
            Assert.Throws<AdresException>(() => validAdres.zetStraatnaam(x));
        }

        [Theory]
        [InlineData("22 bus A")]
        [InlineData("24/B")]
        [InlineData("12345678901234567890123456789012345678901234567890")] // 50
        public void Test_Setter_ZetHuisnummer_valid(string x) {
            Assert.NotEqual(x, validAdres.Huisnummer);
            validAdres.zetHuisnummer(x);
            Assert.Equal(x, validAdres.Huisnummer);
        }

        [Theory]
        [InlineData("Blok Twee")]
        [InlineData("")]
        [InlineData("12345678901234567890123456789012345678901234567890B")] // 51
        public void Test_Setter_ZetHuisnummer_invalid(string x)
        {
            Assert.Throws<AdresException>(() => validAdres.zetHuisnummer(x));
        }

        [Theory]
        [InlineData("0234567890123456789012345678901234567890123456789")] // 49
        [InlineData("9998BENL")]
        [InlineData("1250")]
        public void Test_Setter_ZetPostcode_valid(string x) {
            Assert.NotEqual(x, validAdres.Postcode);
            validAdres.zetPostcode(x);
            Assert.Equal(x, validAdres.Postcode);
        }

        [Theory]
        [InlineData("12345678901234567890123456789012345678901234567890")] // 50
        [InlineData("999")]
        [InlineData("Vlaanderen")]
        [InlineData("")]
        public void Test_Setter_ZetPostcode_invalid(string x)
        {
            Assert.Throws<AdresException>(() => validAdres.zetPostcode(x));
        }

        [Theory]
        [InlineData("Hoboken")]
        [InlineData("Aalst")]
        [InlineData("Amsterdam")]
        [InlineData("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA")] // 150
        public void Test_Setter_ZetPlaats_valid(string x) {
            Assert.NotEqual(x, validAdres.Plaatsnaam);
            validAdres.zetPlaatsnaam(x);
            Assert.Equal(x, validAdres.Plaatsnaam);
        }

        [Theory]
        [InlineData("")]
        [InlineData("9Gent")]
        [InlineData("9999")]
        [InlineData("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAB")] // 151
        public void Test_Setter_ZetPlaats_invalid(string x)
        {
            Assert.Throws<AdresException>(() => validAdres.zetPlaatsnaam(x));
        }

        [Theory]
        [InlineData("Nordrhein-Westfalen")]
        [InlineData("Vlaanderen")]
        [InlineData("West-vlaanderen")]
        [InlineData("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA")] // 150
        public void Test_Setter_ZetProvincie_valid(string x) {
            Assert.NotEqual(x, validAdres.Provincie);
            validAdres.zetProvincie(x);
            Assert.Equal(x, validAdres.Provincie);
        }

        [Theory]
        [InlineData("")]
        [InlineData("9Vlaanderen")]
        [InlineData("9999")]
        [InlineData("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAB")] // 151
        public void Test_Setter_ZetProvincie_invalid(string x)
        {
            Assert.Throws<AdresException>(() => validAdres.zetProvincie(x));
        }

        [Theory]
        [InlineData("Eswatini")]
        [InlineData("Nederland")]
        [InlineData("Congo")]
        [InlineData("BelgiumBBBBelgiumBBBBelgiumBBBBelgiumBBBBelgiumBBBBelgiumBBBBelgiumBBBBelgiumBBBBelgiumBBBBelgiumBBB")] //100
        public void Test_Setter_ZetLand_valid(string x) {
            Assert.NotEqual(x, validAdres.Land);
            validAdres.zetLand(x);
            Assert.Equal(x, validAdres.Land);
        }

        [Theory]
        [InlineData("")]
        [InlineData("9Belgium")]
        [InlineData("9999")]
        [InlineData("BelgiumBBBBelgiumBBBBelgiumBBBBelgiumBBBBelgiumBBBBelgiumBBBBelgiumBBBBelgiumBBBBelgiumBBBBelgiumBBBZ")] //101
        public void Test_Setter_ZetLand_invalid(string x)
        {
            Assert.Throws<AdresException>(() => validAdres.zetLand(x));
        }
    }
}
