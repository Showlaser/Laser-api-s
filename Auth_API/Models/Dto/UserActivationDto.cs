namespace Auth_API.Models.Dto
{
    public class UserActivationDto
    {
        public Guid Uuid { get; set; }
        public Guid UserUuid { get; set; }
        public Guid Code { get; set; }
    }
}
