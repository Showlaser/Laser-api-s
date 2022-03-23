namespace Vote_API.Models.FromFrontend
{
    public class WebsocketIdentifier
    {
        public Guid VoteDataUuid { get; set; }
        public Guid WebsocketUuid { get; set; }
        public string JoinCode { get; set; }
        public string AccessCode { get; set; }
    }
}
