using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLaag.Model;
using BusinessLaag;
using BusinessLaag.Exceptions;
using BusinessLaag.Interfaces;
using System.Reflection;
using System.Data.SqlClient;
using DataLaag.Helpers;
using Newtonsoft.Json;
using System.Data;
using System.Text.RegularExpressions;
using BusinessLaag.Model.Enum;

namespace DataLaag.Repositories
{
    public sealed class BestuurderOpslag : IBestuurderOpslag 
    {
        private SqlConnection _conn { get; set; }

        public void ZetConnectionString(string connString) {
            _conn = connString.Length > 5 ? new SqlConnection(connString) : throw new BestuurderOpslagException("Connectiestring moet langer zijn dan 5 karakters.");
        }

        // Adres
        // Valt onder bewind van Bestuurder
        public int VoegAdresToe(Adres adres) {

            try {
                _conn.Open();
                SqlCommand cmd = _conn.CreateCommand();

                cmd.CommandText = "INSERT INTO Adres " +
                                  "(Straatnaam, Huisnummer, Postcode, Plaatsnaam, Provincie, Land) " +
                                  "OUTPUT INSERTED.Id " +
                                  "VALUES (@strt, @hn, @pc, @pn, @prov, @land) ;";
                cmd.Parameters.Add(new SqlParameter("@strt", DbType.String));
                cmd.Parameters.Add(new SqlParameter("@hn", DbType.String));
                cmd.Parameters.Add(new SqlParameter("@pc", DbType.String));
                cmd.Parameters.Add(new SqlParameter("@pn", DbType.String));
                cmd.Parameters.Add(new SqlParameter("@prov", DbType.String));
                cmd.Parameters.Add(new SqlParameter("@land", DbType.String));
                cmd.Parameters["@strt"].Value = adres.Straatnaam;
                cmd.Parameters["@hn"].Value = adres.Huisnummer;
                cmd.Parameters["@pc"].Value = adres.Postcode;
                cmd.Parameters["@pn"].Value = adres.Plaatsnaam;
                cmd.Parameters["@prov"].Value = adres.Provincie;
                cmd.Parameters["@land"].Value = adres.Land;

                return Convert.ToInt32(cmd.ExecuteScalar());
            } catch (Exception e) {
                throw new BestuurderOpslagException("Er vond een onverwachte fout plaats tijdens het toevoegen van een adres.", e);
            } finally {
                _conn.Close();
            }
        }

        public List<Adres> GeefAdressen(string? kolom=null, object? waarde = null) {
            try {
                _conn.Open();
                SqlCommand cmd = _conn.CreateCommand();

                string zoekAppendix = "";

                if (kolom != null && waarde != null) {
                    string parsedKolomNaam = Regex.Replace(kolom, "[^a-zA-Z0-9]", String.Empty);
                    zoekAppendix = $"WHERE {parsedKolomNaam}=@waarde";
                }

                cmd.CommandText = $"SELECT " +
                                  "a.Id AS BestuurderAdresId, " +
                                  "a.Straatnaam AS BestuurderAdresStraatnaam, " +
                                  "a.Huisnummer AS BestuurderAdresHuisnummer, " +
                                  "a.Postcode AS BestuurderAdresPostcode, " +
                                  "a.Plaatsnaam AS BestuurderAdresPlaatsnaam, " +
                                  "a.Provincie AS BestuurderAdresProvincie, " +
                                  "a.Land AS BestuurderAdresLand " +
                                  $"FROM Adres AS a { zoekAppendix} ;";

                SqlDataReader r = cmd.ExecuteReader();
                List<Adres> resultaten = new();

                while (r.Read()) {
                    resultaten.Add(QueryParser.ParseReaderNaarAdres(r));
                }

                return resultaten;
            } catch (Exception e) {
                throw new BestuurderOpslagException("Onverwachte fout tijdens het vergaren van adressen.", e);
            } finally {
                _conn.Close();
            }
        }

        public void UpdateAdres(Adres adres) {
            try {
                _conn.Open();
                SqlCommand cmd = _conn.CreateCommand();

                cmd.CommandText = "UPDATE Adres " +
                                  "SET Straatnaam=@strt , Huisnummer=@hn , Postcode=@pc , " +
                                  "Plaatsnaam=@pn , Provincie=@prov , Land=@land " +
                                  "WHERE Id=@adresid ;";
                cmd.Parameters.Add(new SqlParameter("@strt", DbType.String));
                cmd.Parameters.Add(new SqlParameter("@hn", DbType.String));
                cmd.Parameters.Add(new SqlParameter("@pc", DbType.String));
                cmd.Parameters.Add(new SqlParameter("@pn", DbType.String));
                cmd.Parameters.Add(new SqlParameter("@prov", DbType.String));
                cmd.Parameters.Add(new SqlParameter("@land", DbType.String));
                cmd.Parameters.Add(new SqlParameter("@adresid", DbType.Int32));
                cmd.Parameters["@strt"].Value = adres.Straatnaam;
                cmd.Parameters["@hn"].Value = adres.Huisnummer;
                cmd.Parameters["@pc"].Value = adres.Postcode;
                cmd.Parameters["@pn"].Value = adres.Plaatsnaam;
                cmd.Parameters["@prov"].Value = adres.Provincie;
                cmd.Parameters["@land"].Value = adres.Land;
                cmd.Parameters["@adresid"].Value = adres.Id;

                cmd.ExecuteNonQuery();
            } catch (Exception e) {
                throw new BestuurderOpslagException("Kon adres niet bijwerken.", e);
            } finally {
                _conn.Close();
            }
        }

