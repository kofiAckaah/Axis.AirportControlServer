using System.ComponentModel.DataAnnotations;

namespace AdminUI.Shared.Requests
{
    public class RegisterUserRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match!")]
        public string ConfirmPassword { get; set; }
    }
}
