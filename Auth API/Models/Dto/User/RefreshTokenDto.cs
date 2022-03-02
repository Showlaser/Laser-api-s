namespace Auth_API.Models.Dto.User
{
    public class RefreshTokenDto
    {
        public Guid Uuid { get; set; }
        public Guid UserUuid { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
