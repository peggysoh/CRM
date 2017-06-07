using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OgsysCRM.Models
{
    public class Note
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public DateTime DateCreated { get; set; }
        public int UserId { get; set; }
        public int CustomerId { get; set; }

        public virtual User User { get; set; }
        public virtual Customer Customer { get; set; }
    }
}