using System.ComponentModel.DataAnnotations;

namespace Auth_API.Models.FromFrontend.User
{
    public class AccountActivationRequest
    {
        [Required]
        public Guid Code { get; set; }
    }
}
