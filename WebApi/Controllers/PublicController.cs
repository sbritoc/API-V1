using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using System.Xml;
using WebApi.Models;

namespace WebApi.Controllers
{
    /// <summary>
    /// customer controller class for testing security token 
    /// </summary>
    [RoutePrefix("api/v1/Public")]
    public class PublicController : ApiController
    {

        [HttpGet]
        [Route("distribucion/{Doc_Cenabast}")]
        //GET : api/v1/distribucion/369854621
        /// <summary>
        /// Controlador que obtiene un Objeto distribucion filtrado por el Doc_Cenabast
        /// </summary> 
        public IHttpActionResult GetId(int Doc_Cenabast)
        {
            Distribucion distribucion = new Distribucion();
            distribucion = distribucion.Obtener(Doc_Cenabast);
            if (distribucion != null)
            {
                return Ok(distribucion);
            }
            else {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("distribucion")]
        //GET : api/v1/distribuciones
        /// <summary>
        /// Controlador que obtiene todas las distribuciones en los registros
        /// </summary> 
        public IHttpActionResult GetAll()
        {
            Distribucion distribucion = new Distribucion();

            if (distribucion.ObtenerDistribuciones() != null)
                return Ok(distribucion.ObtenerDistribuciones());
            else {
                return NotFound();
            }

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
            else {
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
            else {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("cedible")]
        //POST : api/v1/public/cedible
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
        //POST : api/v1/public/cedible/369854621
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
