using System.ComponentModel.DataAnnotations;

namespace Auth_API.Models.FromFrontend.User
{
    public class ForgotPasswordRequest
    {
        [Required]
        public string Email { get; set; } = string.Empty;
    }
}
