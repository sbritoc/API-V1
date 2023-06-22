using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web.Http;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Authorize(Roles = "Proveedor")]
    [RoutePrefix("api/v1/proveedor")]
    public class ProveedorController : ApiController
    {
        [HttpPost]
        [Route("distribucion")]
        //POST : api/v1/proveedor/distribucion
        /// <summary>
        /// Controlador que recibe un Objeto Distribucion para ingresar la infromacion logistica
        /// </summary> 
        public IHttpActionResult Post(Distribucion distribucion)
        {
            if (ModelState.IsValid) 
            {
                if ( (distribucion.Factura==0 ) && (distribucion.Guia==0)) { 
                    return BadRequest("Debe al menos informar uno de los dos documentos relacionados: Factura o Guía.");
                }
                DistribucionDetalle det = new DistribucionDetalle();
                DistribucionMovimiento mov = new DistribucionMovimiento();
                IPrincipal currentPrincipal = Thread.CurrentPrincipal;
                if (currentPrincipal.Identity.Name == distribucion.Rut_Proveedor)
                {
                    if (distribucion.Obtener(distribucion.Doc_Cenabast) == null)
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
                    return BadRequest("Violacion de Seguridad - Suplantacion de Identidad. Revise su informacion.");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }

        }

        [HttpPost]
        [Route("distribucion/{Doc_Cenabast}/movimiento")]
        //POST : api/v1/proveedor/distribucion/369854621/movimiento
        /// <summary>
        /// Controlador que ingresa los movimientos de una distribucion
        /// </summary> 
        public IHttpActionResult Post_movimiento(int Doc_Cenabast, DistribucionMovimiento movimiento)
        {
           
            if (ModelState.IsValid)
            {
                IPrincipal currentPrincipal = Thread.CurrentPrincipal;
                Distribucion distribucion = new Distribucion();
                distribucion = distribucion.Obtener(movimiento.Doc_Cenabast);
                if (currentPrincipal.Identity.Name == distribucion.Rut_Proveedor)
                {
                    if (Doc_Cenabast == movimiento.Doc_Cenabast)
                    {
                        if (movimiento.ObtenerMovimientos(Doc_Cenabast) != null)
                        {
                            int validar = movimiento.Ingresar(movimiento);
                            if (validar > 0)
                            {
                                return Created("/api/v1/Public/distribucion/" + Doc_Cenabast + "/movimiento", new { Message = "El movimiento ha sido registrado." });
                            }
                            else
                            {
                                return BadRequest("No se ha podido registrar el movimiento.");
                            }
                        }
                        else
                        {
                            return BadRequest("No se a ingresado una distribucion con este Documento de Venta.");
                        }
                    }
                    else {
                        return BadRequest("La Url contine el Doc_Cenabast "+ Doc_Cenabast + "y la estructura "+movimiento.Doc_Cenabast +" lo que provoca inconsistencia."  );
                    }
                }
                else
                {
                    return BadRequest("Violacion de Seguridad - Suplantacion de Identidad. Revise su informacion.");
                }
            }
            else {
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Route("cedible")]
        //POST : api/v1/proveedor/cedible
        /// <summary>
        /// Controlador que recibe un Objeto DistribucionCedible para ingresar
        /// </summary> 
        public IHttpActionResult Post_Cedible(DistribucionCedible cedible)
        {
            
            if (ModelState.IsValid)
            {
                IPrincipal currentPrincipal = Thread.CurrentPrincipal;
                Distribucion distribucion = new Distribucion();
                distribucion = distribucion.Obtener(cedible.Doc_Cenabast);
                if (currentPrincipal.Identity.Name == cedible.Rut_Proveedor)
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
                            //}
                            /*else
                            {
                                return BadRequest("El Cedible ya fue registrado.");
                            }*/
                        }
                        else
                        {
                            return BadRequest("No es un Documento Valido");
                        }
                    }
                    else
                    {
                        return BadRequest("No se ha ingresado informacion válida.");
                    }
                }
                else
                {
                    return BadRequest("Violacion de Seguridad - Suplantacion de Identidad. Revise su informacion.");
                }
            }
            else
            {
                return BadRequest(ModelState);
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
                IPrincipal currentPrincipal = Thread.CurrentPrincipal;
                if (currentPrincipal.Identity.Name == distribucion.Rut_Proveedor)
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
                else
                {
                    return BadRequest("Violacion de Seguridad - Suplantacion de Identidad. Revise su informacion.");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }

        }
    }
}
