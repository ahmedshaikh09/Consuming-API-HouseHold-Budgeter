using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_SSD230.Models
{
    public class BankAccount
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Balance { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}