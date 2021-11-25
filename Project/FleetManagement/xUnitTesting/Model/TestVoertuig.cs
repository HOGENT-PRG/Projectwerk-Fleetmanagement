using BusinessLaag.Exceptions;
using BusinessLaag.Model;
using BusinessLaag.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace xUnitTesting.Model
{
    public class TestVoertuig
    {
        private static int validId = 1;
        private static string validVoornaam = "Hans";
        private static string validNaam = "Worst";
        private static DateTime validGeboortedatum = DateTime.UnixEpoch;
        private static string validRRN = "90020199902";
        private static string validKaartnummer = "12345678908765432";
        private static DateTime validVervaldatum = DateTime.Now.AddDays(30);
        private static string validPincode = "1111";

        private static string validModel = "1 XY Z";
        private static string validNummerplaat = "1-AAA-SS";
        private static VoertuigBrandstof BrandstofInstance = VoertuigBrandstof.Diesel;
        private static Voertuigsoort VoertuigsoortInstance = Voertuigsoort.sedan;
        private static string validKleur = "rood";
        private static int validAantalDeuren = 4;
        private static string validChassisnummer = "11111111111111111";
        private static Merk MerkInstance = Merk.Bentley;

        private static Adres validAdres = new Adres(1, "Leliestraat", "1B", "9000", "Gent", "Oost-vlaanderen", "Belgium");
        
        private static Bestuurder validBestuurder = new Bestuurder(validId, validNaam, validVoornaam, validAdres, validGeboortedatum, validRRN, RijbewijsSoort.B, validVoertuig, null);

        private static TankkaartBrandstof validBrandstof = TankkaartBrandstof.CNG;
        private static List<TankkaartBrandstof> validBrandstoffen = new List<TankkaartBrandstof>() { validBrandstof };

        private static Tankkaart validTankkaart = new Tankkaart(validId, validKaartnummer, validVervaldatum, validPincode, validBrandstoffen, validBestuurder);

        private static Voertuig validVoertuig = new Voertuig(validId, MerkInstance, validModel, validNummerplaat, BrandstofInstance, VoertuigsoortInstance, validBestuurder, validChassisnummer, validKleur, validAantalDeuren);

        [Theory]
        [InlineData(null)]  // dit omvat tevens de valid Zet methodes (ctor gebruikt deze)
        [InlineData(1)]
        public void Test_Ctor_valid(int? id)
        {
            Voertuig v = new Voertuig(id, MerkInstance, validModel, validNummerplaat, 
                                      BrandstofInstance, VoertuigsoortInstance, 
                                      validBestuurder, validChassisnummer, validKleur, validAantalDeuren);

            if (id is null) Assert.Null(v.Id);
            else Assert.Equal(id, v.Id);

            Assert.Equal(MerkInstance, v.Merk);
            Assert.Equal(validModel, v.Model);
            Assert.Equal(validNummerplaat, v.Nummerplaat);
            Assert.Equal(BrandstofInstance, v.Brandstof);
            Assert.Equal(VoertuigsoortInstance, v.Voertuigsoort);
            Assert.Equal(validKleur, v.Kleur);
            Assert.Equal(validAantalDeuren, v.AantalDeuren);
            Assert.Equal(validBestuurder, v.Bestuurder);
            Assert.Equal(validChassisnummer, v.Chassisnummer);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        [InlineData(int.MinValue)]
        public void Test_Ctor_InvalidId(int id)
        {
            Assert.Throws<VoertuigException>(() => new Voertuig(id, MerkInstance, validModel, validNummerplaat,
                                      BrandstofInstance, VoertuigsoortInstance,
                                      validBestuurder, validChassisnummer, validKleur, validAantalDeuren));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(int.MaxValue)]
        public void Test_Setter_ZetId_valid(int id)
        {
            validVoertuig.ZetId(id);
            Assert.Equal(id, validVoertuig.Id);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public void Test_Setter_ZetId_invalid(int id)
        {
            Assert.Throws<VoertuigException>(() => validVoertuig.ZetId(id));
        }

        [Theory]
        [InlineData("A")]
        [InlineData("0123456789012345678")]
        public void Test_Setter_ZetModel_valid(string m)
        {
            validVoertuig.ZetModel(m);
            Assert.Equal(m, validVoertuig.Model);
        }

        [Theory]
        [InlineData("           ")]
        [InlineData("")]
        [InlineData("01234567890123456789")]
        public void Test_Setter_ZetModel_invalid(string m)
        {
            Assert.Throws<VoertuigException>(() => validVoertuig.ZetModel(m));
        }

        [Theory]
        [InlineData("1")]
        [InlineData("1234")]
        [InlineData("0123456789012345678")]
        public void Test_Setter_ZetNummerplaat_valid(string n)
        {
            validVoertuig.ZetNummerplaat(n);
            Assert.Equal(n, validVoertuig.Nummerplaat);
        }

        [Theory]
        [InlineData("           ")]
        [InlineData("")]
        [InlineData("012345678901234567890")]
        public void Test_Setter_ZetNummerplaat_invalid(string n)
        {
            Assert.Throws<VoertuigException>(() => validVoertuig.ZetNummerplaat(n));
        }

        [Theory]
        [InlineData("Wit")]
        [InlineData("0")]
        [InlineData("012345678901234567890123456789012345678")]
        public void Test_Setter_ZetKleur_valid(string k)
        {
            validVoertuig.ZetKleur(k);
            Assert.Equal(k, validVoertuig.Kleur);
        }

        [Theory]
        [InlineData("           ")]
        [InlineData("")]
        [InlineData("01234567890123456789001234567890123456789")]
        public void Test_Setter_ZetKleur_invalid(string k)
        {
            Assert.Throws<VoertuigException>(() => validVoertuig.ZetKleur(k));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(20)]
        public void Test_Setter_ZetAantalDeuren_valid(int a)
        {
            validVoertuig.ZetAantalDeuren(a);
            Assert.Equal(a, validVoertuig.AantalDeuren);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        [InlineData(21)]
        [InlineData(int.MaxValue)]
        public void Test_Setter_ZetAantalDeuren_invalid(int a)
        {
            Assert.Throws<VoertuigException>(() => validVoertuig.ZetAantalDeuren(a));
        }

        [Fact]
        public void Test_Setter_ZetBestuurder_valid()
        {
            validVoertuig.ZetBestuurder(null);
            Assert.Null(validVoertuig.Bestuurder);

            validVoertuig.ZetBestuurder(validBestuurder);
            Assert.Equal(validBestuurder, validVoertuig.Bestuurder);
        }

        [Fact]
        public void Test_Setter_ZetBestuurderReedsIngesteld_invalid()
        {
            try { validVoertuig.ZetBestuurder(validBestuurder); } catch { }
            Assert.Equal(validBestuurder, validVoertuig.Bestuurder);
            Assert.Throws<VoertuigException>(() => validVoertuig.ZetBestuurder(validBestuurder));
        }

        [Theory]
        [InlineData("01234567890123456")]
        public void Test_Setter_ZetChassisnr_valid(string c)
        {
            validVoertuig.ZetChassisnummer(c);
            Assert.Equal(c, validVoertuig.Chassisnummer);
        }

        [Theory]
        [InlineData("                 ")]
        [InlineData("")]
        [InlineData("012345678901234567")]
        [InlineData("0123456789012345")]
        public void Test_Setter_ZetChassisnr_invalid(string c)
        {
            Assert.Throws<VoertuigException>(() => validVoertuig.ZetChassisnummer(c));
        }

        // Merk, Brandstof en Voertuigsoort worden beschermd dmv enum (geen invalid mogelijk)

        [Fact]
        public void Test_Setter_ZetMerk_valid()
        {
            Assert.NotEqual(Merk.Dacia, validVoertuig.Merk);
            validVoertuig.ZetMerk(Merk.Dacia);
            Assert.Equal(Merk.Dacia, validVoertuig.Merk);
        }

        [Fact]
        public void Test_Setter_ZetBrandstof_valid()
        {
            Assert.NotEqual(VoertuigBrandstof.HybrideDiesel, validVoertuig.Brandstof);
            validVoertuig.ZetBrandstof(VoertuigBrandstof.HybrideDiesel);
            Assert.Equal(VoertuigBrandstof.HybrideDiesel, validVoertuig.Brandstof);
        }

        [Fact]
        public void Test_Setter_ZetVoertuigsoort_valid()
        {
            Assert.NotEqual(Voertuigsoort.terreinwagen, validVoertuig.Voertuigsoort);
            validVoertuig.ZetVoertuigSoort(Voertuigsoort.terreinwagen);
            Assert.Equal(Voertuigsoort.terreinwagen, validVoertuig.Voertuigsoort);
        }
    }
}
