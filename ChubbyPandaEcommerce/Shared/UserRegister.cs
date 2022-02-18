using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChubbyPandaEcommerce.Shared
{
    public class UserRegister
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, StringLength(maximumLength: 100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [Compare("Password",ErrorMessage = "Please insure that the passwords match")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
