using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BusinessLaag.Model;
using BusinessLaag.Interfaces;
using BusinessLaag.Exceptions;
using BusinessLaag.Model.Enum;
using System.Text.RegularExpressions;
using DataLaag.Helpers;
using System.Linq;

namespace DataLaag.Repositories {
	public sealed class VoertuigOpslag : IVoertuigOpslag {
		private SqlConnection _conn { get; set; }

		public void ZetConnectionString(string connString) {
			_conn = connString.Length > 5 ? new SqlConnection(connString) : throw new VoertuigOpslagException("Connectiestring moet langer zijn dan 5 karakters.");
		}

		// -- Create
		public int VoegVoertuigToe(Voertuig voertuig) {
			_conn.Open();
			SqlTransaction tx = _conn.BeginTransaction();

			try {
				SqlCommand cmd = _conn.CreateCommand();
				cmd.Transaction = tx;

				cmd.CommandText = "INSERT INTO Voertuig (Merk, Model, Nummerplaat, Chasisnummer, Brandstof, Type, Kleur, AantalDeuren) OUTPUT INSERTED.Id VALUES (@merk, @model, @nummerplaat, @chassisnummer, @brandstof, @type, @kleur, @aantaldeuren) ;";
				cmd.Parameters.Add(new SqlParameter("@merk", DbType.String));
				cmd.Parameters.Add(new SqlParameter("@model", DbType.String));
				cmd.Parameters.Add(new SqlParameter("@nummerplaat", DbType.String));
				cmd.Parameters.Add(new SqlParameter("@chassisnummer", DbType.String));
				cmd.Parameters.Add(new SqlParameter("@brandstof", DbType.String));
				cmd.Parameters.Add(new SqlParameter("@type", DbType.String));
				cmd.Parameters.Add(new SqlParameter("@kleur", DbType.String));
				cmd.Parameters.Add(new SqlParameter("@aantaldeuren", DbType.String));
				cmd.Parameters["@merk"].Value = voertuig.Merk.ToString();
				cmd.Parameters["@model"].Value = voertuig.Model;
				cmd.Parameters["@nummerplaat"].Value = voertuig.Nummerplaat;
				cmd.Parameters["@chassisnummer"].Value = voertuig.Chassisnummer;
				cmd.Parameters["@brandstof"].Value = voertuig.Brandstof.ToString();
				cmd.Parameters["@type"].Value = voertuig.Voertuigsoort.ToString();
				cmd.Parameters["@kleur"].Value = (object)voertuig.Kleur ?? DBNull.Value;
				cmd.Parameters["@aantaldeuren"].Value = (object)voertuig.AantalDeuren ?? DBNull.Value;

				voertuig.ZetId(Convert.ToInt32(cmd.ExecuteScalar()));

				if (voertuig.Bestuurder?.Id is not null) {
					SqlCommand c = _conn.CreateCommand();
					c.Transaction = tx;
					c.CommandText = "UPDATE Bestuurder SET VoertuigId=@voertuigid WHERE Id=@bestuurderid ;";

					c.Parameters.Add(new SqlParameter("@voertuigid", DbType.Int32));
					c.Parameters.Add(new SqlParameter("@bestuurderid", DbType.Int32));
					c.Parameters["@voertuigid"].Value = voertuig.Id;
					c.Parameters["@bestuurderid"].Value = voertuig.Bestuurder.Id;

					c.ExecuteNonQuery();
				}

				tx.Commit();

				return (int)voertuig.Id;
			} catch (Exception ex) {
				try {
					tx.Rollback();
				} catch (InvalidOperationException) { /* Error vond plaats voor de commit, exception negeren */
				} catch (Exception e) {
					throw new VoertuigOpslagException("Rollback gaf een onverwachte foutmelding.", e);
				}

				throw new VoertuigOpslagException("Onverwachte fout.", ex);
			} finally {
				_conn.Close();
			}
		}

