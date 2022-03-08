namespace Vote_API.Models.FromFrontend
{
    public class PlaylistVote
    {
        public Guid Uuid { get; set; }
        public Guid SpotifyPlaylistSongUuid { get; set; }
    }
}
