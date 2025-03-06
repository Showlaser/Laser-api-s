namespace Auth_API.Tests.UnitTests
{
    public static class TestHelper
    {
        public static void SetupTestEnvironment()
        {
            SetEnvironmentVariables();
        }

        private static void SetEnvironmentVariables()
        {
            Environment.SetEnvironmentVariable("ARGON2SECRET", "jwoi320djkg40-8u92huyashdfkj");
            Environment.SetEnvironmentVariable("JWTSECRET", "fwhefwiufhawgh98g43hg98ahdf32vfaw4ta3vrejig");
            Environment.SetEnvironmentVariable("SERVER", "localhost");
            Environment.SetEnvironmentVariable("DATABASE", "auth");
            Environment.SetEnvironmentVariable("PORT", "3306");
            Environment.SetEnvironmentVariable("USERNAME", "root");
            Environment.SetEnvironmentVariable("PASSWORD", "qwerty");
            Environment.SetEnvironmentVariable("SPOTIFYCLIENTID", "123");
            Environment.SetEnvironmentVariable("SPOTIFYCLIENTSECRET", "123");
            Environment.SetEnvironmentVariable("FRONTENDURL", "localhost/");
        }
    }
}
