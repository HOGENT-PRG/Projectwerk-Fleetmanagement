using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using BusinessLaag.Model;
using BusinessLaag.Model.Enum;
using BusinessLaag;
using BusinessLaag.Interfaces;
using BusinessLaag.Exceptions;
using System.Text.RegularExpressions;

namespace DataLaag.Repositories
{
    public class TankkaartOpslag : ITankkaartOpslag
    {
        private SqlConnection _connector { get; set; }

        public void ZetConnectionString(string connString)
        {
            _connector = connString.Length > 5 ? new SqlConnection(connString) : throw new TankkaartOpslagException("Connectiestring moet langer zijn dan 5 karakters.");
        }

     

        public IEnumerable<string> geefTankkaartProperties()
        {
            throw new NotImplementedException();
        }

        public void updateTankkaart(Tankkaart tankkaart)
        {
            try
            {
                if(tankkaart.Id is null)
                {
                    throw new TankkaartException("Tankkaart id kan niet null zijn daardoor kan de tankkaart niet worden geupdate");
                }
                _connector.Open();
                SqlCommand cmd = _connector.CreateCommand();
                cmd.CommandText = "Update dbo.tankkaart set Id=@id, Kaartnummer=@kaartnummer,Vervaldatum=@vervaldatum,Pincode=@Pincode";
                cmd.Parameters.Add(new SqlParameter("@id", DbType.Int32));
                cmd.Parameters.Add(new SqlParameter("@kaartnummer", DbType.String));
                cmd.Parameters.Add(new SqlParameter("@vervaldatum", DbType.DateTime));
                cmd.Parameters.Add(new SqlParameter("@Pincode", DbType.String));
                cmd.Parameters["@id"].Value =(object)tankkaart.Id ?? DBNull.Value;
                cmd.Parameters["@kaartnummer"].Value =tankkaart.Kaartnummer;
                cmd.Parameters["@vervaldatum"].Value =tankkaart.Vervaldatum;
                cmd.Parameters["@Pincode"].Value =(object)tankkaart.Pincode ?? DBNull.Value;
                cmd.ExecuteNonQuery();
            }catch(Exception ex)
            {
                throw new TankkaartException("Unexpected error", ex);
            }
            finally
            {
                _connector.Close();
            }
        }

        public void verwijderTankkaart(Tankkaart tankkaart)
        {
            try
            {


                if (tankkaart.Id is null)
                {
                    throw new TankkaartException("Tankkaart kan niet verwijderd worden als id null is");
                }
                _connector.Open();
                SqlCommand command = _connector.CreateCommand();
                command.CommandText = "Delete From dbo.tankkaart where Id=@id";
                command.Parameters.Add(new SqlParameter("@id", DbType.Int32));
                command.Parameters["@id"].Value = tankkaart.Id;
                command.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                throw new TankkaartException("Unexpected error", ex);
            }
            finally
            {
                _connector.Close();
            }
        }

        public void voegTankkaartToe(Tankkaart tankkaart)
        {
            try
            {
                if(tankkaart.Id is null)
                {
                    throw new TankkaartException("Kan tankkaart niet toevoegen als id null is");
                }
                _connector.Open();
                SqlCommand command = _connector.CreateCommand();
                command.CommandText = "insert into FleetManager.dbo.Tankkaart values(@kaartnummer,@vervaldatum,@pincode);";
                command.Parameters.Add(new SqlParameter("@kaartnummer", DbType.String));
                command.Parameters.Add(new SqlParameter("@vervaldatum", DbType.DateTime));
                command.Parameters.Add(new SqlParameter("@pincode", DbType.String));
                command.Parameters["@kaartnummer"].Value = tankkaart.Kaartnummer;
                command.Parameters["@vervaldatum"].Value = tankkaart.Vervaldatum;
                command.Parameters["@pincode"].Value = tankkaart.Pincode;
                command.ExecuteNonQuery();


            }
            catch(Exception ex)
            {
                throw new TankkaartException("Unexpected error", ex);
            }
            finally
            {
                _connector.Close();
            }
        }
        private Tankkaart _parseReaderItemNaarTankkaart(SqlDataReader r)
        {
             TankkaartBrandstof brandstof= (TankkaartBrandstof)(Enum.Parse(typeof(TankkaartBrandstof), (string)r["Brandstof"], true));
            List<TankkaartBrandstof> tkbra = new List<TankkaartBrandstof>();
            tkbra.Add(brandstof);


            return new Tankkaart((int?)r["id"], (string)r["kaartnummer"], (DateTime)r["vervaldatum"], (string)r["pincode"], tkbra, null);
           
        }

        public List<KeyValuePair<int?, Tankkaart>> GeefTankkaarten()
        {
            List<KeyValuePair<int?, Tankkaart>> tankkaarten = new();

            try
            {
                _connector.Open();

                SqlCommand cmd = _connector.CreateCommand();
                cmd.CommandText = "SELECT * from tankkaart ;";

                SqlDataReader r = cmd.ExecuteReader();

                while (r.Read())
                {
                    Tankkaart t = _parseReaderItemNaarTankkaart(r);
                    tankkaarten.Add(new KeyValuePair<int?, Tankkaart>((int?)r["Id"], t));
                }

                return tankkaarten;
            }
            catch (Exception ex)
            {
                throw new VoertuigOpslagException("Unexpected error", ex);
            }
            finally
            {
                _connector.Close();
            }
        }

        public KeyValuePair<int?, Tankkaart> GeefTankkaartDetail(int id)
        {
           

            try
            {
                _connector.Open();

                SqlCommand cmd = _connector.CreateCommand();
                cmd.CommandText = "SELECT * from tankkaart where Id=@id ;";

           
                cmd.Parameters.Add(new SqlParameter("@id",DbType.Int32));
                cmd.Parameters["@id"].Value = id;
                SqlDataReader r = cmd.ExecuteReader();
                while (r.Read())
                {
                    Tankkaart t = _parseReaderItemNaarTankkaart(r);
                    return new KeyValuePair<int?, Tankkaart>((int?)r["Id"], t);
                }

                return new KeyValuePair<int?, Tankkaart>(null, null);
            }
            catch (Exception ex)
            {
                throw new VoertuigOpslagException("Unexpected error", ex);
            }
            finally
            {
                _connector.Close();
            }
        }

        public IEnumerable<Tankkaart> zoekTankkaarten(string kolom, string waarde)
        {
            string parsedKolomNaam = Regex.Replace(kolom, "[^a-zA-Z0-9]", String.Empty);
            try
            {
                _connector.Open();
                SqlCommand command = _connector.CreateCommand();
                command.CommandText = $"SELECT * from tankkaart where tankkaart.{kolom}=@waarde ";
                command.Parameters.Add(new SqlParameter("@waarde", DbType.String));
                command.Parameters["@waarde"].Value = waarde;
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    _parseReaderItemNaarTankkaart(reader);
                }
                return null;
            }
            catch (SqlException ex) when (ex.Number == 207)
            {
                throw new TankkaartException("Er werd een ongeldige kolomnaam opgegeven.");
            }
            catch (Exception ex)
            {
                throw new TankkaartException("Unexpected error", ex);
            }
            finally
            {
                _connector.Close();
            }
        }

        public int voegTankkaartToe(int? id)
        {
            throw new NotImplementedException();
        }
    }
}
