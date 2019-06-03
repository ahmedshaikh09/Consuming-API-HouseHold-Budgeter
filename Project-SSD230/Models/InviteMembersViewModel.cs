using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project_SSD230.Models
{
    public class InviteMembersViewModel
    {
        [Required]
        [EmailAddress]
        public string UserEmail { get; set; }
    }
}