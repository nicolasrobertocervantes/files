using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBProgramming_III.Models
{
    public class IncidentCustomerModel
    {
        public List<Customer> customers = new List<Customer>();
        public Incident incident = new Incident();
    }
}