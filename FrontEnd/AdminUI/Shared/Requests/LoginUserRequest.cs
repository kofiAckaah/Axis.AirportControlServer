using System.ComponentModel.DataAnnotations;

namespace AdminUI.Shared.Requests
{
    public class LoginUserRequest
    {
        [Required]
        public string Username { get; set;}
        [Required]
        public string Password { get; set;}
    }
}
