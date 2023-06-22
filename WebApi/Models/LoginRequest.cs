using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace WebApi.Models
{
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public bool GetProveedor(string rut, string password) {

            string sql = "select RutProveedor from ClaveRegistroProveedor where RutProveedor = @Rut_Proveedor and PasswordApi = @PasswordApi ";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString1"].ConnectionString);
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@Rut_Proveedor", rut);
            cmd.Parameters.AddWithValue("@PasswordApi", password);
            SqlDataReader reader;
            bool isValid = false;

            con.Open();
            try
            {
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    isValid = true;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.ToString());
            }
            finally
            {
                // Cierro la Conexión.
                con.Close();
            }

            return isValid;    
        }

        public bool GetAdministrador(string rut, string password)
        {

            string sql = "select Rut from DistribucionUsuario where Rut = @Rut and Password = @Password ";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@Rut", rut);
            cmd.Parameters.AddWithValue("@Password", password);
            SqlDataReader reader;
            bool isValid = false;

            con.Open();
            try
            {
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    isValid = true;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.ToString());
            }
            finally
            {
                // Cierro la Conexión.
                con.Close();
            }

            return isValid;


        }
    }

    
}