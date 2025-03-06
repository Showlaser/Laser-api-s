namespace Vote_API.Tests.UnitTests
{
    public static class TestHelper
    {
        public static void SetupTestEnvironment()
        {
            //System.Diagnostics.Debugger.Launch();
            SetEnvironmentVariables();
        }

        private static void SetEnvironmentVariables()
        {
            Environment.SetEnvironmentVariable("ARGON2SECRET", "fho938fh32oewfjfefj2398");
            Environment.SetEnvironmentVariable("JWTSECRET", "hwef9hewihsidhfkfdafew");
            Environment.SetEnvironmentVariable("SERVER", "localhost");
            Environment.SetEnvironmentVariable("DATABASE", "vote");
            Environment.SetEnvironmentVariable("PORT", "3306");
            Environment.SetEnvironmentVariable("USERNAME", "root");
            Environment.SetEnvironmentVariable("PASSWORD", "qwerty");
            Environment.SetEnvironmentVariable("FRONTENDURL", "http://localhost:3000/");
        }
    }
}
