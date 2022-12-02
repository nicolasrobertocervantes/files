using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DBProgramming_III.Models;

namespace DBProgramming_III.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            
            var context = new TechSupportEntities();
            List<Incident> incidents = context.Incidents.ToList();
            ViewBag.TotalIncidents = incidents.Count();

            List<Incident> openIncidents = context.Incidents.Where(i => !i.DateClosed.HasValue).ToList();
            ViewBag.OpenIncidents = openIncidents.Count();

            List<Customer> customers = context.Customers.ToList();
            ViewBag.TotalCustomers = customers.Count();

            List<Product> products = context.Products.ToList();
            ViewBag.TotalProducts = products.Count();

            List<Incident> assignedIncidents = context.Incidents.Where(j => j.TechID.HasValue).ToList();
            ViewBag.TotalAssignedIncidents = assignedIncidents.Count();

            List<Incident> unAssignedIncidents = context.Incidents.Where(j => !j.TechID.HasValue).ToList();
            ViewBag.TotalUnAssignedIncidents = unAssignedIncidents.Count();

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}