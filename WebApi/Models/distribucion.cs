using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using WebApi.Model;

namespace WebApi.Models
{
    /// <summary>
    /// Esta es la clase base de dsitribucion que representa la informacion logistica necesaria para tener claridad de la distribucion
    /// </summary> 
    public class Distribucion
    {
        public int IdDistribucion { get; set; }
        [Required]
        public string Rut_Proveedor { get; set; }
        [Required]
        [Range(300000000, 399999999)]
        public int Doc_Cenabast { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Debe ser un valor entre {1} y {2}.")]
        public int Factura { get; set; }
        public string Fecha_Fac { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Debe ser un valor entre {1} y {2}.")]
        public int Guia { get; set; }
        public string Fecha_Gui { get; set; }
        [StringLength(30, ErrorMessage = "El largo de O_Trans no puede ser mayor a 30")]
        public string O_Trans { get; set; }
        public string FechaCreacion { get; set; }
        public string FechaActualizacion { get; set; }
        [Required]
        public List<DistribucionDetalle> Detalles { get; set; }
        [Required]
        public List<DistribucionMovimiento> Movimientos { get; set; }

        /// <summary>
        /// Ingresa un Objeto Distribucion a BBDD
        /// </summary> 
        public int Ingresar(Distribucion distribucion)
        {
            int i = 0;
            //string sql = "insert into Distribucion (Rut_Proveedor, Doc_Cenabast, Factura, Fecha_Fac, Guia, Fecha_Gui, O_Trans, FechaCreacion, FechaActualizacion) values ( '"+distribucion.Rut_Proveedor+"' , '"+distribucion.Doc_Cenabast+"', '"+distribucion.Factura+"','"+distribucion.Fecha_Fac+"', '"+distribucion.Guia+"', '"+distribucion.Fecha_Gui+"','"+distribucion.O_Trans+ "', CONVERT(varchar,GETDATE(),126), CONVERT(varchar,GETDATE(),126))";

            string sql = "insert into Distribucion (Rut_Proveedor, Doc_Cenabast, Factura, Fecha_Fac, Guia, Fecha_Gui, O_Trans, FechaCreacion, FechaActualizacion) values ( @Rut_Proveedor , @Doc_Cenabast, @Factura, @Fecha_Fac, @Guia, @Fecha_Gui, @O_Trans, CONVERT(varchar,GETDATE(),126), CONVERT(varchar,GETDATE(),126))";

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@Rut_Proveedor", distribucion.Rut_Proveedor);
            cmd.Parameters.AddWithValue("@Doc_Cenabast", distribucion.Doc_Cenabast);
            cmd.Parameters.AddWithValue("@Factura", distribucion.Factura);
            cmd.Parameters.AddWithValue("@Fecha_Fac", distribucion.Fecha_Fac);
            cmd.Parameters.AddWithValue("@Guia", distribucion.Guia);
            cmd.Parameters.AddWithValue("@Fecha_Gui", distribucion.Fecha_Gui);
            cmd.Parameters.AddWithValue("@O_Trans", distribucion.O_Trans);
       
            con.Open();
            try
            {
                i = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.ToString());
                i = 0;
            }
            finally
            {
                // Cierro la Conexión.
                con.Close();
            }

            return i;
        }

        /// <summary>
        /// Obtiene un Objeto Distribucion a BBDD
        /// </summary> 
        public Distribucion Obtener(int docVenta)
        {
            //string sql = "select IdDistribucion, Rut_Proveedor, Doc_Cenabast, Factura, Fecha_Fac, Guia, Fecha_Gui, O_Trans, FechaCreacion, FechaActualizacion from Distribucion where Doc_Cenabast = '" + docVenta + "'";
            string sql = "select IdDistribucion, Rut_Proveedor, Doc_Cenabast, Factura, Fecha_Fac, Guia, Fecha_Gui, O_Trans, FechaCreacion, FechaActualizacion from Distribucion where Doc_Cenabast = @docVenta";

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@docVenta", docVenta);
            SqlDataReader reader;
            Distribucion distribucion = null;
            DistribucionDetalle det = new DistribucionDetalle();
            DistribucionMovimiento mov = new DistribucionMovimiento();
            // List<InformacionLogistica_detalle> listaDetalle = null;

            con.Open();
            try
            {
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    // Defino los Estados para los elementos de mi Formulario cuando hago click en el botón Buscar.
                    distribucion = new Distribucion();
                    distribucion.IdDistribucion = int.Parse(reader["IdDistribucion"].ToString());
                    distribucion.Rut_Proveedor = reader["Rut_Proveedor"].ToString();
                    distribucion.Doc_Cenabast = int.Parse(reader["Doc_Cenabast"].ToString());
                    distribucion.Factura = int.Parse(reader["Factura"].ToString());
                    distribucion.Fecha_Fac = reader["Fecha_Fac"].ToString();
                    distribucion.Guia = int.Parse(reader["Guia"].ToString());
                    distribucion.Fecha_Gui = reader["Fecha_Gui"].ToString();
                    distribucion.O_Trans = reader["O_Trans"].ToString();
                    distribucion.FechaCreacion = reader["FechaCreacion"].ToString();
                    distribucion.FechaActualizacion = reader["FechaActualizacion"].ToString();
                    distribucion.Detalles = det.ObtenerDetalles(distribucion.Doc_Cenabast);
                    distribucion.Movimientos = mov.ObtenerMovimientos(distribucion.Doc_Cenabast);

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

            return distribucion;
        }

        /// <summary>
        /// Obtiene una lista de Objetos Distribucion a BBDD
        /// </summary> 
        public List<Distribucion> ObtenerDistribuciones()
        {
            string sql = "select IdDistribucion, Rut_Proveedor, Doc_Cenabast, Factura, Fecha_Fac, Guia, Fecha_Gui, O_Trans, FechaCreacion, FechaActualizacion from Distribucion ";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand(sql, con);
            SqlDataReader reader;
            List<Distribucion> lista = new List<Distribucion>();
            DistribucionDetalle det = new DistribucionDetalle();
            DistribucionMovimiento mov = new DistribucionMovimiento();
            con.Open();
            try
            {
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    // Defino los Estados para los elementos de mi Formulario cuando hago click en el botón Buscar.
                    Distribucion distribucion = new Distribucion();
                    distribucion.IdDistribucion = int.Parse(reader["IdDistribucion"].ToString());
                    distribucion.Rut_Proveedor = reader["Rut_Proveedor"].ToString();
                    distribucion.Doc_Cenabast = int.Parse(reader["Doc_Cenabast"].ToString());
                    distribucion.Factura = int.Parse(reader["Factura"].ToString());
                    distribucion.Fecha_Fac = reader["Fecha_Fac"].ToString();
                    distribucion.Guia = int.Parse(reader["Guia"].ToString());
                    distribucion.Fecha_Gui = reader["Fecha_Gui"].ToString();
                    distribucion.O_Trans = reader["O_Trans"].ToString();
                    distribucion.FechaCreacion = reader["FechaCreacion"].ToString();
                    distribucion.FechaActualizacion = reader["FechaActualizacion"].ToString();
                    distribucion.Detalles = det.ObtenerDetalles(distribucion.Doc_Cenabast);
                    distribucion.Movimientos = mov.ObtenerMovimientos(distribucion.Doc_Cenabast);
                    lista.Add(distribucion);
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

            return lista;
        }

        /// <summary>
        /// Actualiza un Objeto Distribucion a BBDD
        /// </summary> 
        public int Actualizar(int docVenta, Distribucion distribucion)
        {
            int i = 0;
            string sql = "update Distribucion set Factura = @Factura, Fecha_Fac = @Fecha_Fac, Guia = @Guia, Fecha_Gui = @Fecha_Gui , O_Trans = @O_Trans,  FechaActualizacion = GETDATE(), ControlServicio = NULL where Doc_Cenabast = @docVenta and IdDistribucion = @IdDistribucion ";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@Factura", distribucion.Factura);
            cmd.Parameters.AddWithValue("@Fecha_Fac", Convert.ToDateTime(distribucion.Fecha_Fac));
            cmd.Parameters.AddWithValue("@Guia", distribucion.Guia);
            cmd.Parameters.AddWithValue("@Fecha_Gui", Convert.ToDateTime(distribucion.Fecha_Gui));
            cmd.Parameters.AddWithValue("@O_Trans", distribucion.O_Trans);
            cmd.Parameters.AddWithValue("@docVenta", docVenta);
            cmd.Parameters.AddWithValue("@IdDistribucion", distribucion.IdDistribucion);

            con.Open();
            try
            {
                i = cmd.ExecuteNonQuery();
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

            return i;
        }

        /// <summary>
        /// Elimina un Objeto Distribucion a BBDD
        /// </summary> 
        public int Eliminar(int docVenta)
        {

            int i = 0;
            string sql = " delete Distribucion where Doc_Cenabast = @docVenta; ";
            sql = sql + " delete DistribucionDetalle where Doc_Cenabast = @docVenta; ";
            sql = sql + " delete DistribucionMovimiento where Doc_Cenabast = @docVenta; ";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@docVenta", docVenta);
            con.Open();
            try
            {
                i = cmd.ExecuteNonQuery();
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

            return i;
        }

        public int siExiste(int docVenta)
        {

            int i = 0;
            string sql = "select * from Distribucion where Doc_Cenabast = @docVenta";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@docVenta", docVenta);
            con.Open();
            try
            {
                i = cmd.ExecuteNonQuery();
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

            return i;
        }
    }
}