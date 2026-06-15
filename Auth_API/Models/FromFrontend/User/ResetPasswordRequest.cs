using System.ComponentModel.DataAnnotations;

namespace Auth_API.Models.FromFrontend.User
{
    public class ResetPasswordRequest
    {
        [Required]
        public Guid Code { get; set; }
        [Required]
        public string NewPassword { get; set; } = string.Empty;
    }
}
