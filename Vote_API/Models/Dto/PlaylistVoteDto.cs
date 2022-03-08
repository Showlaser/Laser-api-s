namespace Vote_API.Models.Dto
{
    public class PlaylistVoteDto
    {
        public Guid Uuid { get; set; }
        public Guid SpotifyPlaylistSongUuid { get; set; }
    }
}
