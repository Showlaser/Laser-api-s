using Auth_API.Models.Dto.User;

namespace Auth_API.Tests.UnitTests.TestModels
{
    public class TestUserDto
    {
        public readonly UserDto UserDto = new()
        {
            Uuid = Guid.Parse("4a4a4847-e081-40c8-a020-b5c2d4ccc00d"),
            Username = "TestUser",
            Password = "$argon2i$v=19$m=32768,t=10,p=5$CTiqgs+bcu4RdgqpH3q3uxmeUMvXrt/nWDbfhZSheQaWncvl9hK4A5PjdKl5s5Olc2+7txnCoCKCzimt2ykQ+w$3jO0jmbxYNUcIGZfUVOoF54b5gW/y75yZ0EioISMof4", // qwerty plaintext
            Email = "test@example.com123fewef",
            Salt = [9, 56, 170, 130, 207, 155, 114, 238, 17, 118, 10, 169, 31, 122, 183, 187, 25, 158, 80, 203, 215, 174, 223, 231, 88, 54, 223, 133, 148, 161, 121, 6, 150, 157, 203, 229, 246, 18, 184, 3, 147, 227, 116, 169, 121, 179, 147, 165, 115, 111, 187, 183, 25, 194, 160, 34, 130, 206, 41, 173, 219, 41, 16, 251],
        };
    }
}
