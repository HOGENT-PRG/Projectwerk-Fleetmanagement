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
            throw new NotImplementedException();
        }

        public IEnumerable<Tankkaart> zoekTankkaarten()
        {
            throw new NotImplementedException();
        }

        public List<KeyValuePair<int?, Tankkaart>> GeefTankkaarten()
        {
            throw new NotImplementedException();
        }

        public KeyValuePair<int?, Tankkaart> GeefTankkaartDetail(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Tankkaart> zoekTankkaarten(string kolom, string waarde)
        {
            throw new NotImplementedException();
        }

        public int voegTankkaartToe(int? id)
        {
            throw new NotImplementedException();
        }
    }
}
