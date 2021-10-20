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
    public class TestTankkaart
    {
        private static int validId = 1;
        private static string validVoornaam = "Hans";
        private static string validNaam = "Worst";
        private static long validGeboortedatum = 34740539;
        private static string validRRN = "90020199902";
        private static string validKaartnummer = "12345678908765432";
        private static long validDate = 16347405391;
        private static string validPincode = "1111";
        private static Adres validAdres = new Adres(1, "Leliestraat", "1B", "9000", "Gent", "Oost-vlaanderen", "Belgium");
        private static Voertuig validVoertuig = new Voertuig(1, Merk.AlfaRomeo, "1XYZ", "1BCD111", Brandstof.diesel, Voertuigsoort.berline, "rood", null, null, "11111111111111111");
        private static Bestuurder validBestuurder = new Bestuurder(validId, validNaam, validVoornaam, validAdres, validGeboortedatum, validRRN, RijbewijsSoort.B, validVoertuig, null);

        private static Brandstof validBrandstof = Brandstof.cng;
        private static List<Brandstof> validBrandstoffen = new List<Brandstof>() { validBrandstof};

        private static Tankkaart validTankkaart = new Tankkaart(validId, validKaartnummer, validDate, validPincode, new List<Brandstof>() { Brandstof.cng }, validBestuurder);

        [Theory]
        [InlineData(null)]  // dit omvat tevens de valid Zet methodes (ctor gebruikt deze)
        [InlineData(1)]
        public void Test_Ctor_valid(int? id)
        {
            Tankkaart t = new Tankkaart(id, validKaartnummer, validDate, validPincode, validBrandstoffen, validBestuurder);

            if (id is null) Assert.Null(t.Id);
            else Assert.Equal(id, t.Id);

            Assert.Equal(validKaartnummer, t.Kaartnummer);
            Assert.Equal(validDate, t.Vervaldatum);
            Assert.Equal(validPincode, t.Pincode);
            Assert.Equal(validBrandstoffen, t.GeldigVoorBrandstoffen);
            Assert.Equal(validBestuurder, t.Bestuurder);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        [InlineData(int.MinValue)]
        public void Test_Ctor_InvalidId(int id)
        {
            Assert.Throws<TankkaartException>(() => new Tankkaart(id, validKaartnummer, validDate, validPincode, validBrandstoffen, validBestuurder));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public void Test_Setter_ZetId_invalid(int id)
        {
            Assert.Throws<TankkaartException>(() => validTankkaart.zetId(id));
        }

        [Theory]
        [InlineData("1234")]
        [InlineData("")]
        [InlineData("123456789012345678901234567890123456789012345678901")] // 51
        public void Test_Setter_ZetKaartnummer_invalid(string k)
        {
            Assert.Throws<TankkaartException>(() => validTankkaart.zetKaartnummer(k));
        }

        [Theory]
        [InlineData(null)]
        [InlineData(0)]
        [InlineData(int.MinValue)]
        public void Test_Setter_ZetVervaldatum_invalid(long? ts)
        {
            if(ts is null)
            {
                Assert.Throws<TankkaartException>(() => validTankkaart.zetVervaldatum(DateTimeOffset.Now.ToUnixTimeSeconds() - 100));
            } else
            {
                Assert.Throws<TankkaartException>(() => validTankkaart.zetVervaldatum((long)ts));
            }
        }

        [Theory]
        [InlineData("123")]
        [InlineData("12345")]
        [InlineData("")]
        public void Test_Setter_ZetPincode_invalid(string p)
        {
            Assert.Throws<TankkaartException>(() => validTankkaart.zetPincode(p));
        }

        [Theory]
        [InlineData(Brandstof.benzine)]
        [InlineData(Brandstof.hybrideBenzine)]
        public void Test_Setter_Brandstoffen_valid(Brandstof b)
        {
            validTankkaart.VoegBrandstofToe(b);
            Assert.Contains<Brandstof>(b, validTankkaart.GeldigVoorBrandstoffen);
        }

        [Fact]
        public void Test_Setter_Brandstoffen_InsertBestaatAl_invalid()
        {
            if (!validTankkaart.GeldigVoorBrandstoffen.Contains(validBrandstof))
            {
                validTankkaart.GeldigVoorBrandstoffen.Add(validBrandstof);
            }

            Assert.Throws<TankkaartException>(() => validTankkaart.VoegBrandstofToe(validBrandstof));
        }

        [Fact]
        public void Test_Setter_Brandstoffen_DeleteBestaat_valid()
        {
            validTankkaart.VerwijderBrandstof(validBrandstof);
            Assert.Empty(validTankkaart.GeldigVoorBrandstoffen);
        }

        [Fact]
        public void Test_Setter_Brandstoffen_DeleteBestaatNiet_invalid()
        {
            Brandstof e = Brandstof.elektrisch;
            Assert.Throws<TankkaartException>(() => validTankkaart.VerwijderBrandstof(e));
        }

        [Fact]
        public void Test_Setter_Brandstoffen_BestuurderAlIngesteld_invalid()
        {
            Assert.Throws<TankkaartException>(() => validTankkaart.zetBestuurder(validBestuurder));
        }

        [Fact]
        public void Test_Setter_Brandstoffen_Null_valid()
        {
            validTankkaart.zetBestuurder(null);
            Assert.Null(validTankkaart.Bestuurder);
        }

    }
}
