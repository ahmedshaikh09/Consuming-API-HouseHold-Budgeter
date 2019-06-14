using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project_SSD230.Models
{
    public class TransactionCreateEditViewModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public string Description { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }
    }
}