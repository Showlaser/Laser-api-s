using Isopoh.Cryptography.Argon2;
using Isopoh.Cryptography.SecureArray;
using System.Data;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace Vote_API.Logic
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

        private static Argon2Config GetArgon2Config(string password, byte[] salt)
        {
            if (string.IsNullOrEmpty(password) || salt.Length < 64)
            {
                throw new NoNullAllowedException();
            }

            string argon2Secret = Environment.GetEnvironmentVariable("ARGON2SECRET") ?? throw new NoNullAllowedException("Environment variable" +
                " ARGON2SECRET was empty. Set it using the ARGON2SECRET environment variable");

            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] secret = Encoding.UTF8.GetBytes(argon2Secret);

            Argon2Config config = Config;
            config.Password = passwordBytes;
            config.Salt = salt;
            config.Secret = secret;
            return config;
        }

        public static string GenerateRandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz123456789";
            Random random = new();
            char[] codeBytes = new char[length];
            for (int i = 0; i < codeBytes.Length; i++)
            {
                codeBytes[i] = chars[random.Next(chars.Length)];
            }

            return new string(codeBytes);
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
