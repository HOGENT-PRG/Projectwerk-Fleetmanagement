using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using BusinessLaag.Model;
using BusinessLaag.Model.Enum;
using BusinessLaag.Interfaces;
using BusinessLaag.Exceptions;
using System.Text.RegularExpressions;
using DataLaag.Helpers;

namespace DataLaag.Repositories
{
	public sealed class TankkaartOpslag : ITankkaartOpslag {
		private SqlConnection _connector { get; set; }

		public void ZetConnectionString(string connString) {
			_connector = connString.Length > 5 ? new SqlConnection(connString) : throw new TankkaartOpslagException("Connectiestring moet langer zijn dan 5 karakters.");
		}

		// -- Create
		public int VoegTankkaartToe(Tankkaart tankkaart) {
			_connector.Open();
			SqlTransaction tx = _connector.BeginTransaction();

			try {
				SqlCommand command = _connector.CreateCommand();
				command.Transaction = tx;

				command.CommandText = "INSERT INTO Tankkaart (IsGeblokkeerd, Kaartnummer, Vervaldatum, Pincode) " +
									  "OUTPUT INSERTED.Id " +
									  "VALUES(@isgeblokkeerd, @kaartnummer, @vervaldatum, @pincode ) ;";
				command.Parameters.Add(new SqlParameter("@isgeblokkeerd", DbType.Boolean));
				command.Parameters.Add(new SqlParameter("@kaartnummer", DbType.String));
				command.Parameters.Add(new SqlParameter("@vervaldatum", DbType.DateTime));
				command.Parameters.Add(new SqlParameter("@pincode", DbType.String));
				command.Parameters["@isgeblokkeerd"].Value = tankkaart.IsGeblokkeerd;
				command.Parameters["@kaartnummer"].Value = tankkaart.Kaartnummer;
				command.Parameters["@vervaldatum"].Value = tankkaart.Vervaldatum;
				command.Parameters["@pincode"].Value = tankkaart.Pincode;

				tankkaart.ZetId(Convert.ToInt32(command.ExecuteScalar()));

				foreach (TankkaartBrandstof t in tankkaart.GeldigVoorBrandstoffen) {
					SqlCommand c = _connector.CreateCommand();
					c.Transaction = tx;
					c.CommandText = "INSERT INTO TankkaartBrandstof (TankkaartId, Brandstof) " +
									  "VALUES(@id, @brandstof) ;";
					c.Parameters.Add(new SqlParameter("@id", DbType.Int32));
					c.Parameters.Add(new SqlParameter("@brandstof", DbType.String));
					c.Parameters["@id"].Value = tankkaart.Id;
					c.Parameters["@brandstof"].Value = t.ToString();
					c.ExecuteNonQuery();
				}


				if (tankkaart.Bestuurder?.Id is not null) {
					SqlCommand c = _connector.CreateCommand();
					c.Transaction = tx;
					c.CommandText = "UPDATE Bestuurder SET TankkaartId=@tankid WHERE Id=@bestuurderid ;";

					c.Parameters.Add(new SqlParameter("@tankid", DbType.Int32));
					c.Parameters.Add(new SqlParameter("@bestuurderid", DbType.Int32));
					c.Parameters["@tankid"].Value = tankkaart.Id;
					c.Parameters["@bestuurderid"].Value = tankkaart.Bestuurder.Id;

					c.ExecuteNonQuery();
				}

				tx.Commit();

				return (int)tankkaart.Id;
			} catch (Exception ex) {
				try {
					tx.Rollback();
				} catch (InvalidOperationException) { /* Error vond plaats voor de commit, exception negeren */
				} catch (Exception e) {
					throw new TankkaartOpslagException("Rollback gaf een onverwachte foutmelding.", e);
				}

				throw new TankkaartOpslagException("Er is een onverwachte fout opgetreden.", ex);
			} finally {
				_connector.Close();
			}
		}

