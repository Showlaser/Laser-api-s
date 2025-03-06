﻿using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth_API.Logic
{
    public static class JwtLogic
    {
        public static string GenerateJwtToken(Guid userUuid)
        {
            JwtSecurityTokenHandler tokenHandler = new();
            string jwtSecret = Environment.GetEnvironmentVariable("JWTSECRET") ?? throw new NoNullAllowedException("Environment variable" +
                "JWTSECRET was empty. Set it using the JWTSECRET environment variable");

            byte[] jwtSecretKey = Encoding.ASCII.GetBytes(jwtSecret);
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim("uuid", userUuid.ToString()),
                ]),
                Audience = "auth",
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(jwtSecretKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static List<Claim> GetJwtClaims(string jwt)
        {
            JwtSecurityToken securityToken = ValidateJwt(jwt, false);
            return securityToken.Claims.ToList();
        }

        public static bool ValidateJwtToken(string jwt)
        {
            JwtSecurityToken securityToken = ValidateJwt(jwt, true);
            return securityToken.Claims.Any();
        }

        private static JwtSecurityToken ValidateJwt(string jwt, bool validateTime)
        {
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
                    ValidateLifetime = validateTime,
                    ClockSkew = TimeSpan.Zero
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
