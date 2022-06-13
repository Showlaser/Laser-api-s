using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Vote_API.Logic
{
    public static class JwtLogic
    {
        public static List<Claim> GetJwtClaims(string jwt)
        {
            JwtSecurityToken securityToken = ValidateJwt(jwt);
            return securityToken.Claims.ToList();
        }

        public static bool ValidateJwtToken(string jwt)
        {
            JwtSecurityToken securityToken = ValidateJwt(jwt);
            return securityToken.Claims.Any();
        }

        private static JwtSecurityToken ValidateJwt(string jwt)
        {
            Console.WriteLine($"JWT IS: {jwt}");
            Debug.WriteLine($"JWT IS: {jwt}");
            JwtSecurityTokenHandler tokenHandler = new();
            string jwtSecret = Environment.GetEnvironmentVariable("JWTSECRET") ?? throw new NoNullAllowedException("Environment variable" +
                "JWTSECRET was empty. Set it using the JWTSECRET environment variable");
            byte[] jwtSecretKey = Encoding.ASCII.GetBytes(jwtSecret);
            try
            {
                tokenHandler.ValidateToken(jwt, new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(jwtSecretKey),
                    ValidAudience = "auth",
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = true,
                    ValidateIssuer = false,
                    ValidateLifetime = true
                }, out SecurityToken validatedToken);
                return (JwtSecurityToken)validatedToken;
            }
            catch
            {
                return new JwtSecurityToken();
            }
        }
    }
}
