using Auth_API.Logic;
using NUnit.Framework;
using System.Data;
using System.Security.Cryptography;

namespace Auth_API.Tests.LogicTest
{
    [TestFixture]
    public class SecurityLogicTest
    {
        private readonly byte[] _salt;

        public SecurityLogicTest()
        {
            Environment.SetEnvironmentVariable("ARGON2SECRET", "hhjwe093892349jsdfwe");
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            byte[] salt = new byte[64];
            rng.GetBytes(salt);
            _salt = salt;
        }

        [Test]
        public void HashPasswordTest()
        {
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            byte[] salt = new byte[64];
            rng.GetBytes(salt);

            string password = SecurityLogic.HashPassword("123", salt);
            Assert.IsTrue(password.Length > 5);
        }

        [Test]
        public void HashEmptyPasswordTest()
        {
            Assert.Throws<NoNullAllowedException>(() => SecurityLogic.HashPassword("", Array.Empty<byte>()));
        }

        [Test]
        public void ValidatePasswordTest()
        {
            string password = "123";
            string hash = SecurityLogic.HashPassword(password, _salt);
            bool passwordValid = SecurityLogic.ValidatePassword(hash, _salt, password);
            Assert.IsTrue(passwordValid);
        }
    }
}
