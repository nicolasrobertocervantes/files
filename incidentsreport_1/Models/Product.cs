//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DBProgramming_III.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        public Product()
        {
            this.Incidents = new HashSet<Incident>();
            this.Registrations = new HashSet<Registration>();
        }
    
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public decimal Version { get; set; }
        public System.DateTime ReleaseDate { get; set; }
    
        public virtual ICollection<Incident> Incidents { get; set; }
        public virtual ICollection<Registration> Registrations { get; set; }
    }
}
