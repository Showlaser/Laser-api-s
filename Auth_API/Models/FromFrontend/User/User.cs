using System.ComponentModel.DataAnnotations;

namespace Auth_API.Models.FromFrontend.User
{
    public class User
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