		// -- Read
		public List<Tankkaart> GeefTankkaarten(string? kolomnaam = null, object? waarde = null) {
			try {
				_connector.Open();

				string zoekAppendix = "";

				if (kolomnaam != null && waarde != null) {
					string parsedKolomNaam = Regex.Replace(kolomnaam, "[^a-zA-Z0-9]", String.Empty);
					zoekAppendix = $"WHERE t.{parsedKolomNaam}=@waarde";
				}

				SqlCommand cmd = _connector.CreateCommand();
				cmd.CommandText = "SELECT t.Id AS TankkaartId," +
								  "t.Kaartnummer AS TankkaartKaartnummer," +
								  "t.Pincode AS TankkaartPincode," +
								  "t.IsGeblokkeerd AS TankkaartIsGeblokkeerd, " +
								  "t.Vervaldatum AS TankkaartVervalDatum," +
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
								  "FROM Tankkaart AS t " +
								  "LEFT JOIN TankkaartBrandstof AS tb " +
								  "ON(tb.TankkaartId = t.Id) " +
								  "LEFT JOIN Bestuurder AS b " +
								  "ON(b.TankkaartId = t.Id) " +
								  "LEFT JOIN Adres AS a " +
								  "ON(b.AdresId = a.Id) " +
								  "LEFT JOIN Voertuig AS v " +
								  "ON(b.VoertuigId = v.Id) " +
								  $" {zoekAppendix} ;";

				if (zoekAppendix.Length > 0) {
					cmd.Parameters.Add(new SqlParameter("@waarde", TypeConverteerder.GeefDbType(waarde.GetType())));
					cmd.Parameters["@waarde"].Value = waarde is null ? DBNull.Value : waarde;
				}

				SqlDataReader r = cmd.ExecuteReader();
				List<Tankkaart> resultaten = new();

				while (r.Read()) {
					// deze parsen we zonder meegeven brandstoflijst en bestuurder, die stellen we later in
					Tankkaart huidigeTankkaart = QueryParser.ParseReaderNaarTankkaart(r);

					if(huidigeTankkaart is null) {
						// De QueryParser retourneert null indien de vervaldatum reeds bereikt werd en aanmaken van een geldig Tankkaart object niet meer mogelijk is
						continue;
					}

					if (resultaten.Any(t => t.Id == huidigeTankkaart.Id)) {
						huidigeTankkaart = resultaten.First(t => t.Id == huidigeTankkaart.Id);
					} else {
						resultaten.Add(huidigeTankkaart);
					}

					if (!r.IsDBNull(r.GetOrdinal("BestuurderId")) && huidigeTankkaart.Bestuurder is null) {
						// tankaart bestuurder instellen
						huidigeTankkaart.ZetBestuurder(
							// bestuurder maken zonder voertuig want voertuig parser ontvangt een bestuurder
							QueryParser.ParseReaderNaarBestuurder(
								r, huidigeTankkaart, null, QueryParser.ParseReaderNaarAdres(r)
							)
						);

						// nadat bestuurder gemaakt is, voertuig maken met
						// bestuurder en instellen als voertuig van de bestuurder
						huidigeTankkaart.Bestuurder.ZetVoertuig(
							QueryParser.ParseReaderNaarVoertuig(r, huidigeTankkaart.Bestuurder)
						);
					}

					// tankkaart brandstoffen instellen
					if (!r.IsDBNull(r.GetOrdinal("TankkaartBrandstof"))) {
						TankkaartBrandstof b = QueryParser.ParseReaderNaarTankkaartBrandstof(r);
						if (!huidigeTankkaart.GeldigVoorBrandstoffen.Contains(b)) {
							huidigeTankkaart.VoegBrandstofToe(b);
						}
					}

				}

				return resultaten;
			} catch (SqlException ex) when (ex.Number == 207) {
				throw new TankkaartOpslagException("Er werd een ongeldige kolomnaam opgegeven.", ex);
			} catch (Exception ex) {
				throw new TankkaartOpslagException("Er is een onverwachte fout opgetreden.", ex);
			} finally {
				_connector.Close();
			}
		}

