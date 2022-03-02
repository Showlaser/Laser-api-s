using Isopoh.Cryptography.Argon2;
using Isopoh.Cryptography.SecureArray;
using System.Data;
using System.Text;

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

        public static string HashPassword(string password, byte[] salt)
        {
            Argon2Config config = GetArgon2Config(password, salt);
            Argon2 argon2 = new(config);
            using SecureArray<byte> hashA = argon2.Hash();
            return config.EncodeString(hashA.Buffer);
        }

        public static bool ValidatePassword(string hash, byte[] salt, string password)
        {
            Argon2Config config = GetArgon2Config(password, salt);
            return Argon2.Verify(hash, config);
        }
    }
}