        public void VerwijderAdres(int id) {
			try {
                _conn.Open();
                SqlCommand cmd = _conn.CreateCommand();
                cmd.CommandText = "DELETE FROM Adres WHERE Id=@id ;";
                cmd.Parameters.Add(new SqlParameter("@id", DbType.Int32));
                cmd.Parameters["@id"].Value = id;
                cmd.ExecuteNonQuery();
            } catch(Exception e) {
                throw new BestuurderOpslagException("Kon adres niet verwijderen.", e);
			} finally {
                _conn.Close();
            }

            throw new NotImplementedException();
		}

        // Bestuurder 
        private void _behandelAdres(ref Bestuurder bestuurder) {
            if (bestuurder.Adres is not null) {
                Adres AdresInDb = bestuurder.Adres.Id is not null && bestuurder.Adres.Id > 0
                                  ? this.GeefAdressen("Id", bestuurder.Adres.Id).DefaultIfEmpty(null).First()
                                  : null;

                if (AdresInDb is null) {
                    bestuurder.Adres.ZetId(this.VoegAdresToe(bestuurder.Adres));
                } else {
                    if (JsonConvert.SerializeObject(AdresInDb) != JsonConvert.SerializeObject(bestuurder.Adres)) {
                        this.UpdateAdres(bestuurder.Adres);
                    }
                }
            }
        }

        // -- Create
        public int VoegBestuurderToe(Bestuurder bestuurder) {
            try {
                // Aangezien adres mogelijk in bestuurder toevoegen window komt zijn we wel geinteresseerd in 
                // eventueel bestaan, content verschillen, ..
                _behandelAdres(ref bestuurder);
            } catch (Exception e) {
                throw new BestuurderOpslagException("Adres kon niet verwerkt worden.", e);
			}

            // Verder zijn we louter geinteresseerd in de tankkaart / voertuig id's, eventuele eigenschap mismatch
            // met hetgeen in de databank zit kijken we niet naar, louter geinteresseerd in "link leggen met"
            // (kan normaal ook niet binnen wpf context voeg bestuurder toe)

            try {
                _conn.Open();
                SqlCommand cmd = _conn.CreateCommand();

                cmd.CommandText = "INSERT INTO Bestuurder " +
                    "(Naam, Voornaam, Geboortedatum, Rijksregisternummer, Rijbewijssoort, AdresId, VoertuigId, TankkaartId) " +
                    "OUTPUT INSERTED.Id " +
                    "VALUES (@naam, @voornaam, @gebdatum, @rrn, @rijbsoort, @adresid, @voertid, @tankid) ;";
                cmd.Parameters.Add(new SqlParameter("@naam", DbType.String));
                cmd.Parameters.Add(new SqlParameter("@voornaam", DbType.String));
                cmd.Parameters.Add(new SqlParameter("@gebdatum", DbType.DateTime));
                cmd.Parameters.Add(new SqlParameter("@rrn", DbType.String));
                cmd.Parameters.Add(new SqlParameter("@rijbsoort", DbType.String));
                cmd.Parameters.Add(new SqlParameter("@adresid", DbType.Int32));
                cmd.Parameters.Add(new SqlParameter("@voertid", DbType.Int32));
                cmd.Parameters.Add(new SqlParameter("@tankid", DbType.Int32));
                cmd.Parameters["@naam"].Value = bestuurder.Naam;
                cmd.Parameters["@voornaam"].Value = bestuurder.Voornaam;
                cmd.Parameters["@gebdatum"].Value = bestuurder.GeboorteDatum;
                cmd.Parameters["@rrn"].Value = bestuurder.RijksRegisterNummer;
                cmd.Parameters["@rijbsoort"].Value = bestuurder.RijbewijsSoort.ToString();
                cmd.Parameters["@adresid"].Value = bestuurder.Adres?.Id is null ? DBNull.Value : bestuurder.Adres.Id;
                cmd.Parameters["@voertid"].Value = bestuurder.Voertuig?.Id is null ? DBNull.Value : bestuurder.Voertuig.Id;
                cmd.Parameters["@tankid"].Value = bestuurder.Tankkaart?.Id is null ? DBNull.Value : bestuurder.Tankkaart.Id;

                return Convert.ToInt32(cmd.ExecuteScalar());
            } catch (Exception e) {
                throw new BestuurderManagerException("Er vond een onverwachte fout plaats. Het adres werd eventueel wel toegevoegd of geupdate.", e);
			} finally {
                _conn.Close();
            }
        }

