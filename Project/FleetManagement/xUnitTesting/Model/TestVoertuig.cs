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
        private static long validGeboortedatum = 34740539;
        private static string validRRN = "90020199902";
        private static string validKaartnummer = "12345678908765432";
        private static long validDate = 16347405391;
        private static string validPincode = "1111";
        private static string validModel = "1 XY Z";
        private static string validNummerplaat = "1-AAA-SS";
        private static Brandstof BrandstofInstance = Brandstof.diesel;
        private static Voertuigsoort VoertuigsoortInstance = Voertuigsoort.sedan;
        private static string validKleur = "rood";
        private static int validAantalDeuren = 4;
        private static string validChassisnummer = "11111111111111111";
        private static Merk MerkInstance = Merk.Bentley;

        private static Adres validAdres = new Adres(1, "Leliestraat", "1B", "9000", "Gent", "Oost-vlaanderen", "Belgium");
        
        private static Bestuurder validBestuurder = new Bestuurder(validId, validNaam, validVoornaam, validAdres, validGeboortedatum, validRRN, RijbewijsSoort.B, validVoertuig, null);

        private static Brandstof validBrandstof = Brandstof.cng;
        private static List<Brandstof> validBrandstoffen = new List<Brandstof>() { validBrandstof };

        private static Tankkaart validTankkaart = new Tankkaart(validId, validKaartnummer, validDate, validPincode, new List<Brandstof>() { Brandstof.cng }, validBestuurder);

        private static Voertuig validVoertuig = new Voertuig(validId, MerkInstance, validModel, validNummerplaat, BrandstofInstance, VoertuigsoortInstance, validKleur, validAantalDeuren, validBestuurder, validChassisnummer);

        [Theory]
        [InlineData(null)]  // dit omvat tevens de valid Zet methodes (ctor gebruikt deze)
        [InlineData(1)]
        public void Test_Ctor_valid(int? id)
        {
            Voertuig v = new Voertuig(id, MerkInstance, validModel, validNummerplaat, 
                                      BrandstofInstance, VoertuigsoortInstance, validKleur, validAantalDeuren, 
                                      validBestuurder, validChassisnummer);

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
                                      BrandstofInstance, VoertuigsoortInstance, validKleur, validAantalDeuren,
                                      validBestuurder, validChassisnummer));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public void Test_Setter_ZetId_invalid(int id)
        {
            Assert.Throws<VoertuigException>(() => validVoertuig.zetId(id));
        }

        [Theory]
        [InlineData("           ")]
        [InlineData("")]
        [InlineData("012345678901234567890")]
        public void Test_Setter_ZetModel_invalid(string m)
        {
            Assert.Throws<VoertuigException>(() => validVoertuig.zetModel(m));
        }

        [Theory]
        [InlineData("           ")]
        [InlineData("")]
        [InlineData("012345678901234567890")]
        public void Test_Setter_ZetNummerplaat_invalid(string n)
        {
            Assert.Throws<VoertuigException>(() => validVoertuig.zetNummerplaat(n));
        }

        [Theory]
        [InlineData("           ")]
        [InlineData("")]
        [InlineData("01234567890123456789001234567890123456789")]
        public void Test_Setter_ZetKleur_invalid(string k)
        {
            Assert.Throws<VoertuigException>(() => validVoertuig.zetKleur(k));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void Test_Setter_ZetAantalDeuren_invalid(int a)
        {
            Assert.Throws<VoertuigException>(() => validVoertuig.zetAantalDeuren(a));
        }

        [Fact]
        public void Test_Setter_ZetBestuurderNull_valid()
        {
            validVoertuig.zetBestuurder(null);
            Assert.Null(validVoertuig.Bestuurder);
        }

        [Fact]
        public void Test_Setter_ZetBestuurderReedsIngesteld_invalid()
        {
            try { validVoertuig.zetBestuurder(validBestuurder); } catch { }
            Assert.Throws<VoertuigException>(() => validVoertuig.zetBestuurder(validBestuurder));
        }

        [Theory]
        [InlineData("                 ")]
        [InlineData("")]
        [InlineData("012345678901234567")]
        [InlineData("0123456789012345")]
        public void Test_Setter_ZetChassisnr_invalid(string c)
        {
            Assert.Throws<VoertuigException>(() => validVoertuig.zetChasisnummer(c));
        }

        // Merk, Brandstof en Voertuigsoort worden beschermd dmv enum
    }
}
