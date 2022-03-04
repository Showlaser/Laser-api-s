namespace Auth_API.Tests.UnitTests
{
    public static class TestHelper
    {
        public static void SetEnvironmentVariables()
        {
            Environment.SetEnvironmentVariable("ARGON2SECRET", "hhjwe093892349jsdfwe");
            Environment.SetEnvironmentVariable("JWTSECRET", "fwhefwiufhawgh98g43hg98ahdfjig");

        }
    }
}
