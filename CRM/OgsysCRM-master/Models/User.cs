﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OgsysCRM.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }

        public virtual ICollection<Note> Notes { get; set; }
    }
}