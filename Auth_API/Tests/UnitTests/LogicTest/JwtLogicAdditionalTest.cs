using Auth_API.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Claims;

namespace Auth_API.Tests.UnitTests.LogicTest
{
    [TestClass]
    public class JwtLogicAdditionalTest
    {
        public JwtLogicAdditionalTest()
        {
            TestHelper.SetupTestEnvironment();
        }

        [TestMethod]
        public void GeneratedJwtContainsUuidClaimTest()
        {
            Guid uuid = Guid.NewGuid();
            string jwt = JwtLogic.GenerateJwtToken(uuid);

            List<Claim> claims = JwtLogic.GetJwtClaims(jwt);
            Claim? uuidClaim = claims.FirstOrDefault(c => c.Type == "uuid");

            Assert.IsNotNull(uuidClaim);
            Assert.AreEqual(uuid.ToString(), uuidClaim.Value);
        }

        [TestMethod]
        public void TamperedJwtIsInvalidTest()
        {
            string jwt = JwtLogic.GenerateJwtToken(Guid.NewGuid());

            // Flip the first character of the header so the signature no longer matches
            string tampered = (jwt[0] == 'x' ? 'y' : 'x') + jwt[1..];

            Assert.IsFalse(JwtLogic.ValidateJwtToken(tampered));
        }

        [TestMethod]
        public void GarbageJwtIsInvalidTest()
        {
            Assert.IsFalse(JwtLogic.ValidateJwtToken("not-a-real-jwt"));
        }
    }
}
