using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChubbyPandaEcommerce.Shared
{
    public class UserChangePassword
    {
        [Required, StringLength(maximumLength:100,MinimumLength =6)]
        public string Password { get; set; } = string.Empty;

        [Required,Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
