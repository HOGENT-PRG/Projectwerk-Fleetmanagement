using BusinessLaag;
using BusinessLaag.Model;
using BusinessLaag.Model.Enum;
using BusinessLaag.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace xUnitTesting.Model
{
    public class TestBestuurder
    {
        private static int validId = 1;
        private static string validVoornaam = "Hans";
        private static string validNaam = "Worst";
        private static long validGeboortedatum = 34740539;
        private static string validRRN = "90020199902";
        private static Adres validAdres = new Adres(1, "Leliestraat", "1B", "9000", "Gent", "Oost-vlaanderen", "Belgium");
        private static Voertuig validVoertuig = new Voertuig(1, Merk.AlfaRomeo, "1XYZ", "1BCD111", Brandstof.diesel, Voertuigsoort.berline, "rood", null, null, "11111111111111111");
        private static Tankkaart validTankkaart = new Tankkaart(1, "12345678908765432", 16347405391, "1111", new List<Brandstof>() { Brandstof.cng }, null);

        private Bestuurder validBestuurder = new Bestuurder(1, validNaam, validVoornaam, validAdres, validGeboortedatum, validRRN, RijbewijsSoort.B, validVoertuig, validTankkaart);

        // ZetRijbewijs beschermd door gebruik enum, geen test

        [Theory]
        [InlineData(null)]
        [InlineData(1)]       // dit omvat tevens de valid Zet methodes (ctor gebruikt deze)
        public void Test_Ctor_valid(int? id)
        {
            Bestuurder b = new Bestuurder(id, validNaam, validVoornaam, validAdres, validGeboortedatum, validRRN, RijbewijsSoort.B, validVoertuig, validTankkaart);

            if (id is null) Assert.Null(b.Id);
            else Assert.Equal(id, b.Id);

            Assert.Equal(validNaam,b.Naam);
            Assert.Equal(validVoornaam,b.Voornaam);
            Assert.Equal(validAdres,b.Adres);
            Assert.Equal(validGeboortedatum,b.GeboorteDatum);
            Assert.Equal(validRRN,b.RijksRegisterNummer);
            Assert.Equal(RijbewijsSoort.B,b.RijbewijsSoort);
            Assert.Equal(validVoertuig,b.Voertuig);
            Assert.Equal(validTankkaart,b.Tankkaart);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        [InlineData(int.MinValue)]
        public void Test_Ctor_InvalidId(int id)
        {
            Assert.Throws<BestuurderException>(() => new Bestuurder(id, validNaam, validVoornaam, validAdres, validGeboortedatum, validRRN, RijbewijsSoort.B, validVoertuig, validTankkaart));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public void Test_Setter_ZetId_invalid(int id)
        {
            Assert.Throws<BestuurderException>(() => validBestuurder.zetId(id));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("B")]
        public void Test_Setter_ZetNaam_invalid(string n)
        {
            Assert.Throws<BestuurderException>(() => validBestuurder.zetNaam(n));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("B")]
        public void Test_Setter_ZetVoornaam_invalid(string n)
        {
            Assert.Throws<BestuurderException>(() => validBestuurder.zetVoornaam(n));
        }

        [Fact]
        public void Test_Setter_ZetNaam_NieuweWaardeZelfdeAlsHuidige_invalid()
        {
            Assert.Throws<BestuurderException>(() => validBestuurder.zetAdres(validAdres));
        }

        [Theory]
        [InlineData((long)int.MinValue-1)]
        [InlineData((long)int.MinValue-100)]
        public void Test_Setter_ZetGeboortedatum_invalid(long n)
        {
            Assert.Throws<BestuurderException>(() => validBestuurder.zetGeboortedatum(n));
        }

        [Theory]
        [InlineData("90020199901")]
        [InlineData("90020199903")]
        [InlineData("")]
        [InlineData("1")]
        public void Test_Setter_ZetRRN_invalid(string n)
        {
            Assert.Throws<BestuurderException>(() => validBestuurder.zetRijksregisternummer(n));
        }

        [Fact]
        public void Test_Setter_ZetTankkaart_NieuweWaardeZelfdeAlsHuidige_invalid()
        {
            Assert.Throws<BestuurderException>(() => validBestuurder.zetTankkaart(validTankkaart));
        }

        [Fact]
        public void Test_Setter_ZetVoertuig_NieuweWaardeZelfdeAlsHuidige_invalid()
        {
            Assert.Throws<BestuurderException>(() => validBestuurder.zetVoertuig(validVoertuig));
        }



    }
}
