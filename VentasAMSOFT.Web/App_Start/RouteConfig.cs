﻿using System.Web.Mvc;
using System.Web.Routing;

namespace VentasAMSOFT.Web
{
    /// <summary>
    /// Clase encargada del enrutamiento de peticiones.
    /// </summary>
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
