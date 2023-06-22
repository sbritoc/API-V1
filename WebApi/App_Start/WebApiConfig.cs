using System;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApi.Security;

namespace WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Configuración de rutas y servicios de API
            config.MapHttpAttributeRoutes();

            config.MessageHandlers.Add(new TokenValidationHandler());

            config.Routes.MapHttpRoute(
                name: "WebApi",
                routeTemplate: @"api/v1/{controller}/{id}",
               //routeTemplate: "{controller}/{action}/{id}",
               //defaults: new { controller = "Index", action = "Index",  id = RouteParameter.Optional}
               defaults: new { id = RouteParameter.Optional }
            );

            // Adding formatter for Json   
            config.Formatters.JsonFormatter.MediaTypeMappings.Add(
                new QueryStringMapping("type", "json", new MediaTypeHeaderValue("application/json")));

            // Adding formatter for XML   
            config.Formatters.XmlFormatter.MediaTypeMappings.Add(
                new QueryStringMapping("type", "xml", new MediaTypeHeaderValue("application/xml")));

            //deshabilitar cors
            //var cors = new EnableCorsAttribute("http://localhost:3000", "*", "*");
            //config.EnableCors(cors);

        }
    }
}