        // -- Read
        public List<Bestuurder> GeefBestuurders(string? kolom=null, object? waarde=null) {
			try {
                _conn.Open();
                SqlCommand cmd = _conn.CreateCommand();

                string zoekAppendix = "";

                if (kolom != null && waarde != null) {
                    string parsedKolomNaam = Regex.Replace(kolom, "[^a-zA-Z0-9]", String.Empty);
                    zoekAppendix = $"WHERE b.{parsedKolomNaam}=@waarde";
                }

                cmd.CommandText = "SELECT " +
                                  "t.Id AS TankkaartId, " +
                                  "t.Kaartnummer AS TankkaartKaartnummer, " +
                                  "t.Pincode AS TankkaartPincode, " +
                                  "t.Vervaldatum AS TankkaartVervalDatum, " +
                                  "tb.Brandstof AS Brandstof, " +
                                  "b.Id AS BestuurderId, " +
                                  "b.Naam AS BestuurderNaam, " +
                                  "b.Voornaam AS BestuurderVoornaam, " +
                                  "b.Geboortedatum AS BestuurderGeboortedatum, " +
                                  "b.AdresId AS BestuurderAdresId, " +
                                  "b.Rijksregisternummer AS BestuurderRijksregisternummer, " +
                                  "b.Rijbewijssoort AS BestuurderRijbewijssoort, " +
                                  "b.VoertuigId AS BestuurderVoertuigId, " +
                                  "a.Straatnaam AS BestuurderAdresStraatnaam, " +
                                  "a.Huisnummer AS BestuurderAdresHuisnummer, " +
                                  "a.Postcode AS BestuurderAdresPostcode " +
                                  "a.Plaatsnaam AS BestuurderAdresPlaatsnaam, " +
                                  "a.Provincie AS BestuurderAdresProvincie, " +
                                  "a.Land AS BestuurderAdresLand, " +
                                  "v.Merk AS VoertuigMerk, " +
                                  "v.Model AS VoertuigModel, " +
                                  "v.Nummerplaat AS VoertuigNummerplaat, " +
                                  "v.Chasisnummer AS VoertuigChasisnummer, " +
                                  "v.Brandstof AS VoertuigBrandstof, " +
                                  "v.Type AS VoertuigType, " +
                                  "v.Kleur AS VoertuigKleur, " +
                                  "v.AantalDeuren AS VoertuigAantalDeuren, " +
                                  "v.Type AS VoertuigSoort " +
                                  "FROM Bestuurder AS b " +
                                  "LEFT JOIN Voertuig AS v " +
                                  "ON(b.VoertuigId = v.Id) " +
                                  "LEFT JOIN Tankkaart AS t " +
                                  "ON(t.Id = b.TankkaartId) " +
                                  "LEFT JOIN TankkaartBrandstof AS tb " +
                                  "ON(tb.TankkaartId = t.Id) " +
                                  "LEFT JOIN Adres AS a " +
                                  $"ON(b.AdresId = a.Id) {zoekAppendix} ;";

                if (zoekAppendix.Length > 0) {
                    cmd.Parameters.Add(new SqlParameter("@waarde", TypeConverteerder.GeefDbType(waarde.GetType())));
                    cmd.Parameters["@waarde"].Value = waarde is null ? DBNull.Value : waarde;
                }

                SqlDataReader r = cmd.ExecuteReader();
                List<Bestuurder> resultaten = new();

                while (r.Read()) {
                    Bestuurder huidigeBestuurder = QueryParser.ParseReaderNaarBestuurder(r);

                    if (resultaten.Any(b => b.Id == huidigeBestuurder.Id)) {
                        huidigeBestuurder = resultaten.First(b => b.Id == huidigeBestuurder.Id);
                    } else {
                        resultaten.Add(huidigeBestuurder);
                    }

                    if(huidigeBestuurder.Voertuig is null) {
                        huidigeBestuurder.ZetVoertuig(
                            QueryParser.ParseReaderNaarVoertuig(r, huidigeBestuurder)
                        );
					}

                    if(huidigeBestuurder.Tankkaart is null) {
                        huidigeBestuurder.ZetTankkaart(
                            QueryParser.ParseReaderNaarTankkaart(r, null, huidigeBestuurder)
                        );
					}

                    if(huidigeBestuurder.Adres is null) {
                        huidigeBestuurder.ZetAdres(
                            QueryParser.ParseReaderNaarAdres(r)
                        );
					}

                    if (!r.IsDBNull(r.GetOrdinal("Brandstof")) && huidigeBestuurder.Tankkaart is not null) {
                        TankkaartBrandstof b = QueryParser.ParseReaderNaarTankkaartBrandstof(r);
                        if (!huidigeBestuurder.Tankkaart.GeldigVoorBrandstoffen.Contains(b)) {
                            huidigeBestuurder.Tankkaart.VoegBrandstofToe(b);
                        }
                    }

                }

                return resultaten;
            } catch (SqlException ex) when (ex.Number == 207) {
                throw new BestuurderOpslagException("Er werd een ongeldige kolomnaam opgegeven.", ex);
            } catch (Exception ex) {
                throw new BestuurderOpslagException("Er is een onverwachte fout opgetreden.", ex);
            } finally {
                _conn.Close();
            }
        }

