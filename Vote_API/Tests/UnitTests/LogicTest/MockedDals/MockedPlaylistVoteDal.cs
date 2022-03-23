using Moq;
using Vote_API.Interfaces.Dal;

namespace Vote_API.Tests.UnitTests.LogicTest.MockedDals
{
    public class MockedPlaylistVoteDal
    {
        public readonly IPlaylistVoteDal PlaylistVoteDal;

        public MockedPlaylistVoteDal()
        {
            Mock<IPlaylistVoteDal> playlistVoteDal = new();
            PlaylistVoteDal = playlistVoteDal.Object;
        }
    }
}