		public List<Tankkaart> ZoekTankkaarten(List<string> kolomnamen, List<object> zoektermen, bool likeWildcard = false) {
			try {
				if (kolomnamen.Count == 0 || zoektermen.Count == 0
				   || zoektermen.Count != kolomnamen.Count
				   || zoektermen.Distinct().Count() != zoektermen.Count) {
					throw new TankkaartOpslagException("U heeft geen kolomnamen opgegeven of de hoeveelheid komt niet overeen met de zoektermen. Kolomnamen moeten tevens uniek zijn.");
				}

				_connector.Open();
				SqlCommand cmd = _connector.CreateCommand();

				Regex re = new Regex("[^a-zA-Z0-9]");
				List<string> parsed_kolomnamen = new List<string>();
				kolomnamen.ForEach(x => parsed_kolomnamen.Add(re.Replace(x, "")));

				// opbouw
				StringBuilder zoekAppendix = new($" WHERE ");
				foreach (string k in parsed_kolomnamen) {
					if (likeWildcard) {
						zoekAppendix.Append($"t.{k} LIKE @{k.ToLower()}");
					} else {
						zoekAppendix.Append($"t.{k}=@{k.ToLower()}");
					}
					if (parsed_kolomnamen.IndexOf(k) < parsed_kolomnamen.Count - 1) {
						zoekAppendix.Append(" AND ");
					}
				}


				cmd.CommandText = "SELECT t.Id AS TankkaartId," +
								  "t.Kaartnummer AS TankkaartKaartnummer," +
								  "t.Pincode AS TankkaartPincode," +
								  "t.IsGeblokkeerd AS TankkaartIsGeblokkeerd, " +
								  "t.Vervaldatum AS TankkaartVervalDatum," +
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
								  "FROM Tankkaart AS t " +
								  "LEFT JOIN TankkaartBrandstof AS tb " +
								  "ON(tb.TankkaartId = t.Id) " +
								  "LEFT JOIN Bestuurder AS b " +
								  "ON(b.TankkaartId = t.Id) " +
								  "LEFT JOIN Adres AS a " +
								  "ON(b.AdresId = a.Id) " +
								  "LEFT JOIN Voertuig AS v " +
								  "ON(b.VoertuigId = v.Id) " +
								  $" {zoekAppendix} ;";

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
				List<Tankkaart> resultaten = new();

				while (r.Read()) {
					// deze parsen we zonder meegeven brandstoflijst en bestuurder, die stellen we later in
					Tankkaart huidigeTankkaart = QueryParser.ParseReaderNaarTankkaart(r);

					if (huidigeTankkaart is null) {
						// De QueryParser retourneert null indien de vervaldatum reeds bereikt werd en aanmaken van een geldig Tankkaart object niet meer mogelijk is
						continue;
					}

					if (resultaten.Any(t => t.Id == huidigeTankkaart.Id)) {
						huidigeTankkaart = resultaten.First(t => t.Id == huidigeTankkaart.Id);
					} else {
						resultaten.Add(huidigeTankkaart);
					}

					if (!r.IsDBNull(r.GetOrdinal("BestuurderId")) && huidigeTankkaart.Bestuurder is null) {
						// tankaart bestuurder instellen
						huidigeTankkaart.ZetBestuurder(
							// bestuurder maken zonder voertuig want voertuig parser ontvangt een bestuurder
							QueryParser.ParseReaderNaarBestuurder(
								r, huidigeTankkaart, null, QueryParser.ParseReaderNaarAdres(r)
							)
						);

						// nadat bestuurder gemaakt is, voertuig maken met
						// bestuurder en instellen als voertuig van de bestuurder
						huidigeTankkaart.Bestuurder.ZetVoertuig(
							QueryParser.ParseReaderNaarVoertuig(r, huidigeTankkaart.Bestuurder)
						);
					}

					// tankkaart brandstoffen instellen
					if (!r.IsDBNull(r.GetOrdinal("TankkaartBrandstof"))) {
						TankkaartBrandstof b = QueryParser.ParseReaderNaarTankkaartBrandstof(r);
						if (!huidigeTankkaart.GeldigVoorBrandstoffen.Contains(b)) {
							huidigeTankkaart.VoegBrandstofToe(b);
						}
					}

				}

				return resultaten;
			} catch (SqlException ex) when (ex.Number == 207) {
				throw new TankkaartOpslagException("Er werd een ongeldige kolomnaam opgegeven.", ex);
			} catch (Exception ex) {
				throw new TankkaartOpslagException("Er is een onverwachte fout opgetreden.", ex);
			} finally {
				_connector.Close();
			}
		}

