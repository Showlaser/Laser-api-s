using Vote_API.Models.Dto;

namespace Vote_API.Tests.UnitTests.TestModels
{
    public class TestSpotifyPlaylistSongDto
    {
        public readonly SpotifyPlaylistSongDto SpotifyPlaylistSong = new()
        {
            Uuid = Guid.Parse("e169e772-178e-430b-8809-81089906ca22"),
            ArtistName = "Test Artist",
            SongName = "Test Song",
            SongImageUrl = "https://example.com",
            PlaylistUuid = Guid.Parse("7b98cec9-a30c-4cf3-af48-f26f8acfbb7f")
        };
    }
}
