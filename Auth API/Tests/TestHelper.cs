namespace Auth_API.Tests
{
    public static class TestHelper
    {
        public static void SetEnvironmentVariables()
        {
            Environment.SetEnvironmentVariable("ARGON2SECRET", "hhjwe093892349jsdfwe");
            Environment.SetEnvironmentVariable("JWTSECRET", "fwhefwiufhawgh98g43hg98ahdfjig");
            Environment.SetEnvironmentVariable("SERVER", "localhost");
            Environment.SetEnvironmentVariable("DATABASE", "auth");
            Environment.SetEnvironmentVariable("PORT", "3306");
            Environment.SetEnvironmentVariable("USERNAME", "root");
            Environment.SetEnvironmentVariable("PASSWORD", "qwerty");
            Environment.SetEnvironmentVariable("SPOTIFYCLIENTID", "123");
            Environment.SetEnvironmentVariable("SPOTIFYCLIENTSECRET", "123");
        }
    }
}
