using Auth_API.Models.Dto.Tokens;
using Isopoh.Cryptography.Argon2;
using Isopoh.Cryptography.SecureArray;
using System.Data;
using System.Net;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Auth_API.Logic
{
    public static class SecurityLogic
    {
        private static readonly Argon2Config Config = new()
        {
            Type = Argon2Type.DataIndependentAddressing,
            Version = Argon2Version.Nineteen,
            TimeCost = 10,
            MemoryCost = 32768,
            Lanes = 5,
            Threads = Environment.ProcessorCount, // higher than "Lanes" doesn't help (or hurt)
            HashLength = 32 // >= 4
        };

        public static string GenerateCodeChallenge(string codeVerifier)
        {
            using SHA256? sha256 = SHA256.Create();
            byte[]? hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(codeVerifier));
            string? b64Hash = Convert.ToBase64String(hash);
            string? code = Regex.Replace(b64Hash, "\\+", "-");
            code = Regex.Replace(code, "\\/", "_");
            code = Regex.Replace(code, "=+$", "");
            return code;
        }

        public static string GenerateNonce()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz123456789";
            Random random = new();
            char[] nonce = new char[128];
            for (int i = 0; i < nonce.Length; i++)
            {
                nonce[i] = chars[random.Next(chars.Length)];
            }

            return new string(nonce);
        }


        private static Argon2Config GetArgon2Config(string password, byte[] salt)
        {
            if (string.IsNullOrEmpty(password) || salt.Length < 64)
            {
                throw new NoNullAllowedException();
            }

            string argon2Secret = Environment.GetEnvironmentVariable("ARGON2SECRET") ?? throw new NoNullAllowedException("Environment variable" +
                "ARGON2SECRET was empty. Set it using the ARGON2SECRET environment variable");

            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] secret = Encoding.UTF8.GetBytes(argon2Secret);

            Argon2Config config = Config;
            config.Password = passwordBytes;
            config.Salt = salt;
            config.Secret = secret;
            return config;
        }

        public static UserTokensDto GenerateRefreshToken(Guid userUuid, IPAddress? clientIp)
        {
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            byte[] randomBytes = new byte[64];
            rng.GetBytes(randomBytes);
            return new UserTokensDto
            {
                Uuid = Guid.NewGuid(),
                UserUuid = userUuid,
                ClientIp = clientIp,
                RefreshToken = Convert.ToBase64String(randomBytes),
                RefreshTokenExpireDate = DateTime.Now.AddDays(7),
            };
        }

        public static byte[] GetSalt()
        {
            byte[] salt = new byte[64];
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);
            return salt;
        }

        public static string HashPassword(string password, byte[] salt)
        {
            Argon2Config config = GetArgon2Config(password, salt);
            Argon2 argon2 = new(config);
            using SecureArray<byte> hashA = argon2.Hash();
            return config.EncodeString(hashA.Buffer);
        }

        public static void ValidatePassword(string hash, byte[] salt, string password)
        {
            Argon2Config config = GetArgon2Config(password, salt);
            bool passwordValid = Argon2.Verify(hash, config);
            if (!passwordValid)
            {
                throw new SecurityException();
            }
        }
    }
}
