namespace Auth_API.CustomExceptions
{
    public class SpotifyUnavailableException : Exception
    {
        public SpotifyUnavailableException() { }
        public SpotifyUnavailableException(string message) : base(message) { }
        public SpotifyUnavailableException(string message, Exception inner) : base(message, inner) { }
    }
}
