using Auth_API.CustomExceptions;
using Auth_API.Enums;
using Auth_API.Interfaces.Dal;
using Auth_API.Logic;
using Auth_API.Models.Dto;
using Auth_API.Models.Dto.User;
using Auth_API.Tests.UnitTests.MockedLogics;
using Auth_API.Tests.UnitTests.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Data;
using System.Net;
using System.Security;

namespace Auth_API.Tests.UnitTests.LogicTest
{
    [TestClass]
    public class UserLogicAdditionalTest
    {
        private readonly UserLogic _userLogic;
        private readonly TestUserDto _testUser = new();

        public UserLogicAdditionalTest()
        {
            TestHelper.SetupTestEnvironment();
            _userLogic = new MockedUserLogic().UserLogic;
        }

        [TestMethod]
        public async Task LoginUnknownUserThrowsSecurityExceptionTest()
        {
            await Assert.ThrowsAsync<SecurityException>(async () =>
                await _userLogic.Login(new UserDto { Username = "doesNotExist", Password = "qwerty" },
                    IPAddress.Parse("127.0.0.1")));
        }

        [TestMethod]
        public async Task AddUserWithMissingFieldsThrowsInvalidDataTest()
        {
            // ValidateUser requires a non-empty password, email and username
            await Assert.ThrowsAsync<InvalidDataException>(async () =>
                await _userLogic.Add(new UserDto { Username = "user", Password = "", Email = "user@example.com" }));
        }

        [TestMethod]
        public async Task LoginDisabledUserThrowsUserDisabledExceptionTest()
        {
            Mock<IUserDal> userDal = new();
            userDal.Setup(d => d.Find(_testUser.UserDto.Username)).ReturnsAsync(_testUser.UserDto);

            Mock<IDisabledUserDal> disabledUserDal = new();
            disabledUserDal.Setup(d => d.Find(_testUser.UserDto.Uuid)).ReturnsAsync(new DisabledUserDto
            {
                Uuid = Guid.NewGuid(),
                UserUuid = _testUser.UserDto.Uuid,
                DisabledReason = DisabledReason.EmailNeedsToBeValidated
            });

            UserLogic userLogic = new(userDal.Object, new Mock<IUserTokenDal>().Object,
                new Mock<IUserActivationDal>().Object, disabledUserDal.Object);

            await Assert.ThrowsAsync<UserDisabledException>(async () =>
                await userLogic.Login(new UserDto { Username = _testUser.UserDto.Username, Password = "qwerty" },
                    IPAddress.Parse("127.0.0.1")));
        }

        [TestMethod]
        public async Task FindReturnsUserForKnownUuidTest()
        {
            UserDto? user = await _userLogic.Find(_testUser.UserDto.Uuid);

            Assert.IsNotNull(user);
            Assert.AreEqual(_testUser.UserDto.Username, user.Username);
        }

        [TestMethod]
        public async Task RequestPasswordResetForUnknownEmailDoesNotThrowTest()
        {
            // No user enumeration: an unknown email must succeed silently
            Mock<IUserDal> userDal = new();
            userDal.Setup(d => d.FindByEmail(It.IsAny<string>())).ReturnsAsync((UserDto?)null);

            UserLogic userLogic = new(userDal.Object, new Mock<IUserTokenDal>().Object,
                new Mock<IUserActivationDal>().Object, new Mock<IDisabledUserDal>().Object);

            await userLogic.RequestPasswordReset("unknown@example.com");
        }

        [TestMethod]
        public async Task ResetPasswordWithExpiredCodeThrowsTest()
        {
            Guid code = Guid.NewGuid();
            Mock<IUserActivationDal> activationDal = new();
            activationDal.Setup(d => d.Find(code)).ReturnsAsync(new UserActivationDto
            {
                Uuid = Guid.NewGuid(),
                UserUuid = _testUser.UserDto.Uuid,
                Code = code,
                ExpirationDate = DateTime.UtcNow.AddHours(-1)
            });

            UserLogic userLogic = new(new Mock<IUserDal>().Object, new Mock<IUserTokenDal>().Object,
                activationDal.Object, new Mock<IDisabledUserDal>().Object);

            await Assert.ThrowsAsync<Microsoft.IdentityModel.Tokens.SecurityTokenExpiredException>(async () =>
                await userLogic.ResetPassword(code, "newPassword123"));
        }

        [TestMethod]
        public async Task ResetPasswordWithEmptyPasswordThrowsTest()
        {
            UserLogic userLogic = new(new Mock<IUserDal>().Object, new Mock<IUserTokenDal>().Object,
                new Mock<IUserActivationDal>().Object, new Mock<IDisabledUserDal>().Object);

            await Assert.ThrowsAsync<InvalidDataException>(async () =>
                await userLogic.ResetPassword(Guid.NewGuid(), ""));
        }

        [TestMethod]
        public async Task UpdateToUsernameOfAnotherUserThrowsDuplicateTest()
        {
            Guid currentUserUuid = _testUser.UserDto.Uuid;
            UserDto otherUser = new()
            {
                Uuid = Guid.NewGuid(),
                Username = "takenUsername",
                Email = "other@example.com"
            };

            Mock<IUserDal> userDal = new();
            userDal.Setup(d => d.Find(currentUserUuid)).ReturnsAsync(_testUser.UserDto);
            // The desired new username is already owned by a different user
            userDal.Setup(d => d.Find(otherUser.Username)).ReturnsAsync(otherUser);

            UserLogic userLogic = new(userDal.Object, new Mock<IUserTokenDal>().Object,
                new Mock<IUserActivationDal>().Object, new Mock<IDisabledUserDal>().Object);

            await Assert.ThrowsAsync<DuplicateNameException>(async () =>
                await userLogic.Update(new UserDto
                {
                    Uuid = currentUserUuid,
                    Username = otherUser.Username,
                    Email = _testUser.UserDto.Email,
                    Password = "qwerty"
                }, string.Empty));
        }
    }
}
