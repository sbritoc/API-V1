using System;
using System.Net;
using System.Threading;
using System.Web.Http;
using WebApi.Models;
using WebApi.Security;

namespace WebApi.Controllers
{
    /// <summary>
    /// login controller class for authenticate users
    /// </summary>
    [AllowAnonymous]
    [RoutePrefix("api/v1/login")]
    public class LoginController : ApiController
    {
        [HttpGet]
        [Route("echoping")]
        public IHttpActionResult EchoPing()
        {
            return Ok(true);
        }

        [HttpGet]
        [Route("echouser")]
        public IHttpActionResult EchoUser()
        {
            var identity = Thread.CurrentPrincipal.Identity;
            return Ok($" IPrincipal-user: {identity.Name} - IsAuthenticated: {identity.IsAuthenticated}");
        }

        [HttpPost]
        [Route("authenticate")]
        /// <summary>
        /// login controller class for authenticate users
        /// </summary>
        /// <remarks>
        /// Este controlador sive para authenticar a los usuarios que utilicen la API
        /// </remarks>
        public IHttpActionResult Authenticate(LoginRequest login)
        {
            if (login == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            //TODO: This code is only for demo - extract method in new class & validate correctly in your application !!
            var isUserValid = (login.GetProveedor(login.Username, login.Password));
            if (isUserValid)
            {
                var rolename = "Proveedor";
                var token = TokenGenerator.GenerateTokenJwt(login.Username, rolename);
                return Ok(token);
            }

            //TODO: This code is only for demo - extract method in new class & validate correctly in your application !!
            /*var isTesterValid = (login.Username == "test" && login.Password == "123456");
            if (isTesterValid)
            {
                var rolename = "Tester";
                var token = TokenGenerator.GenerateTokenJwt(login.Username, rolename);
                return Ok(token);
            }
            */
            //TODO: This code is only for demo - extract method in new class & validate correctly in your application !!
            //var isAdminValid = (login.Username == "admin" && login.Password == "123456");
            var isAdminValid = (login.GetAdministrador(login.Username, login.Password));
            if (isAdminValid)
            {
                var rolename = "Administrador";
                var token = TokenGenerator.GenerateTokenJwt(login.Username, rolename);
                return Ok(token);
            }

            // Unauthorized access 
            return Unauthorized();
        }
    }
}
