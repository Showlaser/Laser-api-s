namespace Vote_API.Models.Dto
{
    public class VoteablePlaylistDto
    {
        public Guid Uuid { get; set; }
        public Guid VoteDataUuid { get; set; }
        public string? PlaylistName { get; set; }
        public string? PlaylistImageUrl { get; set; }
        public List<SpotifyPlaylistSongDto>? SongsInPlaylist { get; set; }
    }
}
