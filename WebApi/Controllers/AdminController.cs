using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web.Http;
using WebApi.Models;

namespace WebApi.Controllers
{
    /// <summary>
    /// admin controller class for testing security token with role admin
    /// </summary>
    [Authorize(Roles = "Administrador")]
    [RoutePrefix("api/v1/admin")]
    public class AdminController : ApiController
    {

        [HttpPost]
        [Route("distribucion")]
        //POST : api/v1/distribucion
        /// <summary>
        /// Controlador que recibe un Objeto Distribucion para ingresar la infromacion logistica
        /// </summary> 
        public IHttpActionResult Post(Distribucion distribucion)
        {
            if (ModelState.IsValid)
            {
                if ((distribucion.Factura == 0) && (distribucion.Guia == 0)) {
                    return BadRequest("Debe al menos informar uno de los dos documentos relacionados: Factura o Guía.");
                }
                DistribucionDetalle det = new DistribucionDetalle();
                DistribucionMovimiento mov = new DistribucionMovimiento();
                if (distribucion.Obtener(distribucion.Doc_Cenabast) == null)//valido si no existe el doc_cenabast
                {
                    if (distribucion.Detalles != null)//valido si los detalles no son nulos
                    {
                        if (distribucion.Movimientos != null)//valido si los movimiento no son nulos
                        {
                            int isValid = det.Ingresar(distribucion.Detalles);
                            if (isValid <= 0)
                            {
                                return BadRequest("Ha ocurrido un error al ingresar un detalle.");
                            }

                            isValid = mov.Ingresar(distribucion.Movimientos);
                            if (isValid <= 0)
                            {
                                return BadRequest("Ha ocurrido un error al ingresar un movimiento.");
                            }
                            isValid = distribucion.Ingresar(distribucion);
                            if (isValid <= 0)
                            {
                                return BadRequest("Ha ocurrido un error al ingresar una distribucion.");
                            }
                            return Created("/api/v1/Public/distribucion/" + distribucion.Doc_Cenabast, new { Message = "La Distribucion ha sido registrada." });
                        }
                        else
                        {
                            return BadRequest("El Movimiento del recurso no debe ser nulo.");
                        }
                    }
                    else
                    {
                        return BadRequest("El Detalle del recurso no debe ser nulo.");
                    }
                }
                else
                {
                    return BadRequest("El Doc_Cenabast ya existe.");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }

        }

        [HttpGet]
        [Route("distribucion/{Doc_Cenabast}")]
        //GET : api/v1/distribucion/369854621
        /// <summary>
        /// Controlador que obtiene un Objeto distribucion filtrado por el Doc_Cenabast
        /// </summary> 
        public IHttpActionResult Get_Distribucion(int Doc_Cenabast)
        {
            Distribucion distribucion = new Distribucion();
            distribucion = distribucion.Obtener(Doc_Cenabast);
            if (distribucion != null)
            {
                return Ok(distribucion);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("distribucion")]
        //GET : api/v1/distribuciones
        /// <summary>
        /// Controlador que obtiene todas las distribuciones en los registros
        /// </summary> 
        public IHttpActionResult GetAll_Distribucion()
        {
            Distribucion distribucion = new Distribucion();

            if (distribucion.ObtenerDistribuciones() != null)
                return Ok(distribucion.ObtenerDistribuciones());
            else
            {
                return NotFound();
            }
        }

        [HttpPut]
        [Route("distribucion/{Doc_Cenabast}")]
        //PUT : api/v1/distribucion/369854621
        /// <summary>
        /// Controlador que actualiza una distribucion
        /// </summary> 
        public IHttpActionResult Put(int Doc_Cenabast, Distribucion distribucion)
        {
            if (ModelState.IsValid)
            {
                if (Doc_Cenabast == distribucion.Doc_Cenabast)
                {
                    if (distribucion.Obtener(Doc_Cenabast) != null)
                    {
                        DistribucionDetalle det = new DistribucionDetalle();
                        DistribucionMovimiento mov = new DistribucionMovimiento();

                        if (distribucion.Detalles != null)//valido si los detalles no son nulos
                        {
                            if (distribucion.Movimientos != null)//valido si los movimiento no son nulos
                            {
                                int isValid = distribucion.Actualizar(Doc_Cenabast, distribucion);
                                if (isValid <= 0)
                                {
                                    return BadRequest("No ha sido posible actualizar el recurso.");
                                    //return Ok(distribucion);
                                }

                                isValid = det.Actualizar(Doc_Cenabast, distribucion.Detalles);
                                if (isValid <= 0)
                                {
                                    return BadRequest("Ha ocurrido un error al actualizar el recurso.");
                                }

                                isValid = mov.Actualizar(Doc_Cenabast, distribucion.Movimientos);
                                if (isValid <= 0)
                                {
                                    return BadRequest("Ha ocurrido un error al ingresar un movimiento.");
                                }
                                distribucion = distribucion.Obtener(Doc_Cenabast);
                                return Ok(distribucion);
                            }
                            else
                            {
                                return BadRequest("El Movimiento del recurso no debe ser nulo.");
                            }
                        }
                        else
                        {
                            return BadRequest("El Detalle del recurso no debe ser nulo.");
                        }
                    }
                    else
                    {
                        return BadRequest("El " + Doc_Cenabast + " no existe. Favor revisar y volvera intentar.");
                    }
                }
                else {
                    return BadRequest("La Url contine el Doc_Cenabast " + Doc_Cenabast + "y la estructura " + distribucion.Doc_Cenabast + " lo que provoca inconsistencia.");
                }
            }
            else {
                return BadRequest(ModelState);
            }

        }

        [HttpDelete]
        [Route("distribucion/{Doc_Cenabast}")]
        //DELETE : api/v1/distribucion/369854621
        /// <summary>
        /// Controlador que elimina un Objeto Distribucion
        /// </summary> 
        public IHttpActionResult Delete(int Doc_Cenabast)
        {
            Distribucion distribucion = new Distribucion();
            if (distribucion.Obtener(Doc_Cenabast) != null)
            {
                int validar = distribucion.Eliminar(Doc_Cenabast);
                if (validar > 0)
                    return Ok(new { Message = "El recurso ha sido eliminado exitosamente." });
                else
                    return BadRequest("El recurso no ha sido encontrado.");
            }
            else
                return NotFound();
        }

        [HttpGet]
        [Route("distribucion/{Doc_Cenabast}/detalle")]
        //GET : api/v1/distribucion/369854621/detalle
        /// <summary>
        /// Controlador que obtiene el detalle del Doc_Cenabast
        /// </summary> 
        public IHttpActionResult Get_detalle(int Doc_Cenabast)
        {

            DistribucionDetalle det = new DistribucionDetalle();
            if (det.ObtenerDetalles(Doc_Cenabast) != null)
            {
                return Ok(det.ObtenerDetalles(Doc_Cenabast));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("distribucion/{Doc_Cenabast}/movimiento")]
        //GET : api/v1/distribucion/369854621/movimiento
        /// <summary>
        /// Controlador que obtiene los movimientos del Doc_Cenabast
        /// </summary> 
        public IHttpActionResult Get_movimiento(int Doc_Cenabast)
        {
            DistribucionMovimiento mov = new DistribucionMovimiento();
            if (mov.ObtenerMovimientos(Doc_Cenabast) != null)
            {
                return Ok(mov.ObtenerMovimientos(Doc_Cenabast));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("distribucion/{Doc_Cenabast}/movimiento")]
        //POST : api/v1/distribucion/369854621/movimiento
        /// <summary>
        /// Controlador que ingresa los movimientos de una distribucion
        /// </summary> 
        public IHttpActionResult Post_movimiento(int Doc_Cenabast, DistribucionMovimiento movimiento)
        {
            if (ModelState.IsValid)
            {
                if (movimiento.ObtenerMovimientos(Doc_Cenabast) != null)
                {
                    int validar = movimiento.Ingresar(movimiento);
                    if (validar > 0)
                    {
                        //return CreatedAtRoute("DefaultApi", new { id = movimiento.Doc_Cenabast }, movimiento);
                        return Created("/api/v1/Public/distribucion/" + Doc_Cenabast + "/movimiento", new { Message = "El movimiento ha sido registrado." });
                    }
                    else
                    {
                        return BadRequest("No se ha podido registrar el movimiento.");
                    }

                }
                else
                {
                    return BadRequest("No se ha registrado una distribucion con este Doc_Cenabast.");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Route("cedible")]
        //POST : api/v1/admin/cedible
        /// <summary>
        /// Controlador que recibe un Objeto DistribucionCedible para ingresar
        /// </summary> 
        public IHttpActionResult Post_Cedible(DistribucionCedible cedible)
        {
            if (ModelState.IsValid)
            {
                if (cedible != null)
                {
                    if (cedible.IsBase64(cedible.Documento)) 
                    {
                        //if (cedible.Obtener(cedible.Doc_Cenabast) == null)
                        //{
                            if (cedible.IngresarCedible(cedible) > 0)
                            {
                                return Created("/api/v1/Public/cedible/" + cedible.Doc_Cenabast, new { Message = "El cedible ha sido registrado." });
                            }
                            else
                            {
                                return BadRequest("El Cedible no se ha registrado.");
                            }
                        /*}
                        else
                        {
                            return BadRequest("El Cedible ya fue registrado.");
                        }*/
                    }
                    else {
                        return BadRequest("No es un Documento Valido");
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            else {
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [Route("cedible")]
        //POST : api/v1/admin/cedible
        /// <summary>
        /// Controlador obtiene todos los objetos DistribucionCedibles
        /// </summary> 
        public IHttpActionResult GetAll_Cedible()
        {
            DistribucionCedible cedible = new DistribucionCedible();

            if (cedible.ObtenerCedibles() != null)
                return Ok(cedible.ObtenerCedibles());
            else
            {
                return NotFound();
            }

        }

        [HttpGet]
        [Route("cedible/{Doc_Cenabast}")]
        //POST : api/v1/admin/cedible/369854621
        /// <summary>
        /// Controlador que obtiene un Objeto DistribucionCedible a traves e el Doc_Cenabast
        /// </summary> 
        public IHttpActionResult Get_Cedible(int Doc_Cenabast)
        {
            DistribucionCedible cedible = new DistribucionCedible();

            if (cedible.Obtener(Doc_Cenabast) != null)
                return Ok(cedible.Obtener(Doc_Cenabast));
            else
            {
                return NotFound();
            }

        }
    }
}
