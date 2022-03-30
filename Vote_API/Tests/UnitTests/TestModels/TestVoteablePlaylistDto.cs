using Vote_API.Models.Dto;

namespace Vote_API.Tests.UnitTests.TestModels
{
    public class TestVoteablePlaylistDto
    {
        public readonly VoteablePlaylistDto VoteablePlaylist = new()
        {
            Uuid = Guid.Parse("7b98cec9-a30c-4cf3-af48-f26f8acfbb7f"),
            VoteDataUuid = Guid.Parse("922eaf07-238b-40d6-bc43-589cc0682a69"),
            PlaylistImageUrl = "https://example.com",
            PlaylistName = "Test playlist",
            SpotifyPlaylistId = "r32r23wefe",
            SongsInPlaylist = new List<SpotifyPlaylistSongDto>
            {
                new TestSpotifyPlaylistSongDto().SpotifyPlaylistSong
            }
        };
    }
}