		// -- Read
		public List<Voertuig> GeefVoertuigen(string? kolomnaam = null, object? waarde = null) {
			try {
				_conn.Open();

				string zoekAppendix = "";

				if (kolomnaam != null && waarde != null) {
					string parsedKolomNaam = Regex.Replace(kolomnaam, "[^a-zA-Z0-9]", String.Empty);
					zoekAppendix = $"WHERE v.{parsedKolomNaam}=@waarde";
				}

				SqlCommand cmd = _conn.CreateCommand();
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
					"a.Postcode AS BestuurderAdresPostcode, " +
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
					"FROM Voertuig AS v " +
					"LEFT JOIN Bestuurder AS b " +
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
				List<Voertuig> resultaten = new();

				while (r.Read()) {
					Voertuig huidigVoertuig = QueryParser.ParseReaderNaarVoertuig(r);

					if (resultaten.Any(v => v.Id == huidigVoertuig.Id)) {
						huidigVoertuig = resultaten.First(v => v.Id == huidigVoertuig.Id);
					} else {
						resultaten.Add(huidigVoertuig);
					}

					// het voertuig heeft een bekende bestuurder en deze is nog niet ingesteld
					if (!r.IsDBNull(r.GetOrdinal("BestuurderId")) && huidigVoertuig.Bestuurder is null) {
						// voertuig bestuurder instellen
						huidigVoertuig.ZetBestuurder(
							// bestuurder maken zonder tankkaart want tankkaart parser ontvangt een bestuurder
							QueryParser.ParseReaderNaarBestuurder(
								r, null, huidigVoertuig, QueryParser.ParseReaderNaarAdres(r)
							)
						);

						// nadat bestuurder gemaakt is, tankkaart maken met
						// bestuurder en instellen als tankkaart van de bestuurder
						huidigVoertuig.Bestuurder.ZetTankkaart(
							QueryParser.ParseReaderNaarTankkaart(r, null, huidigVoertuig.Bestuurder)
						);
					}

					// tankkaart brandstof instellen als deze bestaat,
					// en natuurlijk enkel indien de bestuurder een tankkaart ingesteld heeft
					if (!r.IsDBNull(r.GetOrdinal("Brandstof")) && huidigVoertuig.Bestuurder.Tankkaart is not null) {
						TankkaartBrandstof b = QueryParser.ParseReaderNaarTankkaartBrandstof(r);
						if (!huidigVoertuig.Bestuurder.Tankkaart.GeldigVoorBrandstoffen.Contains(b)) {
							huidigVoertuig.Bestuurder.Tankkaart.VoegBrandstofToe(b);
						}
					}


				}

				return resultaten;
			} catch (SqlException ex) when (ex.Number == 207) {
				throw new VoertuigOpslagException("Er werd een ongeldige kolomnaam opgegeven.", ex);
			} catch (Exception ex) {
				throw new VoertuigOpslagException("Er is een onverwachte fout opgetreden.", ex);
			} finally {
				_conn.Close();
			}
		}

		public Voertuig GeefVoertuigDetail(int id) {
			return this.GeefVoertuigen("Id", id).DefaultIfEmpty(null).First();
		}

		public Voertuig ZoekVoertuig(string kolomnaamHoofdletterGevoelig, string waarde) {
			string parsedKolomNaam = Regex.Replace(kolomnaamHoofdletterGevoelig, "[^a-zA-Z0-9]", String.Empty);

			return this.GeefVoertuigen(parsedKolomNaam, waarde).DefaultIfEmpty(null).First();
		}

