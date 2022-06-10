using Auth_API.Models.FromFrontend.User;

namespace Auth_API.Tests.IntegrationTests.TestModels
{
    public class TestUser
    {
        public readonly User User = new()
        {
            Username = "TestUser",
            Password = "qwerty",
            Email = "test@example.com123fewef"
        };
    }
}
