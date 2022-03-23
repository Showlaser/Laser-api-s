using Vote_API.Models.FromFrontend;

namespace Vote_API.Models.ToFrontend
{
    public class VoteablePlaylistViewmodel
    {
        public Guid Uuid { get; set; }
        public Guid VoteDataUuid { get; set; }
        public string? PlaylistName { get; set; }
        public string? PlaylistImageUrl { get; set; }
        public List<SpotifyPlaylistSong>? SongsInPlaylist { get; set; }
        public List<PlaylistVoteViewmodel> Votes { get; set; }
    }
}
