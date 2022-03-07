using Auth_API.Interfaces.Dal;
using Auth_API.Tests.UnitTests.TestModels;
using Moq;

namespace Auth_API.Tests.MockedDals
{
    public class MockedUserDal
    {
        public readonly IUserDal UserDal;

        public MockedUserDal()
        {
            TestUserDto testUser = new();
            Mock<IUserDal> userDal = new();
            userDal.Setup(ud => ud.Find(testUser.UserDto.Uuid)).ReturnsAsync(testUser.UserDto);
            userDal.Setup(ud => ud.Find(testUser.UserDto.UserName)).ReturnsAsync(testUser.UserDto);
            UserDal = userDal.Object;
        }
    }
}
