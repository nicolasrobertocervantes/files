using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DBProgramming_III.Models;
using System.Globalization;

namespace DBProgramming_III.Controllers
{
    public class IncidentsController : Controller
    {
        // GET: Incidents
        public ActionResult Index(string searchTerm, int top = 10, int page = 1)
        {
            int skip = ((page - 1) * top);

            var context = new TechSupportEntities();
            
            List<Incident> incidents = context.Incidents.OrderByDescending(row => row.DateOpened).Skip(skip).Take(top).ToList();
            ViewBag.TotalIncidents = incidents.Count();

            ViewBag.searchTerm = searchTerm;
            int totalItems = context.Incidents.Count();
            ViewBag.TotalItems = totalItems;
            ViewBag.page = page;
            ViewBag.top = top;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                int j;
                bool isInt = Int32.TryParse(searchTerm, out j);

                incidents = incidents.Where(i => i.IncidentID == j || 
                i.CustomerID == j || i.TechID == j || i.Title.IndexOf(searchTerm)  != -1 || i.Customer.Name.IndexOf(searchTerm) != -1).ToList();

                ViewBag.searchCount = incidents.Count();
            }

            return View(incidents);
        }

        public ActionResult AddIncident(int? id, int message = 0)
        {
            var context = new TechSupportEntities();
            Incident incident = context.Incidents.FirstOrDefault(x => x.IncidentID == id);

            if (incident == null)
            {
                incident = new Incident();
                incident.DateOpened = DateTime.Now;
            }

            List<Customer> customers = context.Customers.OrderBy(row => row.CustomerID).ToList();
            List<Product> products = context.Products.OrderBy(row => row.ProductCode).ToList();
            List<Technician> technicians = context.Technicians.OrderBy(row => row.TechID).ToList();
            CombinedModels combinedModels = new CombinedModels();
            combinedModels.customers = customers;
            combinedModels.products = products;
            combinedModels.technicians = technicians;
            combinedModels.incident = incident;

            if (message != 0)
            {
                ViewBag.SavedMessage = "Incident successfully saved.";
            }

            return View(combinedModels);
        }

        [HttpPost]
        public ActionResult AddIncident(Incident incident)
        {
            var context = new TechSupportEntities();

            int message = 0;

            try
            {
                if (incident.DateOpened == DateTime.MinValue)
                {
                    incident.DateOpened = DateTime.Now.AddYears(-5);
                }

                if (Request.Form["closeIncident"] == null)
                {
                    incident.DateClosed = null;
                }
                else
                {
                    incident.DateClosed = DateTime.Now;
                }

                incident.ProductCode = incident.ProductCode.Trim();

                context.Incidents.AddOrUpdate(incident);
                context.SaveChanges();

                message = 1;

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }

            return Redirect("/Incidents/AddIncident/" + incident.IncidentID + "/" + message);
        }

        public ActionResult OpenIncidents(string searchTerm)
        {
            var context = new TechSupportEntities();
            List<Incident> openIncidents = context.Incidents.Where(i => !i.DateClosed.HasValue).ToList();
            ViewBag.TotalOpenIncidents = openIncidents.Count();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                int j;
                bool isInt = Int32.TryParse(searchTerm, out j);

                openIncidents = openIncidents.Where(i => i.IncidentID == j ||
                i.CustomerID == j || i.TechID == j || i.Title.IndexOf(searchTerm) != -1).ToList();
            }

            return View(openIncidents);
        }

        public ActionResult AssignedIncidents(string searchTerm)
        {
            var context = new TechSupportEntities();
            List<Incident> assignedIncidents = context.Incidents.Where(i => i.TechID.HasValue).ToList();
            ViewBag.TotalAssignedIncidents = assignedIncidents.Count();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                int j;
                bool isInt = Int32.TryParse(searchTerm, out j);

                assignedIncidents = assignedIncidents.Where(i => i.IncidentID == j ||
                i.CustomerID == j || i.TechID == j || i.Title.IndexOf(searchTerm) != -1).ToList();
            }

            return View(assignedIncidents);
        }

        public ActionResult UnAssignedIncidents(string searchTerm)
        {
            var context = new TechSupportEntities();
            List<Incident> unAssignedIncidents = context.Incidents.Where(i => !i.TechID.HasValue).ToList();
            ViewBag.TotalUnAssignedIncidents = unAssignedIncidents.Count();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                int j;
                bool isInt = Int32.TryParse(searchTerm, out j);

                unAssignedIncidents = unAssignedIncidents.Where(i => i.IncidentID == j ||
                i.CustomerID == j || i.TechID == j || i.Title.IndexOf(searchTerm) != -1).ToList();
            }

            return View(unAssignedIncidents);
        }
    }
}