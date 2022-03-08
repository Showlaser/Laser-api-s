using Moq;
using Vote_API.Interfaces.Dal;
using Vote_API.Models.Dto;
using Vote_API.Tests.UnitTests.TestModels;

namespace Vote_API.Tests.UnitTests.LogicTest.MockedDals
{
    public class MockedVoteDal
    {
        public readonly IVoteDal VoteDal;

        public MockedVoteDal()
        {
            Mock<IVoteDal> voteDal = new();
            TestVoteDataDto voteData = new();
            VoteDataDto data = voteData.VoteData;

            voteDal.Setup(s => s.Find(data.AuthorUserUuid)).ReturnsAsync(data);
            VoteDal = voteDal.Object;
        }
    }
}
