using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBProgramming_III.Models
{
    public class CustomerStateModel
    {
        public List<State> states = new List<State>();
        public Customer customer = new Customer();
    }
}