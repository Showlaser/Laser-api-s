namespace Auth_API.Models.Dto
{
    public class PasswordResetDto
    {
        public Guid Uuid { get; set; }
        public Guid UserUuid { get; set; }
        public Guid Code { get; set; }
    }
}
