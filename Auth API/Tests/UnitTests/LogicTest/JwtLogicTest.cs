using Auth_API.Logic;
using Auth_API.Tests.TestModels;
using Auth_API.Tests.UnitTests;
using NUnit.Framework;
using System.Security.Claims;

namespace Auth_API.Tests.LogicTest
{
    [TestFixture]
    public class JwtLogicTest
    {
        private readonly TestRefreshTokenDto _testRefreshToken;

        public JwtLogicTest()
        {
            _testRefreshToken = new TestRefreshTokenDto();
            TestHelper.SetEnvironmentVariables();
        }

        [Test]
        public void GenerateJwtTest()
        {
            string jwt = JwtLogic.GenerateJwtToken(_testRefreshToken.RefreshToken.UserUuid);
            Assert.IsNotNull(jwt);
            Assert.IsTrue(jwt.Length > 25);
        }

        [Test]
        public void GetJwtClaimsTest()
        {
            string jwt = JwtLogic.GenerateJwtToken(_testRefreshToken.RefreshToken.UserUuid);
            IEnumerable<Claim> claims = JwtLogic.GetJwtClaims(jwt);
            Assert.IsTrue(claims.Any());
        }

        [Test]
        public void GetEmptyJwtClaimsTest()
        {
            IEnumerable<Claim> claims = JwtLogic.GetJwtClaims("");
            Assert.IsFalse(claims.Any());
        }

        [Test]
        public void ValidateJwtToken()
        {
            string jwt = JwtLogic.GenerateJwtToken(_testRefreshToken.RefreshToken.UserUuid);
            bool jwtValid = JwtLogic.ValidateJwtToken(jwt);
            Assert.IsTrue(jwtValid);
        }

        [Test]
        public void ValidateEmptyJwtToken()
        {
            bool jwtValid = JwtLogic.ValidateJwtToken("");
            Assert.IsFalse(jwtValid);
        }
    }
}
