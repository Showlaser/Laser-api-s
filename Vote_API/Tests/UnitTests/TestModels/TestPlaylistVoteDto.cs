using Vote_API.Models.Dto;

namespace Vote_API.Tests.UnitTests.TestModels
{
    public class TestPlaylistVoteDto
    {
        public readonly PlaylistVoteDto PlaylistVote = new()
        {
            Uuid = Guid.Parse("f4243881-d651-4fe0-8b7c-d603ccec177a"),
            PlaylistUuid = Guid.Parse("e169e772-178e-430b-8809-81089906ca22"),
        };
    }
}