		public Tankkaart GeefTankkaartDetail(int id) {
			return this.GeefTankkaarten("Id", id).DefaultIfEmpty(null).First();
		}

		public List<Tankkaart> ZoekTankkaarten(string kolom, object waarde) {
			return GeefTankkaarten(kolom, waarde);
		}


		// -- Update
		public void UpdateTankkaart(Tankkaart tankkaart) {
			// Dient te gebeuren alvorens de connectie hier geopend wordt (shared SqlConnection)
			Tankkaart BestaandeTankkaart = this.GeefTankkaartDetail((int)tankkaart.Id);
			List<TankkaartBrandstof> bestaandeBrandstoffen = this.GeefTankkaartDetail((int)tankkaart.Id).GeldigVoorBrandstoffen;

			_connector.Open();
			SqlTransaction transactie = _connector.BeginTransaction();
			try {
				SqlCommand cmd = _connector.CreateCommand();
				cmd.Transaction = transactie;
				cmd.CommandText = "UPDATE Tankkaart SET IsGeblokkeerd=@isgeblokkeerd,  Kaartnummer=@kaartnummer, Vervaldatum=@vervaldatum, Pincode=@pincode WHERE Id=@id ;";

				cmd.Parameters.Add(new SqlParameter("@id", DbType.Int32));
				cmd.Parameters.Add(new SqlParameter("@isgeblokkeerd", DbType.Boolean));
				cmd.Parameters.Add(new SqlParameter("@kaartnummer", DbType.String));
				cmd.Parameters.Add(new SqlParameter("@vervaldatum", DbType.DateTime));
				cmd.Parameters.Add(new SqlParameter("@pincode", DbType.String));
				cmd.Parameters["@id"].Value = tankkaart.Id;
				cmd.Parameters["@isgeblokkeerd"].Value = tankkaart.IsGeblokkeerd;
				cmd.Parameters["@kaartnummer"].Value = tankkaart.Kaartnummer;
				cmd.Parameters["@vervaldatum"].Value = tankkaart.Vervaldatum;
				cmd.Parameters["@pincode"].Value = (object)tankkaart.Pincode ?? DBNull.Value;

				cmd.ExecuteNonQuery();

				// Controle overeenkomst
				if (BestaandeTankkaart.Bestuurder?.Id != tankkaart.Bestuurder?.Id) {

					// Verwijder eventuele huidige relatie
					if (BestaandeTankkaart.Bestuurder?.Id is not null) {
						SqlCommand cmd_del = _connector.CreateCommand();
						cmd_del.Transaction = transactie;

						cmd_del.CommandText = "UPDATE Bestuurder SET TankkaartId=@tid WHERE Id=@bid ;";
						cmd_del.Parameters.Add(new SqlParameter("@tid", DbType.Int32));
						cmd_del.Parameters.Add(new SqlParameter("@bid", DbType.Int32));
						cmd_del.Parameters["@tid"].Value = DBNull.Value;
						cmd_del.Parameters["@bid"].Value = BestaandeTankkaart.Bestuurder.Id;
						cmd_del.ExecuteNonQuery();
					}

					// Stel eventuele nieuwe relatie in
					if (tankkaart.Bestuurder?.Id is not null) {
						SqlCommand cmd_add = _connector.CreateCommand();
						cmd_add.Transaction = transactie;

						cmd_add.CommandText = "UPDATE Bestuurder SET TankkaartId=@tid WHERE Id=@bid ;";
						cmd_add.Parameters.Add(new SqlParameter("@tid", DbType.Int32));
						cmd_add.Parameters.Add(new SqlParameter("@bid", DbType.Int32));
						cmd_add.Parameters["@tid"].Value = tankkaart.Id;
						cmd_add.Parameters["@bid"].Value = tankkaart.Bestuurder.Id;
						cmd_add.ExecuteNonQuery();
					}
				}

				// We bepalen vervolgens of er wijzigingen zijn op het vlak van brandstoffen,
				// daarvoor gebruiken we de bestaande entries en vergelijken we om te weten te komen
				// welke verwijderd / toegevoegd dienen te worden.
				// Bestaande entries worden uiteraard genegeerd.

				foreach (TankkaartBrandstof bestaandeBrandstof in bestaandeBrandstoffen) {
					if (!tankkaart.GeldigVoorBrandstoffen.Contains(bestaandeBrandstof)) {
						SqlCommand c = _connector.CreateCommand();
						c.Transaction = transactie;
						c.CommandText = "DELETE FROM TankkaartBrandstof WHERE TankkaartId=@id AND Brandstof=@brandstofnaam ;";
						c.Parameters.Add(new SqlParameter("@id", DbType.Int32));
						c.Parameters.Add(new SqlParameter("@brandstofnaam", DbType.String));
						c.Parameters["@id"].Value = tankkaart.Id;
						c.Parameters["@brandstofnaam"].Value = bestaandeBrandstof.ToString();
						c.ExecuteNonQuery();
					}
				}

				foreach (TankkaartBrandstof potentieelNieuweBrandstof in tankkaart.GeldigVoorBrandstoffen) {
					if (!bestaandeBrandstoffen.Contains(potentieelNieuweBrandstof)) {
						SqlCommand c = _connector.CreateCommand();
						c.Transaction = transactie;
						c.CommandText = "INSERT INTO TankkaartBrandstof (TankkaartId, Brandstof) " +
										  "VALUES(@id, @brandstof) ;";
						c.Parameters.Add(new SqlParameter("@id", DbType.Int32));
						c.Parameters.Add(new SqlParameter("@brandstof", DbType.String));
						c.Parameters["@id"].Value = tankkaart.Id;
						c.Parameters["@brandstof"].Value = potentieelNieuweBrandstof.ToString();
						c.ExecuteNonQuery();
					}
				}

				transactie.Commit();
			} catch (Exception ex) {
				try {
					transactie.Rollback();
				} catch (InvalidOperationException) { /* Error vond plaats voor de commit, exception negeren */
				} catch (Exception e) {
					throw new TankkaartOpslagException("Rollback gaf een onverwachte foutmelding.", e);
				}

				throw new TankkaartOpslagException("Er is een onverwachte fout opgetreden.", ex);
			} finally {
				_connector.Close();
			}
		}

