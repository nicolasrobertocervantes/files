using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DBProgramming_III.Models;

namespace DBProgramming_III.Controllers
{
    public class CustomersController : Controller
    {
        // GET: Customers
        public ActionResult Index(string searchTerm, int top = 10, int page = 1)
        {
            int skip = ((page - 1) * top);

            var context = new TechSupportEntities();
            List<Customer> customers = context.Customers.OrderByDescending(c => c.Name).Skip(skip).Take(top).ToList();
            
            ViewBag.searchTerm = searchTerm;
            int totalItems = context.Customers.Count();
            ViewBag.TotalItems = totalItems;
            ViewBag.page = page;
            ViewBag.top = top;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                customers = context.Customers.ToList();
                customers = customers.Where(c => c.Name.IndexOf(searchTerm) != -1 || 
                c.ZipCode.IndexOf(searchTerm) != -1 || c.City.IndexOf(searchTerm) != -1 || 
                c.State.IndexOf(searchTerm) != -1).ToList();

                ViewBag.searchCount = customers.Count();
            }

            return View(customers);
        }

        public ActionResult AddCustomer(int? id, int message = 0)
        {
            var context = new TechSupportEntities();
            Customer customer = context.Customers.FirstOrDefault(x => x.CustomerID == id);

            if (customer == null)
            {
                customer = new Customer();
            }

            if (message != 0)
            {
                ViewBag.SavedMessage = "Customer successfully saved.";
            }

            List<State> states = context.States.OrderBy(row => row.StateName).ToList();
            CustomerStateModel customerStateModel = new CustomerStateModel();
            customerStateModel.customer = customer;
            customerStateModel.states = states;

            return View(customerStateModel);
        }

        [HttpPost]
        public ActionResult AddCustomer(Customer customer)
        {
            var context = new TechSupportEntities();

            int message = 0;

            try
            {
                context.Customers.AddOrUpdate(customer);
                context.SaveChanges();

                message = 1;

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }

            return Redirect("/Customers/AddCustomer/" + customer.CustomerID + "/" + message);
        }
    }
}