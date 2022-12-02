using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DBProgramming_III
{
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

            routes.MapRoute(
                name: "Registration",
                url: "Registrations/AddRegistration/{id}/{code}/{message}",
                defaults: new { controller = "Registrations", action = "AddRegistration", message = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "AddIncident",
                url: "Incidents/AddIncident/{id}/{message}",
                defaults: new { controller = "Incidents", action = "AddIncident", message = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "AddCustomer",
                url: "Customers/AddCustomer/{id}/{message}",
                defaults: new { controller = "Customers", action = "AddCustomer", message = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "AddProduct",
                url: "Products/AddProduct/{id}/{message}",
                defaults: new { controller = "Products", action = "AddProduct", message = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "AddTechnician",
                url: "Technicians/AddTechnician/{id}/{message}",
                defaults: new { controller = "Technicians", action = "AddTechnician", message = UrlParameter.Optional }
            );
        }
    }
}
