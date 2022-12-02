using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBProgramming_III.Models
{
    public class RegistrationCombinedModel
    {
        public List<Customer> customers = new List<Customer>();
        public List<Product> products = new List<Product>();
        public Registration registration = new Registration();
    }
}