using Auth_API.Enums;

namespace Auth_API.Models.Dto
{
    public class DisabledUserDto
    {
        public Guid Uuid { get; set; }
        public Guid UserUuid { get; set; }
        public DisabledReason DisabledReason { get; set; }
    }
}
