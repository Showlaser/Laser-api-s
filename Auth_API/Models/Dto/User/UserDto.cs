namespace Auth_API.Models.Dto.User
{
    public class UserDto
    {
        public Guid Uuid { get; set; }
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public byte[] Salt { get; set; } = Array.Empty<byte>();
    }
}
