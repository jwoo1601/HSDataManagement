using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.InputModels
{
    public class RegisterInputModel
    {
        [Required(ErrorMessage = "username.required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "password.required")]
        [StringLength(100, ErrorMessage = "Password must contain at least {2} characters", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "confirmPassword.required")]
        [Compare("Password", ErrorMessage = "confirmPassword.notEqual")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "name.required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "email.required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "emailDomain.required")]
        public string EmailDomain { get; set; }
        [Required(ErrorMessage = "securityCode.required")]
        public string SecurityCode { get; set; }

        [EmailAddress(ErrorMessage = "email.invalid")]
        public string EmailAddress
            => $"{Email}@{EmailDomain}";
    }
}
