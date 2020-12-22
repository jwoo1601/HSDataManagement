using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.InputModels
{
    public class LoginInputModel
    {
        [Required(ErrorMessage = "username.required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "password.required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string RememberLogin { get; set; }
        public string RedirectUrl { get; set; }
        public string Action { get; set; }

        public bool AllowRememberLogin
            => !string.IsNullOrEmpty(RememberLogin);
        public bool IsCancellationRequested
            => Action != "login";
    }
}
