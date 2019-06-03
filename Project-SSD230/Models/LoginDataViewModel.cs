using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project_SSD230.Models
{
    public class LoginDataViewModel
    {
        [Required]
        [EmailAddress]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string GrantType { get; set; }

        public LoginDataViewModel()
        {
            GrantType = "password";
        }
    }
}