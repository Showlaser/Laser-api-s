namespace Vote_API.Models.FromFrontend
{
    public class SpotifyPlaylistSong
    {
        public Guid Uuid { get; set; }
        public Guid PlaylistUuid { get; set; }
        public string? SongName { get; set; }
        public string? ArtistName { get; set; }
        public string? SongImageUrl { get; set; }
    }
}
