using System;
using System.Collections.Generic;

namespace OgsysCRM.Models
{
    public class NoteViewModel
    {
        public Note Note { get; set; }
        public List<Customer> Customers {get;set; }
        public List<User> Users { get; set; }
        public string CreatedBy { get; set; }
        public string DateCreated { get; set; }

        public NoteViewModel()
        {
        }

        public NoteViewModel(Note note, List<Customer> customers)
        {
            Note = note;
            Customers = customers;
        }
    }
}