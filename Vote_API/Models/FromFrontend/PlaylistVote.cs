namespace Vote_API.Models.FromFrontend
{
    public class PlaylistVote
    {
        public Guid VoteDataUuid { get; set; }
        public Guid SpotifyPlaylistUuid { get; set; }
        public VoteJoinData JoinData { get; set; }
    }
}
