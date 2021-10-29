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
        private static DateTime validGeboortedatum = new DateTime(1980, 1, 1);
        private static string validRRN = "90020199902";
        private static Adres validAdres = new Adres(1, "Leliestraat", "1B", "9000", "Gent", "Oost-vlaanderen", "Belgium");
        private static Voertuig validVoertuig = new Voertuig(1, Merk.AlfaRomeo, "1XYZ", "1BCD111", VoertuigBrandstof.Diesel, Voertuigsoort.berline, "rood", null, null, "11111111111111111");
        private static Tankkaart validTankkaart = new Tankkaart(1, "12345678908765432", new DateTime(2024, 1,1), "1111", new List<TankkaartBrandstof>() { TankkaartBrandstof.CNG }, null);

        private Bestuurder validBestuurder = new Bestuurder(1, validNaam, validVoornaam, validAdres, validGeboortedatum, validRRN, RijbewijsSoort.B, validVoertuig, validTankkaart);

        [Theory]
        [InlineData(null)]
        [InlineData(1)]       
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
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(int.MaxValue)]
        public void Test_Setter_ZetId_valid(int id)
        {
            validBestuurder.zetId(id);
            Assert.Equal(id, validBestuurder.Id);
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
        [InlineData("Benjamin")]
        [InlineData("Bo")]
        [InlineData("Been Jammin For Days")]
        [InlineData("AAABBBCCCDAAABBBCCCDAAABBBCCCDAAABBBCCCDAAABBBCCCDAAABBBCCCDAAABBBCCCD")] // 70
        public void Test_Setter_ZetNaam_valid(string n)
        {
            Assert.NotEqual(n, validBestuurder.Naam);
            validBestuurder.zetNaam(n);
            Assert.Equal(n, validBestuurder.Naam);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("B")]
        [InlineData("Benjamin9")]
        [InlineData("Been Jammin 9")]
        [InlineData("AAABBBCCCDAAABBBCCCDAAABBBCCCDAAABBBCCCDAAABBBCCCDAAABBBCCCDAAABBBCCCDZ")] // 71
        public void Test_Setter_ZetNaam_invalid(string n)
        {
            Assert.Throws<BestuurderException>(() => validBestuurder.zetNaam(n));
        }

        [Theory]
        [InlineData("Bo")]
        [InlineData("Benjamin")]
        [InlineData("Been Jammin")]
        [InlineData("AAABBBCCCDAAABBBCCCDAAABBBCCCDAAABBBCCCDAAABBBCCCDAAABBBCCCDAAABBBCCCD")] // 70
        public void Test_Setter_ZetVoornaam_valid(string n)
        {
            Assert.NotEqual(n, validBestuurder.Voornaam);
            validBestuurder.zetVoornaam(n);
            Assert.Equal(n, validBestuurder.Voornaam);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("B")]
        [InlineData("Benjamin9")]
        [InlineData("AAABBBCCCDAAABBBCCCDAAABBBCCCDAAABBBCCCDAAABBBCCCDAAABBBCCCDAAABBBCCCDZ")] // 71
        public void Test_Setter_ZetVoornaam_invalid(string n)
        {
            Assert.Throws<BestuurderException>(() => validBestuurder.zetVoornaam(n));
        }

        [Fact]
        public void Test_Setter_ZetAdres_valid()
        {
            try { validBestuurder.zetAdres(validAdres); } catch { }
            Assert.NotNull(validBestuurder.Adres);
            validBestuurder.zetAdres(null);
            Assert.Null(validBestuurder.Adres); // null toegelaten
            validBestuurder.zetAdres(validAdres);
            Assert.Equal(validAdres, validBestuurder.Adres);
        }

        [Fact]
        public void Test_Setter_ZetAdres_NieuweWaardeZelfdeAlsHuidige_invalid()
        {
            try { validBestuurder.zetAdres(validAdres); } catch { }
            Assert.Throws<BestuurderException>(() => validBestuurder.zetAdres(validAdres));
        }

        [Fact]
        public void Test_Setter_ZetGeboortedatum_valid()
        {
            DateTime _validGeboorteDatum1 = DateTime.Now.AddYears(-120).AddDays(1);
            DateTime _validGeboorteDatum2 = DateTime.Now.AddMinutes(-1);      //kan niet met inline data

            Assert.NotEqual(_validGeboorteDatum1, validBestuurder.GeboorteDatum);
            validBestuurder.zetGeboortedatum(_validGeboorteDatum1);
            Assert.Equal(_validGeboorteDatum1, validBestuurder.GeboorteDatum);
            validBestuurder.zetGeboortedatum(_validGeboorteDatum2);
            Assert.Equal(_validGeboorteDatum2, validBestuurder.GeboorteDatum);
        }

        [Fact]
        public void Test_Setter_ZetGeboortedatum_invalid()
        {
            DateTime _invalidGeboorteDatum1 = DateTime.Now.AddYears(-120).AddDays(-1);
            DateTime _invalidGeboorteDatum2 = DateTime.Now.AddMinutes(1);      //kan niet met inline data

            Assert.Throws<BestuurderException>(() => validBestuurder.zetGeboortedatum(_invalidGeboorteDatum1));
            Assert.Throws<BestuurderException>(() => validBestuurder.zetGeboortedatum(_invalidGeboorteDatum2));
        }

        [Theory]
        [InlineData("90020199902")]
        public void Test_Setter_ZetRRN_valid(string n)
        {
            validBestuurder.zetRijksregisternummer(n);
        }

        [Theory]
        [InlineData("90020199901")]
        [InlineData("90020199903")]
        [InlineData("85020100201")]
        [InlineData("")]
        [InlineData("1")]
        public void Test_Setter_ZetRRN_invalid(string n)
        {
            Assert.Throws<BestuurderException>(() => validBestuurder.zetRijksregisternummer(n));
        }

        [Fact]
        public void Test_Setter_ZetTankkaart_valid()
        {
            validBestuurder.zetTankkaart(null);
            Assert.Null(validBestuurder.Tankkaart);
            validBestuurder.zetTankkaart(validTankkaart);
            Assert.Equal(validTankkaart, validBestuurder.Tankkaart);
        }

        [Fact]
        public void Test_Setter_ZetTankkaart_NieuweWaardeZelfdeAlsHuidige_invalid()
        {
            Assert.Throws<BestuurderException>(() => validBestuurder.zetTankkaart(validTankkaart));
        }

        [Fact]
        public void Test_Setter_ZetVoertuig_valid()
        {
            validBestuurder.zetVoertuig(null);
            Assert.Null(validBestuurder.Voertuig);
            validBestuurder.zetVoertuig(validVoertuig);
            Assert.Equal(validVoertuig, validBestuurder.Voertuig);
        }

        [Fact]
        public void Test_Setter_ZetVoertuig_NieuweWaardeZelfdeAlsHuidige_invalid()
        {
            Assert.Throws<BestuurderException>(() => validBestuurder.zetVoertuig(validVoertuig));
        }

        [Fact]
        public void Test_Rijbewijs_valid()
        {
            validBestuurder.zetRijbewijs(RijbewijsSoort.G);
            Assert.Equal(RijbewijsSoort.G, validBestuurder.RijbewijsSoort);
            validBestuurder.zetRijbewijs(RijbewijsSoort.A);
            Assert.Equal(RijbewijsSoort.A, validBestuurder.RijbewijsSoort);
        }



    }
}
