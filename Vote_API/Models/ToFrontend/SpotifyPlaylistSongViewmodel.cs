namespace Vote_API.Models.ToFrontend
{
    public class SpotifyPlaylistSongViewmodel
    {
        public Guid Uuid { get; set; }
        public Guid PlaylistUuid { get; set; }
        public string? SongName { get; set; }
        public string? ArtistName { get; set; }
        public string? SongImageUrl { get; set; }
    }
}
