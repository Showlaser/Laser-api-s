using Vote_API.Interfaces.Dal;
using Vote_API.Logic;
using Vote_API.Models.Helper;
using Vote_API.Tests.UnitTests.LogicTest.MockedDals;

namespace Vote_API.Tests.UnitTests.LogicTest.MockedLogics
{
    public class MockedVoteLogic
    {
        public readonly VoteLogic VoteLogic;

        public MockedVoteLogic()
        {
            IVoteDal voteDal = new MockedVoteDal().VoteDal;
            IPlaylistVoteDal playlistVoteDal = new MockedPlaylistVoteDal().PlaylistVoteDal;
            VoteLogic = new VoteLogic(voteDal, playlistVoteDal, new WebsocketVoteEventSubscriber());
        }
    }
}
