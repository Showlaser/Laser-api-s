using Vote_API.Models.FromFrontend;

namespace Vote_API.Tests.IntegrationTests.TestModels
{
    public class TestVoteData
    {
        public readonly VoteData VoteData = new()
        {
            AuthorUserUuid = Guid.Parse("de900a7d-c0be-4b7a-bcc9-d9bd9cc6ce12"),
            Uuid = Guid.Parse("dd6ef019-0cd4-4648-a1e8-83159898dfbd"),
            ValidUntil = DateTime.Now.AddMinutes(5),
            VoteablePlaylistCollection = new List<VoteablePlaylist>
            {
                new()
                {
                    Uuid = Guid.Parse("30d12ff1-6451-4113-b164-12c549215ccd"),
                    VoteDataUuid = Guid.Parse("dd6ef019-0cd4-4648-a1e8-83159898dfbd"),
                    PlaylistName = "TestPlaylist",
                    SpotifyPlaylistId = "wef343gt34",
                    PlaylistImageUrl = "",
                    SongsInPlaylist = new List<SpotifyPlaylistSong>
                    {
                        new()
                        {
                            Uuid = Guid.Parse("f48113c1-bd42-40d4-82bd-69e97eabdbcd"),
                            ArtistName = "TestArtist",
                            PlaylistUuid = Guid.Parse("30d12ff1-6451-4113-b164-12c549215ccd"),
                            SongImageUrl = "",
                            SongName = "TestSong"
                        }
                    }

                }
            }
        };
    }
}
