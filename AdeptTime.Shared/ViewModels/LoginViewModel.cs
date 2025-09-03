using System;
using System.ComponentModel.DataAnnotations;

namespace AdeptTime.Shared.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }

        public bool ValidateLogin()
        {
            // Mock validation - always return true for now
            return true;
        }
    }
}