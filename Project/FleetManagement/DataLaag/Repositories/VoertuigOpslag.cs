using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using BusinessLaag.Model;
using BusinessLaag;
using BusinessLaag.Interfaces;
using BusinessLaag.Exceptions;
using BusinessLaag.Model.Enum;
using System.Text.RegularExpressions;

namespace DataLaag.Repositories
{
    public class VoertuigOpslag : IVoertuigOpslag
    {
        private SqlConnection _conn { get; set; }

        public void ZetConnectionString(string connString) {
            _conn = connString.Length > 5 ? new SqlConnection(connString) : throw new VoertuigOpslagException("Connectiestring moet langer zijn dan 5 karakters.");
        }

        // -- create
        public int VoegVoertuigToe(Voertuig voertuig) {
            try {
                _conn.Open();
                SqlCommand cmd = _conn.CreateCommand();

                cmd.CommandText = "INSERT INTO Voertuig (Merk, Model, Nummerplaat, Chasisnummer, Brandstof, Type, Kleur, AantalDeuren) VALUES (@merk, @model, @nummerplaat, @chassisnummer, @brandstof, @type, @kleur, @aantaldeuren); SELECT SCOPE_IDENTITY()";
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

                return Convert.ToInt32(cmd.ExecuteScalar());
            } catch (SqlException e) {
                if (e.Number == 2627) {
                    throw new VoertuigOpslagException("Dit voertuig bestaat reeds in de databank.");
                } else { throw; }
            } catch (Exception e) {
                throw new VoertuigOpslagException("Onverwachte fout.", e);
            } finally {
                _conn.Close();
            }
        }

        // -- read
        private Voertuig _parseReaderItemNaarVoertuig(SqlDataReader r) {
            Merk readerMerk = (Merk)(Enum.Parse(typeof(Merk), (string)r["Merk"], true));
            VoertuigBrandstof readerBrandstof = (VoertuigBrandstof)(Enum.Parse(typeof(VoertuigBrandstof), (string)r["Brandstof"], true));
            Voertuigsoort readerVoertuigsoort = (Voertuigsoort)(Enum.Parse(typeof(Voertuigsoort), (string)r["Voertuigsoort"], true));

            return new Voertuig((int?)r["Id"], readerMerk, (string)r["Model"], (string)r["Nummerplaat"],
                readerBrandstof, readerVoertuigsoort, (string?)r["Kleur"], (int?)r["AantalDeuren"], null, (string)r["Chasisnummer"]);
        }

        public KeyValuePair<int?, Voertuig> GeefVoertuigDetail(int id) {
            try {
                _conn.Open();

                SqlCommand cmd = _conn.CreateCommand();
                cmd.CommandText = "SELECT Voertuig.*, Bestuurder.Id AS BestuurderId FROM Voertuig INNER JOIN Bestuurder ON Voertuig.Id=Bestuurder.VoertuigId WHERE Voertuig.Id = @voertuigid ;";

                cmd.Parameters.Add(new SqlParameter("@voertuigid", DbType.Int32));
                cmd.Parameters["@voertuigid"].Value = id;

                SqlDataReader r = cmd.ExecuteReader();

                while (r.Read()) {
                    Voertuig v = _parseReaderItemNaarVoertuig(r);
                    return new KeyValuePair<int?, Voertuig>((int?)r["BestuurderId"], v);
                }

                return new KeyValuePair<int?, Voertuig>(null, null);

            } catch (Exception e) {
                throw new VoertuigOpslagException("Er trad een onverwachte fout op.", e);
            } finally {
                _conn.Close();
            }
        }

        public List<KeyValuePair<int?, Voertuig>> GeefVoertuigen() {

            List<KeyValuePair<int?, Voertuig>> voertuigen = new();

            try {
                _conn.Open();

                SqlCommand cmd = _conn.CreateCommand();
                cmd.CommandText = "SELECT Voertuig.*, Bestuurder.Id AS BestuurderId FROM Voertuig INNER JOIN Bestuurder ON Voertuig.Id=Bestuurder.VoertuigId ;";

                SqlDataReader r = cmd.ExecuteReader();

                while (r.Read()) {
                    Voertuig v = _parseReaderItemNaarVoertuig(r);
                    voertuigen.Add(new KeyValuePair<int?, Voertuig>((int?)r["BestuurderId"], v));
                }

                return voertuigen;
            } catch (Exception e) {
                throw new VoertuigOpslagException("Er trad een onverwachte fout op.", e);
            } finally {
                _conn.Close();
            }
        }

        public Voertuig ZoekVoertuig(string kolomnaamHoofdletterGevoelig, string waarde) {
            string parsedKolomNaam = Regex.Replace(kolomnaamHoofdletterGevoelig, "[^a-zA-Z0-9]", String.Empty);

            try {
                _conn.Open();

                SqlCommand cmd = _conn.CreateCommand();
                cmd.CommandText = $"SELECT Voertuig.*, Bestuurder.Id AS BestuurderId FROM Voertuig INNER JOIN Bestuurder ON Voertuig.Id=Bestuurder.VoertuigId WHERE Voertuig.{parsedKolomNaam} = @waarde ;";

                cmd.Parameters.Add(new SqlParameter("@waarde", DbType.String));
                cmd.Parameters["@waarde"].Value = waarde;

                SqlDataReader r = cmd.ExecuteReader();

                while (r.Read()) {
                    return _parseReaderItemNaarVoertuig(r);
                }

                return null;

            } catch (SqlException e) {
                if (e.Number == 207) {
                    throw new VoertuigOpslagException("Er werd een ongeldige kolomnaam opgegeven.");
                } else { throw; } // TODO: geeft dit de exception door aan onderstaande catch?
            } catch (Exception e) {
                throw new VoertuigOpslagException("Er trad een onverwachte fout op.", e);
            } finally {
                _conn.Close();
            }
        }

        // -- update
        public void UpdateVoertuig(Voertuig voertuig) {
            try {
                if(voertuig?.Id is null) {
                    throw new VoertuigOpslagException("Kan de bestelling niet updaten zonder dat Voertuig en zijn id niet null zijn.");
                }

                _conn.Open();
                SqlCommand cmd = _conn.CreateCommand();

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
            } catch (Exception e) {
                throw new VoertuigOpslagException("Onverwachte fout.", e);
            } finally {
                _conn.Close();
            }
        }

        // -- delete
        public void VerwijderVoertuig(Voertuig voertuig) {
            try {
                if (voertuig.Id is null) throw new VoertuigOpslagException("Kan voertuig niet verwijderen zonder id.");

                _conn.Open();
                SqlCommand cmd = _conn.CreateCommand();

                cmd.CommandText = "DELETE FROM Voertuig WHERE Id=@id ;";
                cmd.Parameters.Add(new SqlParameter("@id", DbType.Int32));
                cmd.Parameters["@id"].Value = voertuig.Id;

                cmd.ExecuteNonQuery();
            } catch (Exception e) {
                throw new VoertuigOpslagException("Er trad een onverwachte fout op.", e);
            } finally {
                _conn.Close();
            }
        }

    }
}