		// -- Update
		public void UpdateVoertuig(Voertuig voertuig) {
			_conn.Open();
			SqlTransaction tx = _conn.BeginTransaction();

			try {
				SqlCommand cmd = _conn.CreateCommand();
				cmd.Transaction = tx;

				cmd.CommandText = "UPDATE Voertuig SET " +
					"Merk=@merk, Model=@model, Nummerplaat=@nummerplaat, " +
					"Chasisnummer=@chassisnummer, Brandstof=@brandstof, " +
					"Type=@type, Kleur=@kleur, AantalDeuren=@aantaldeuren " +
					"WHERE Id=@voertuigid ;";
				cmd.Parameters.Add(new SqlParameter("@merk", DbType.String));
				cmd.Parameters.Add(new SqlParameter("@model", DbType.String));
				cmd.Parameters.Add(new SqlParameter("@nummerplaat", DbType.String));
				cmd.Parameters.Add(new SqlParameter("@chassisnummer", DbType.String));
				cmd.Parameters.Add(new SqlParameter("@brandstof", DbType.String));
				cmd.Parameters.Add(new SqlParameter("@type", DbType.String));
				cmd.Parameters.Add(new SqlParameter("@kleur", DbType.String));
				cmd.Parameters.Add(new SqlParameter("@aantaldeuren", DbType.String));
				cmd.Parameters.Add(new SqlParameter("@voertuigid", DbType.Int32));
				cmd.Parameters["@merk"].Value = voertuig.Merk.ToString();
				cmd.Parameters["@model"].Value = voertuig.Model;
				cmd.Parameters["@nummerplaat"].Value = voertuig.Nummerplaat;
				cmd.Parameters["@chassisnummer"].Value = voertuig.Chassisnummer;
				cmd.Parameters["@brandstof"].Value = voertuig.Brandstof.ToString();
				cmd.Parameters["@type"].Value = voertuig.Voertuigsoort.ToString();
				cmd.Parameters["@kleur"].Value = (object)voertuig.Kleur ?? DBNull.Value;
				cmd.Parameters["@aantaldeuren"].Value = (object)voertuig.AantalDeuren ?? DBNull.Value;
				cmd.Parameters["@voertuigid"].Value = voertuig.Id;

				cmd.ExecuteNonQuery();

				Voertuig BestaandVoertuig = this.GeefVoertuigDetail((int)voertuig.Id);

				// Controle overeenkomst
				if (BestaandVoertuig.Bestuurder?.Id != voertuig.Bestuurder?.Id) {

					// Verwijder eventuele huidige relatie
					if (BestaandVoertuig.Bestuurder?.Id is not null) {
						SqlCommand cmd_del = _conn.CreateCommand();
						cmd_del.Transaction = tx;

						cmd_del.CommandText = "UPDATE Bestuurder SET VoertuigId=@vid WHERE Id=@bid ;";
						cmd_del.Parameters.Add(new SqlParameter("@vid", DbType.Int32));
						cmd_del.Parameters.Add(new SqlParameter("@bid", DbType.Int32));
						cmd_del.Parameters["@vid"].Value = DBNull.Value;
						cmd_del.Parameters["@bid"].Value = BestaandVoertuig.Bestuurder.Id;
						cmd_del.ExecuteNonQuery();
					}

					// Stel eventuele nieuwe relatie in
					if (voertuig.Bestuurder?.Id is not null) {
						SqlCommand cmd_add = _conn.CreateCommand();
						cmd_add.Transaction = tx;

						cmd_add.CommandText = "UPDATE Bestuurder SET VoertuigId=@vid WHERE Id=@bid ;";
						cmd_add.Parameters.Add(new SqlParameter("@vid", DbType.Int32));
						cmd_add.Parameters.Add(new SqlParameter("@bid", DbType.Int32));
						cmd_add.Parameters["@vid"].Value = voertuig.Id;
						cmd_add.Parameters["@bid"].Value = voertuig.Bestuurder.Id;
						cmd_add.ExecuteNonQuery();
					}
				}

				tx.Commit();
			} catch (Exception ex) {
				try {
					tx.Rollback();
				} catch (InvalidOperationException e) { /* Error vond plaats voor de commit, exception negeren */
				} catch (Exception e) {
					throw new VoertuigOpslagException("Rollback gaf een onverwachte foutmelding.", e);
				}

				throw new VoertuigOpslagException("Er is een onverwachte fout opgetreden.", ex);
			} finally {
				_conn.Close();
			}
		}

		// Delete
		public void VerwijderVoertuig(int id) {
			_conn.Open();
			SqlTransaction tx = _conn.BeginTransaction();

			try {
				SqlCommand cmd_relatie = _conn.CreateCommand();
				cmd_relatie.Transaction = tx;
				cmd_relatie.CommandText = "UPDATE Bestuurder SET VoertuigId=@nieuw WHERE VoertuigId=@id ;";
				cmd_relatie.Parameters.Add(new SqlParameter("@id", DbType.Int32));
				cmd_relatie.Parameters.Add(new SqlParameter("@nieuw", DbType.Int32));
				cmd_relatie.Parameters["@id"].Value = id;
				cmd_relatie.Parameters["@nieuw"].Value = DBNull.Value;
				cmd_relatie.ExecuteNonQuery();

				SqlCommand cmd = _conn.CreateCommand();
				cmd.Transaction = tx;
				cmd.CommandText = "DELETE FROM Voertuig WHERE Id=@id ;";
				cmd.Parameters.Add(new SqlParameter("@id", DbType.Int32));
				cmd.Parameters["@id"].Value = id;
				cmd.ExecuteNonQuery();

				tx.Commit();
			} catch (Exception ex) {
				try {
					tx.Rollback();
				} catch (InvalidOperationException e) { /* Error vond plaats voor de commit, exception negeren */
				} catch (Exception e) {
					throw new TankkaartOpslagException("Rollback gaf een onverwachte foutmelding.", e);
				}

				throw new VoertuigOpslagException("Er trad een onverwachte fout op.", ex);
			} finally {
				_conn.Close();
			}
		}

	}
}
