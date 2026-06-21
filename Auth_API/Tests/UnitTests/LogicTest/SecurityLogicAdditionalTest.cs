using Auth_API.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Net;
using System.Security;

namespace Auth_API.Tests.UnitTests.LogicTest
{
    [TestClass]
    public class SecurityLogicAdditionalTest
    {
        public SecurityLogicAdditionalTest()
        {
            TestHelper.SetupTestEnvironment();
        }

        [TestMethod]
        public void GenerateNonceHasRequestedLengthAndCharsetTest()
        {
            const string allowedChars = "abcdefghijklmnopqrstuvwxyz123456789";
            string nonce = SecurityLogic.GenerateNonce();

            Assert.AreEqual(128, nonce.Length);
            Assert.IsTrue(nonce.All(c => allowedChars.Contains(c)));
        }

        [TestMethod]
        public void GenerateNonceIsDifferentEachCallTest()
        {
            Assert.AreNotEqual(SecurityLogic.GenerateNonce(), SecurityLogic.GenerateNonce());
        }

        [TestMethod]
        public void GenerateCodeChallengeIsDeterministicAndUrlSafeTest()
        {
            const string verifier = "abc123abc123abc123abc123abc123abc123abc1";
            string challenge = SecurityLogic.GenerateCodeChallenge(verifier);

            Assert.AreEqual(challenge, SecurityLogic.GenerateCodeChallenge(verifier));
            Assert.IsFalse(challenge.Contains('+'));
            Assert.IsFalse(challenge.Contains('/'));
            Assert.IsFalse(challenge.EndsWith("="));
        }

        [TestMethod]
        public void GenerateRefreshTokenReturnsFutureExpiryAndTokenTest()
        {
            Guid uuid = Guid.NewGuid();
            IPAddress ip = IPAddress.Parse("127.0.0.1");

            Models.Dto.Tokens.UserTokensDto token = SecurityLogic.GenerateRefreshToken(uuid, ip);

            Assert.AreEqual(uuid, token.UserUuid);
            Assert.AreEqual(ip, token.ClientIp);
            Assert.IsTrue(token.RefreshToken.Length > 25);
            Assert.IsTrue(token.RefreshTokenExpireDate > DateTime.UtcNow);
        }

        [TestMethod]
        public void HashPasswordWithTooShortSaltThrowsTest()
        {
            Assert.Throws<NoNullAllowedException>(() => SecurityLogic.HashPassword("password", new byte[10]));
        }

        [TestMethod]
        public void HashPasswordUsesSaltSoEqualPasswordsHashDifferentlyTest()
        {
            string firstHash = SecurityLogic.HashPassword("samePassword", SecurityLogic.GetSalt());
            string secondHash = SecurityLogic.HashPassword("samePassword", SecurityLogic.GetSalt());

            Assert.AreNotEqual(firstHash, secondHash);
        }

        [TestMethod]
        public void ValidatePasswordWithWrongPasswordThrowsTest()
        {
            byte[] salt = SecurityLogic.GetSalt();
            string hash = SecurityLogic.HashPassword("correctPassword", salt);

            Assert.Throws<SecurityException>(() => SecurityLogic.ValidatePassword(hash, salt, "wrongPassword"));
        }
    }
}
