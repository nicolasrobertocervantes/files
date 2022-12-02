using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DBProgramming_III.Models;

namespace DBProgramming_III.Controllers
{
    public class RegistrationsController : Controller
    {
        // GET: Registrations
        public ActionResult Index(string searchTerm, int top = 10, int page = 1)
        {
            int skip = ((page - 1) * top);

            var context = new TechSupportEntities();

            List<Registration> registrations = context.Registrations.OrderByDescending(r => r.RegistrationDate).Skip(skip).Take(top).ToList();

            ViewBag.searchTerm = searchTerm;
            int totalItems = context.Registrations.Count();
            ViewBag.TotalItems = totalItems;
            ViewBag.page = page;
            ViewBag.top = top;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                registrations = context.Registrations.ToList();
                registrations = registrations.Where(i => i.Customer.Name.IndexOf(searchTerm) != -1 ||
                i.Product.ProductCode.IndexOf(searchTerm) != -1 || i.Product.Name.IndexOf(searchTerm) != -1).ToList();

                ViewBag.searchCount = registrations.Count();
            }

            return View(registrations);
        }

        public ActionResult AddRegistration(int? id, string code, int message = 0)
        {
            var context = new TechSupportEntities();
            Registration registration = context.Registrations.FirstOrDefault(x => x.CustomerID == id && x.ProductCode == code);

            if (registration == null)
            {
                registration = new Registration();
                registration.RegistrationDate = DateTime.Now;
            }

            List<Customer> customers = context.Customers.OrderBy(row => row.CustomerID).ToList();
            List<Product> products = context.Products.OrderBy(row => row.ProductCode).ToList();
            RegistrationCombinedModel combinedModels = new RegistrationCombinedModel();
            combinedModels.customers = customers;
            combinedModels.products = products;
            combinedModels.registration = registration;

            if (message != 0)
            {
                ViewBag.SavedMessage = "Registration successfully saved.";
            }

            return View(combinedModels);
        }

        [HttpPost]
        public ActionResult AddRegistration(Registration registration)
        {
            var context = new TechSupportEntities();
            var pdCode = "";
            int message = 0;

            pdCode = registration.ProductCode.Trim();
            registration.ProductCode = pdCode;

            try
            {
                context.Registrations.AddOrUpdate(registration);
                context.SaveChanges();

                message = 1;

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }

            return Redirect("/Registrations/AddRegistration/" + registration.CustomerID + "/" + pdCode + "/" + message);
        }
    }
}