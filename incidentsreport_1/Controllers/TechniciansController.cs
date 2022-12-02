using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DBProgramming_III.Models;

namespace DBProgramming_III.Controllers
{
    public class TechniciansController : Controller
    {
        // GET: Technicians
        public ActionResult Index(string searchTerm)
        {
            var context = new TechSupportEntities();

            List<Technician> technicians = context.Technicians.ToList();
            ViewBag.TotalTechinicians = technicians.Count();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                int j;
                bool isInt = Int32.TryParse(searchTerm, out j);

                technicians = technicians.Where(i => i.TechID == j ||
                 i.Name.IndexOf(searchTerm) != -1).ToList();
            }

            return View(technicians);
        }

        public ActionResult AddTechnician(int? id)
        {
            var context = new TechSupportEntities();
            Technician technician = context.Technicians.FirstOrDefault(t => t.TechID == id);

            if (technician == null)
            {
                technician = new Technician();
            }

            return View(technician);
        }

        [HttpPost]
        public ActionResult AddTechnician(Technician technician)
        {
            var context = new TechSupportEntities();

            try
            {
                context.Technicians.AddOrUpdate(technician);
                context.SaveChanges();

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }

            return Redirect("/Technicians/AddTechnician/" + technician.TechID);
        }
    }
}