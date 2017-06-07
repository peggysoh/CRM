using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OgsysCRM.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int AddressId { get; set; }
        
        public virtual Address Address { get; set; }
        public virtual ICollection<Note> Notes { get; set; }
    }
}