        public Bestuurder GeefBestuurderDetail(int id) {
            return this.GeefBestuurders("Id", id).DefaultIfEmpty(null).First();
        }

        // -- Update
        public void UpdateBestuurder(Bestuurder bestuurder) {
            // Zelfde principe als VoegToe, we behandelen louter adres eigenschapsgewijs, de rest wordt behandelt
            // op basis van het id (fk)
            try {
                _behandelAdres(ref bestuurder);
            } catch (Exception e) {
                throw new BestuurderOpslagException("Adres kon niet verwerkt worden.", e);
            }

            try {
                _conn.Open();
                SqlCommand cmd = _conn.CreateCommand();

                cmd.CommandText = "UPDATE Bestuurder SET " +
                                  "Naam=@naam , Voornaam=@voornaam , Geboortedatum=@gebdatum , " +
                                  "Rijksregisternummer=@rrn , Rijbewijssoort=@rijbsoort , " +
                                  "AdresId=@adresid , VoertuigId=@voertid , TankkaartId=@tankid " +
                                  "WHERE Id=@bestuurderid ;";

                cmd.Parameters.Add(new SqlParameter("@naam", DbType.String));
                cmd.Parameters.Add(new SqlParameter("@voornaam", DbType.String));
                cmd.Parameters.Add(new SqlParameter("@gebdatum", DbType.DateTime));
                cmd.Parameters.Add(new SqlParameter("@rrn", DbType.String));
                cmd.Parameters.Add(new SqlParameter("@rijbsoort", DbType.String));
                cmd.Parameters.Add(new SqlParameter("@adresid", DbType.Int32));
                cmd.Parameters.Add(new SqlParameter("@voertid", DbType.Int32));
                cmd.Parameters.Add(new SqlParameter("@tankid", DbType.Int32));
                cmd.Parameters.Add(new SqlParameter("@bestuurderid", DbType.Int32));
                cmd.Parameters["@naam"].Value = bestuurder.Naam;
                cmd.Parameters["@voornaam"].Value = bestuurder.Voornaam;
                cmd.Parameters["@gebdatum"].Value = bestuurder.GeboorteDatum;
                cmd.Parameters["@rrn"].Value = bestuurder.RijksRegisterNummer;
                cmd.Parameters["@rijbsoort"].Value = bestuurder.RijbewijsSoort.ToString();
                cmd.Parameters["@adresid"].Value = bestuurder.Adres?.Id is null ? DBNull.Value : bestuurder.Adres.Id;
                cmd.Parameters["@voertid"].Value = bestuurder.Voertuig?.Id is null ? DBNull.Value : bestuurder.Voertuig.Id;
                cmd.Parameters["@tankid"].Value = bestuurder.Tankkaart?.Id is null ? DBNull.Value : bestuurder.Tankkaart.Id;
                cmd.Parameters["@bestuurderid"].Value = bestuurder.Id;

                cmd.ExecuteNonQuery();
            } catch (Exception e) {
                throw new BestuurderOpslagException("Er is een onverwachte fout opgetreden.", e);
            } finally {
                _conn.Close();
            }
        }

        // -- Delete
        public void VerwijderBestuurder(int id) {
            try {
                _conn.Open();
                SqlCommand cmd = _conn.CreateCommand();
                cmd.CommandText = "DELETE FROM Bestuurder WHERE Id=@id ;";
                cmd.Parameters.Add(new SqlParameter("@id", DbType.Int32));
                cmd.Parameters["@id"].Value = id;
                cmd.ExecuteNonQuery();

            } catch (Exception e) {
                throw new BestuurderOpslagException("Er is een onverwachte fout opgetreden.",e);
            } finally {
                _conn.Close();
            }
        }


	}
} 
