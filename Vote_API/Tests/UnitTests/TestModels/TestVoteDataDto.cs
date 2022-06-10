using Vote_API.Logic;
using Vote_API.Models.Dto;

namespace Vote_API.Tests.UnitTests.TestModels
{
    public class TestVoteDataDto
    {
        public readonly VoteDataDto VoteData = new()
        {
            Uuid = Guid.Parse("922eaf07-238b-40d6-bc43-589cc0682a69"),
            JoinCode = "f32sd",
            AuthorUserUuid = Guid.Parse("e2f1986b-1d88-40c1-94b7-b5624d1105f6"),
            ValidUntil = DateTime.Now.AddMinutes(5),
            VoteablePlaylistCollection = new List<VoteablePlaylistDto>
            {
                new TestVoteablePlaylistDto().VoteablePlaylist
            },
        };

        public TestVoteDataDto()
        {
            byte[] salt = SecurityLogic.GetSalt();
            string hash = SecurityLogic.HashPassword("qwerty", salt);
            VoteData.Password = hash;
            VoteData.Salt = salt;
        }
    }
}
