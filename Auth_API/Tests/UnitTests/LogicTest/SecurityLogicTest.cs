using Auth_API.Logic;
using Auth_API.Tests.IntegrationTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Security.Cryptography;

namespace Auth_API.Tests.UnitTests.LogicTest
{
    [TestClass]
    public class SecurityLogicTest
    {
        private readonly byte[] _salt;

        public SecurityLogicTest()
        {
            TestHelper.SetupTestEnvironment();
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            byte[] salt = new byte[64];
            rng.GetBytes(salt);
            _salt = salt;
        }

        [TestMethod]
        public void HashPasswordTest()
        {
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            byte[] salt = new byte[64];
            rng.GetBytes(salt);

            string password = SecurityLogic.HashPassword("123", salt);
            Assert.IsTrue(password.Length > 5);
        }

        [TestMethod]
        public void HashEmptyPasswordTest()
        {
            Assert.ThrowsException<NoNullAllowedException>(() => SecurityLogic.HashPassword("", Array.Empty<byte>()));
        }

        [TestMethod]
        public void SaltTest()
        {
            List<byte> salt = SecurityLogic.GetSalt().ToList();
            Assert.IsTrue(salt.Count(b => b != 0) > 50);
        }

        [TestMethod]
        public void ValidatePasswordTest()
        {
            string password = "123";
            string hash = SecurityLogic.HashPassword(password, _salt);
            SecurityLogic.ValidatePassword(hash, _salt, password);
        }
    }
}
