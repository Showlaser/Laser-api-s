namespace Vote_API.Models.FromFrontend
{
    public class VoteablePlaylist
    {
        public Guid Uuid { get; set; }
        public Guid VoteDataUuid { get; set; }
        public string? SpotifyPlaylistId { get; set; }
        public string? PlaylistName { get; set; }
        public string? PlaylistImageUrl { get; set; }
        public List<SpotifyPlaylistSong>? SongsInPlaylist { get; set; }
        public List<VoteData>? Votes { get; set; }
    }
}
