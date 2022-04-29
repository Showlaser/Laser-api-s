using Auth_API.Enums;

namespace Auth_API.Models.Helper
{
    public static class DisabledUserHelper
    {
        public static string? ConvertEnumToFriendlyMessage(DisabledReason disabledReason)
        {
            return disabledReason switch
            {
                DisabledReason.BruteForceAttempt =>
                    "To many failed login attempts have occurred. Contact the customer service to unblock your account.",
                DisabledReason.EmailNeedsToBeValidated =>
                    "Your email needs to be validated, check your email and click the activation link.",
                DisabledReason.Misuse =>
                    "Misuse has been detected and your account is disabled. Contact the customer service to unblock your account.",
                _ => null
            };
        }
    }
}
