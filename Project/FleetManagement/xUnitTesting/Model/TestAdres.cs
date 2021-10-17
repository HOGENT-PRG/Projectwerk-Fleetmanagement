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
        [InlineData(null)]
        [InlineData(1)]
        public void Test_Ctor_valid(int? id)
        {
            Adres adr = new Adres(id, validStraat, validHuisnummer, validPostcode, validPlaatsnaam, validProvincie, validLand);

            Assert.Equal(id, adr.Id);
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
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public void Test_Setter_ZetId_invalid(int id)
        {
            Assert.Throws<AdresException>(() => validAdres.zetId(id));
        }

        [Theory]
        [InlineData("")]
        [InlineData("9Leliestraat")]
        public void Test_Setter_ZetStraat_invalid(string x)
        {
            Assert.Throws<AdresException>(() => validAdres.zetStraatnaam(x));
        }

        [Theory]
        [InlineData("Blok Twee")]
        [InlineData("")]
        public void Test_Setter_ZetHuisnummer_invalid(string x)
        {
            Assert.Throws<AdresException>(() => validAdres.zetHuisnummer(x));
        }

        [Theory]
        [InlineData("999")]
        [InlineData("Vlaanderen")]
        [InlineData("")]
        public void Test_Setter_ZetPostcode_invalid(string x)
        {
            Assert.Throws<AdresException>(() => validAdres.zetPostcode(x));
        }

        [Theory]
        [InlineData("")]
        [InlineData("9Gent")]
        [InlineData("9999")]
        public void Test_Setter_ZetPlaats_invalid(string x)
        {
            Assert.Throws<AdresException>(() => validAdres.zetPlaatsnaam(x));
        }

        [Theory]
        [InlineData("")]
        [InlineData("9Vlaanderen")]
        [InlineData("9999")]
        public void Test_Setter_ZetProvincie_invalid(string x)
        {
            Assert.Throws<AdresException>(() => validAdres.zetProvincie(x));
        }

        [Theory]
        [InlineData("")]
        [InlineData("9Belgium")]
        [InlineData("9999")]
        public void Test_Setter_ZetLand_invalid(string x)
        {
            Assert.Throws<AdresException>(() => validAdres.zetStraatnaam(x));
        }

        //[Theory]
        //[InlineData()]
        //public void Test_Setter_ZetX_invalid(string x)
        //{
        //    Assert.Throws<AdresException>(() => validAdres.zetX(x));
        //}
    }
}
