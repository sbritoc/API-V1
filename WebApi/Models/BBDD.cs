using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace WebApi.Model
{
    class BBDD
    {
        public SqlConnection conexion  = null;
        public SqlCommand cmd = null;
        public SqlDataReader reader = null;
        string server           = "10.8.0.50";
        string database = "Transacciones";
        string uid = "sa";
        string pwd = "Troyano81596120";
        
         public BBDD() {

            //this.conexion = new SqlConnection("DRIVER={SQL Server}; SERVER=10.8.0.50; UID=sa; PWD=tc1581; DATABASE=Transacciones");
            this.conexion = new SqlConnection("server="+server+" ; database="+database+" ; UID="+uid+"; PWD="+pwd+" ");
            //conexion.Open();
            //conexion.Close();
            
         }

        public void Conectar() {
            this.conexion.Open();
        }

        public void Desconectar() {
            this.conexion.Close();
        }

        /*INGRESA LOS DATOS AL SQL SERVER Y DEVUELVE EL ID EL OBJETO INSERTADO*/
        public int Ingresar(string sql) {

            int i = 0;
            this.cmd = new SqlCommand(sql, this.conexion);
            //this.cmd.CommandType = CommandType.;
            this.Conectar();
            try
            {
                i = Convert.ToInt32(this.cmd.ExecuteScalar());
                if(i > 0) 
                    Console.WriteLine("Registro ingresado correctamente !");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.ToString());
            }
            finally
            {
                // Cierro la Conexión.
                this.Desconectar();
            }
            return i;
        } 


    }
}
