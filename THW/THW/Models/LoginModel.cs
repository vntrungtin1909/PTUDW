using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace THW.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Không được để trống")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}