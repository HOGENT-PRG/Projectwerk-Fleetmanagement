using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessLaag.Model;
using BusinessLaag.Exceptions;
using BusinessLaag.Interfaces;
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

                if (zoekAppendix.Length > 0) {
                    cmd.Parameters.Add(new SqlParameter("@waarde", TypeConverteerder.GeefDbType(waarde.GetType())));
                    cmd.Parameters["@waarde"].Value = waarde is null ? DBNull.Value : waarde;
                }

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

        public List<Adres> ZoekAdressen(List<string> kolomnamen, List<object> zoektermen, bool likeWildcard = false) {
            try {
                if (kolomnamen.Count == 0 || zoektermen.Count == 0
                   || zoektermen.Count != kolomnamen.Count
                   || zoektermen.Distinct().Count() != zoektermen.Count) {
                    throw new BestuurderOpslagException("U heeft geen kolomnamen opgegeven of de hoeveelheid komt niet overeen met de zoektermen. Kolomnamen moeten tevens uniek zijn.");
                }

                _conn.Open();
                SqlCommand cmd = _conn.CreateCommand();

                Regex re = new Regex("[^a-zA-Z0-9]");
                List<string> parsed_kolomnamen = new List<string>();
                kolomnamen.ForEach(x => parsed_kolomnamen.Add(re.Replace(x, "")));

                // opbouw
                StringBuilder zoekAppendix = new($" WHERE ");
                foreach (string k in parsed_kolomnamen) {
                    if (likeWildcard) {
                        zoekAppendix.Append($"a.{k} LIKE @{k.ToLower()}");
                    } else {
                        zoekAppendix.Append($"a.{k}=@{k.ToLower()}");
                    }
                    if (parsed_kolomnamen.IndexOf(k) < parsed_kolomnamen.Count - 1) {
                        zoekAppendix.Append(" AND ");
                    }
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

                foreach (object o in zoektermen) {
                    int i = zoektermen.IndexOf(o);
                    string corresp_col = parsed_kolomnamen[i].ToLower();
                    cmd.Parameters.Add(new SqlParameter($"@{corresp_col}", TypeConverteerder.GeefDbType(o.GetType())));
                    if (likeWildcard) {
                        cmd.Parameters[$"@{corresp_col}"].Value = "%" + o + "%";
                    } else {
                        cmd.Parameters[$"@{corresp_col}"].Value = o is null ? DBNull.Value : o;
                    }
                }

                SqlDataReader r = cmd.ExecuteReader();
                List<Adres> resultaten = new();

                while (r.Read()) {
                    resultaten.Add(QueryParser.ParseReaderNaarAdres(r));
                }

                return resultaten;
            } catch (Exception e) {
                throw new BestuurderOpslagException("Onverwachte fout tijdens het zoeken van adressen.", e);
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
            _conn.Open();
            SqlTransaction tx = _conn.BeginTransaction();

            try {
                SqlCommand cmd_best_rel = _conn.CreateCommand();
                cmd_best_rel.Transaction = tx;
                cmd_best_rel.CommandText = "UPDATE Bestuurder SET AdresId=@addrnull WHERE AdresId=@addrid ;";
                cmd_best_rel.Parameters.Add(new SqlParameter("@addrnull", DbType.Int32));
                cmd_best_rel.Parameters.Add(new SqlParameter("@addrid", DbType.Int32));
                cmd_best_rel.Parameters["@addrnull"].Value = DBNull.Value;
                cmd_best_rel.Parameters["@addrid"].Value = id;
                cmd_best_rel.ExecuteNonQuery();


                SqlCommand cmd = _conn.CreateCommand();
                cmd.Transaction = tx;
                cmd.CommandText = "DELETE FROM Adres WHERE Id=@id ;";
                cmd.Parameters.Add(new SqlParameter("@id", DbType.Int32));
                cmd.Parameters["@id"].Value = id;
                cmd.ExecuteNonQuery();

                tx.Commit();
            } catch(Exception ex) {
                try {
                    tx.Rollback();
                } catch (InvalidOperationException e) { /* Error vond plaats voor de commit, exception negeren */
                } catch (Exception e) {
                    throw new BestuurderOpslagException("Rollback gaf een onverwachte foutmelding.", e);
                }

                throw new BestuurderOpslagException("Kon adres niet verwijderen.", ex);
			} finally {
                _conn.Close();
            }
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
                                  "t.IsGeblokkeerd AS TankkaartIsGeblokkeerd, " +
                                  "t.Vervaldatum AS TankkaartVervalDatum, " +
                                  "tb.Brandstof AS TankkaartBrandstof, " +
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
                                  "a.Postcode AS BestuurderAdresPostcode, " +
                                  "a.Plaatsnaam AS BestuurderAdresPlaatsnaam, " +
                                  "a.Provincie AS BestuurderAdresProvincie, " +
                                  "a.Land AS BestuurderAdresLand, " +
                                  "v.Id AS VoertuigId, " +
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

                    if (!r.IsDBNull(r.GetOrdinal("TankkaartBrandstof")) && huidigeBestuurder.Tankkaart is not null) {
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

        public List<Bestuurder> ZoekBestuurders(List<string> kolomnamen, List<object> zoektermen, bool likeWildcard = false) {
            try {
                if (kolomnamen.Count == 0 || zoektermen.Count == 0
                   || zoektermen.Count != kolomnamen.Count
                   || zoektermen.Distinct().Count() != zoektermen.Count) {
                    throw new BestuurderOpslagException("U heeft geen kolomnamen opgegeven of de hoeveelheid komt niet overeen met de zoektermen. Kolomnamen moeten tevens uniek zijn.");
                }

                _conn.Open();
                SqlCommand cmd = _conn.CreateCommand();

                Regex re = new Regex("[^a-zA-Z0-9]");
                List<string> parsed_kolomnamen = new List<string>();
                kolomnamen.ForEach(x => parsed_kolomnamen.Add(re.Replace(x, "")));

                // opbouw
                StringBuilder zoekAppendix = new($" WHERE ");
                foreach (string k in parsed_kolomnamen) {
                    if (likeWildcard) {
                        zoekAppendix.Append($"b.{k} LIKE @{k.ToLower()}");
                    } else {
                        zoekAppendix.Append($"b.{k}=@{k.ToLower()}");
                    }
                    if (parsed_kolomnamen.IndexOf(k) < parsed_kolomnamen.Count - 1) {
                        zoekAppendix.Append(" AND ");
                    }
                }

                cmd.CommandText = "SELECT " +
                                  "t.Id AS TankkaartId, " +
                                  "t.Kaartnummer AS TankkaartKaartnummer, " +
                                  "t.Pincode AS TankkaartPincode, " +
                                  "t.IsGeblokkeerd AS TankkaartIsGeblokkeerd, " +
                                  "t.Vervaldatum AS TankkaartVervalDatum, " +
                                  "tb.Brandstof AS TankkaartBrandstof, " +
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
                                  "a.Postcode AS BestuurderAdresPostcode, " +
                                  "a.Plaatsnaam AS BestuurderAdresPlaatsnaam, " +
                                  "a.Provincie AS BestuurderAdresProvincie, " +
                                  "a.Land AS BestuurderAdresLand, " +
                                  "v.Id AS VoertuigId, " +
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

                foreach (object o in zoektermen) {
                    int i = zoektermen.IndexOf(o);
                    string corresp_col = parsed_kolomnamen[i].ToLower();
                    cmd.Parameters.Add(new SqlParameter($"@{corresp_col}", TypeConverteerder.GeefDbType(o.GetType())));
                    if (likeWildcard) {
                        cmd.Parameters[$"@{corresp_col}"].Value = "%" + o + "%";
                    } else {
                        cmd.Parameters[$"@{corresp_col}"].Value = o is null ? DBNull.Value : o;
                    }
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

                    if (huidigeBestuurder.Voertuig is null) {
                        huidigeBestuurder.ZetVoertuig(
                            QueryParser.ParseReaderNaarVoertuig(r, huidigeBestuurder)
                        );
                    }

                    if (huidigeBestuurder.Tankkaart is null) {
                        huidigeBestuurder.ZetTankkaart(
                            QueryParser.ParseReaderNaarTankkaart(r, null, huidigeBestuurder)
                        );
                    }

                    if (huidigeBestuurder.Adres is null) {
                        huidigeBestuurder.ZetAdres(
                            QueryParser.ParseReaderNaarAdres(r)
                        );
                    }

                    if (!r.IsDBNull(r.GetOrdinal("TankkaartBrandstof")) && huidigeBestuurder.Tankkaart is not null) {
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