		// -- Delete
		public void VerwijderTankkaart(int id) {
			_connector.Open();
			SqlTransaction transactie = _connector.BeginTransaction();
			try {
				SqlCommand cmd_brandstoffen = _connector.CreateCommand();
				cmd_brandstoffen.Transaction = transactie;
				cmd_brandstoffen.CommandText = "DELETE FROM TankkaartBrandstof WHERE TankkaartId=@id ;";
				cmd_brandstoffen.Parameters.Add(new SqlParameter("@id", DbType.Int32));
				cmd_brandstoffen.Parameters["@id"].Value = id;
				cmd_brandstoffen.ExecuteNonQuery();

				SqlCommand cmd_relatie = _connector.CreateCommand();
				cmd_relatie.Transaction = transactie;
				cmd_relatie.CommandText = "UPDATE Bestuurder SET TankkaartId=@nieuw WHERE TankkaartId=@id ;";
				cmd_relatie.Parameters.Add(new SqlParameter("@id", DbType.Int32));
				cmd_relatie.Parameters.Add(new SqlParameter("@nieuw", DbType.Int32));
				cmd_relatie.Parameters["@id"].Value = id;
				cmd_relatie.Parameters["@nieuw"].Value = DBNull.Value;
				cmd_relatie.ExecuteNonQuery();

				SqlCommand command = _connector.CreateCommand();
				command.Transaction = transactie;

				command.CommandText = "DELETE FROM Tankkaart WHERE Id=@id ;";
				command.Parameters.Add(new SqlParameter("@id", DbType.Int32));
				command.Parameters["@id"].Value = id;
				command.ExecuteNonQuery();

				transactie.Commit();
			} catch (Exception ex) {
				try {
					transactie.Rollback();
				} catch (InvalidOperationException e) { /* Error vond plaats voor de commit, exception negeren */
				} catch (Exception e) {
					throw new TankkaartOpslagException("Rollback gaf een onverwachte foutmelding.", e);
				}

				throw new TankkaartOpslagException("Er is een onverwachte fout opgetreden.", ex);
			} finally {
				_connector.Close();
			}
		}

	}
}
