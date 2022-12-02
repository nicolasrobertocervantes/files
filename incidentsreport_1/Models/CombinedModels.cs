using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBProgramming_III.Models
{
    public class CombinedModels
    {
        public Incident incident = new Incident();
        public List<Customer> customers = new List<Customer>();
        public List<Product> products = new List<Product>();
        public List<Registration> registrations = new List<Registration>();
        public List<Technician> technicians = new List<Technician>();
    }
}