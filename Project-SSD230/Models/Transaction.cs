using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_SSD230.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }

        public DateTime TransactionDate { get; set; }
        public bool Void { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}