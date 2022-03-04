using Auth_API.Logic;
using Auth_API.Tests.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Claims;

namespace Auth_API.Tests.LogicTest
{
    [TestClass]
    public class JwtLogicTest
    {
        private readonly TestRefreshTokenDto _testRefreshToken;

        public JwtLogicTest()
        {
            _testRefreshToken = new TestRefreshTokenDto();
            TestHelper.SetEnvironmentVariables();
        }

        [TestMethod]
        public void GenerateJwtTest()
        {
            string jwt = JwtLogic.GenerateJwtToken(_testRefreshToken.RefreshToken.UserUuid);
            Assert.IsNotNull(jwt);
            Assert.IsTrue(jwt.Length > 25);
        }

        [TestMethod]
        public void GetJwtClaimsTest()
        {
            string jwt = JwtLogic.GenerateJwtToken(_testRefreshToken.RefreshToken.UserUuid);
            IEnumerable<Claim> claims = JwtLogic.GetJwtClaims(jwt);
            Assert.IsTrue(claims.Any());
        }

        [TestMethod]
        public void GetEmptyJwtClaimsTest()
        {
            IEnumerable<Claim> claims = JwtLogic.GetJwtClaims("");
            Assert.IsFalse(claims.Any());
        }

        [TestMethod]
        public void ValidateJwtToken()
        {
            string jwt = JwtLogic.GenerateJwtToken(_testRefreshToken.RefreshToken.UserUuid);
            bool jwtValid = JwtLogic.ValidateJwtToken(jwt);
            Assert.IsTrue(jwtValid);
        }

        [TestMethod]
        public void ValidateEmptyJwtToken()
        {
            bool jwtValid = JwtLogic.ValidateJwtToken("");
            Assert.IsFalse(jwtValid);
        }
    }
}
