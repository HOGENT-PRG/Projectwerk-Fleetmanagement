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
        private static DateTime validGeboortedatum = new DateTime(1980, 1, 1);
        private static string validRRN = "90020199902";
        private static string validKaartnummer = "12345678908765432";
        private static DateTime validVervaldatum = DateTime.Now.AddDays(30);
        private static string validPincode = "1111";
        private static Adres validAdres = new Adres(1, "Leliestraat", "1B", "9000", "Gent", "Oost-vlaanderen", "Belgium");
        private static Voertuig validVoertuig = new Voertuig(1, Merk.AlfaRomeo, "1XYZ", "1BCD111", VoertuigBrandstof.Diesel, Voertuigsoort.berline, null, "11111111111111111", "rood", null);
        private static Bestuurder validBestuurder = new Bestuurder(validId, validNaam, validVoornaam, validAdres, validGeboortedatum, validRRN, RijbewijsSoort.B, validVoertuig, null);

        private static TankkaartBrandstof validBrandstof = TankkaartBrandstof.CNG;
        private static List<TankkaartBrandstof> validBrandstoffen = new List<TankkaartBrandstof>() { validBrandstof};

        private static Tankkaart validTankkaart = new Tankkaart(validId, validKaartnummer, validVervaldatum, validPincode, new List<TankkaartBrandstof>() { TankkaartBrandstof.CNG }, validBestuurder);

        [Theory]
        [InlineData(null)]  
        [InlineData(1)]
        public void Test_Ctor_valid(int? id)
        {
            Tankkaart t = new Tankkaart(id, validKaartnummer, validVervaldatum, validPincode, validBrandstoffen, validBestuurder);

            if (id is null) Assert.Null(t.Id);
            else Assert.Equal(id, t.Id);

            Assert.Equal(validKaartnummer, t.Kaartnummer);
            Assert.Equal(validVervaldatum, t.Vervaldatum);
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
            Assert.Throws<TankkaartException>(() => new Tankkaart(id, validKaartnummer, validVervaldatum, validPincode, validBrandstoffen, validBestuurder));
        }

        [Theory]
        [InlineData(null)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(int.MaxValue)]
        public void Test_Setter_ZetId_valid(int? id)
        {
            validTankkaart.ZetId(id);
            Assert.Equal(id, validTankkaart.Id);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public void Test_Setter_ZetId_invalid(int id)
        {
            Assert.Throws<TankkaartException>(() => validTankkaart.ZetId(id));
        }

        [Theory]
        [InlineData("12345")]
        [InlineData("723TUS-UYEWHIJ-QWUIDHJ")]
        [InlineData("B1234567890123456789012345678901234567890123456789")] // 51
        public void Test_Setter_ZetKaartnummer_valid(string k)
        {
            validTankkaart.ZetKaartnummer("00001");
            Assert.NotEqual(k, validTankkaart.Kaartnummer);
            validTankkaart.ZetKaartnummer(k);
            Assert.Equal(k, validTankkaart.Kaartnummer);
        }

        [Theory]
        [InlineData("1234")]
        [InlineData("")]
        [InlineData("123456789012345678901234567890123456789012345678901")] // 51
        public void Test_Setter_ZetKaartnummer_invalid(string k)
        {
            Assert.Throws<TankkaartException>(() => validTankkaart.ZetKaartnummer(k));
        }

        [Fact]
        public void Test_Setter_ZetVervaldatum_valid()
        {
            DateTime valid1 = DateTime.Now.AddHours(24);
            DateTime valid2 = DateTime.Today.AddDays(7);               // kan niet met inline data
            DateTime valid3 = DateTime.Today.AddYears(10);
            DateTime valid4 = DateTime.MaxValue;

            validTankkaart.ZetVervaldatum(DateTime.MaxValue.AddHours(-1));
            Assert.NotEqual(valid1, validTankkaart.Vervaldatum);
            validTankkaart.ZetVervaldatum(valid1);
            Assert.Equal(valid1, validTankkaart.Vervaldatum);
            validTankkaart.ZetVervaldatum(valid2);
            Assert.Equal(valid2, validTankkaart.Vervaldatum);
            validTankkaart.ZetVervaldatum(valid3);
            Assert.Equal(valid3, validTankkaart.Vervaldatum);
            validTankkaart.ZetVervaldatum(valid4);
            Assert.Equal(valid4, validTankkaart.Vervaldatum);
        }

        [Fact]
        public void Test_Setter_ZetVervaldatum_invalid()
        {
            DateTime invalid1 = DateTime.Now;
            DateTime invalid2 = DateTime.Today;               // kan niet met inline data
            DateTime invalid3 = DateTime.Today.AddDays(-1);
            DateTime invalid4 = DateTime.UnixEpoch;
            DateTime invalid5 = DateTime.MinValue;

            Assert.Throws<TankkaartException>(() => validTankkaart.ZetVervaldatum(invalid1));
            Assert.Throws<TankkaartException>(() => validTankkaart.ZetVervaldatum(invalid2));
            Assert.Throws<TankkaartException>(() => validTankkaart.ZetVervaldatum(invalid3));
            Assert.Throws<TankkaartException>(() => validTankkaart.ZetVervaldatum(invalid4));
            Assert.Throws<TankkaartException>(() => validTankkaart.ZetVervaldatum(invalid5));
        }

        [Theory]
        [InlineData("1234")]
        public void Test_Setter_ZetPincode_valid(string p)
        {
            Assert.NotEqual(p, validTankkaart.Pincode);
            validTankkaart.ZetPincode(p);
            Assert.Equal(p, validTankkaart.Pincode);
        }

        [Theory]
        [InlineData("123")]
        [InlineData("12345")]
        [InlineData("AAAA")]
        [InlineData("")]
        public void Test_Setter_ZetPincode_invalid(string p)
        {
            Assert.Throws<TankkaartException>(() => validTankkaart.ZetPincode(p));
        }

        [Theory]
        [InlineData(TankkaartBrandstof.Benzine)]
        [InlineData(TankkaartBrandstof.Diesel)]
        public void Test_Setter_Brandstoffen_valid(TankkaartBrandstof b)
        {
            List<TankkaartBrandstof> bs = new List<TankkaartBrandstof>(validTankkaart.GeldigVoorBrandstoffen);
            bs.ForEach(b => validTankkaart.VerwijderBrandstof(b));

            Assert.DoesNotContain(b, validTankkaart.GeldigVoorBrandstoffen);
            Assert.Empty(validTankkaart.GeldigVoorBrandstoffen);
            validTankkaart.VoegBrandstofToe(b);
            Assert.Contains(b, validTankkaart.GeldigVoorBrandstoffen);
            validTankkaart.VoegBrandstofToe(TankkaartBrandstof.CNG);
            Assert.Contains(TankkaartBrandstof.CNG, validTankkaart.GeldigVoorBrandstoffen);
            Assert.Equal(2, validTankkaart.GeldigVoorBrandstoffen.Count);
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
            Assert.Contains(validBrandstof, validTankkaart.GeldigVoorBrandstoffen);
            Assert.Single(validTankkaart.GeldigVoorBrandstoffen);
            validTankkaart.VerwijderBrandstof(validBrandstof);
            Assert.Empty(validTankkaart.GeldigVoorBrandstoffen);
        }

        [Fact]
        public void Test_Setter_Brandstoffen_DeleteBestaatNiet_invalid()
        {
            TankkaartBrandstof e = TankkaartBrandstof.Elektrisch;
            Assert.DoesNotContain(e, validTankkaart.GeldigVoorBrandstoffen);
            Assert.Throws<TankkaartException>(() => validTankkaart.VerwijderBrandstof(e));
            Assert.DoesNotContain(e, validTankkaart.GeldigVoorBrandstoffen);
        }

        [Fact]
        public void Test_Setter_Bestuurder_valid()
        {
            validTankkaart.ZetBestuurder(null);
            Assert.NotEqual(validBestuurder, validTankkaart.Bestuurder);
            validTankkaart.ZetBestuurder(validBestuurder);
            Assert.Equal(validBestuurder, validTankkaart.Bestuurder);
        }

        [Fact]
        public void Test_Setter_Bestuurder_AlIngesteld_invalid()
        {
            Assert.Equal(validBestuurder, validTankkaart.Bestuurder);
            Assert.Throws<TankkaartException>(() => validTankkaart.ZetBestuurder(validBestuurder));
            Assert.Equal(validBestuurder, validTankkaart.Bestuurder);
        }

        [Fact]
        public void Test_Setter_Brandstoffen_Null_valid()
        {
            Assert.NotNull(validTankkaart.Bestuurder);
            validTankkaart.ZetBestuurder(null);
            Assert.Null(validTankkaart.Bestuurder);
        }

    }
}
