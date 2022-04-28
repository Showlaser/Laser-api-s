namespace Auth_API.CustomExceptions
{
    public class UserDisabledException : Exception
    {
        public UserDisabledException() { }
        public UserDisabledException(string message) : base(message) { }
        public UserDisabledException(string message, Exception inner) : base(message, inner) { }
    }
}
