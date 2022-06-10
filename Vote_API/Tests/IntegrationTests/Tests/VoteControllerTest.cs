using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vote_API.Models.FromFrontend;
using Vote_API.Models.ToFrontend;
using Vote_API.Tests.IntegrationTests.TestModels;

namespace Vote_API.Tests.IntegrationTests.Tests
{
    [TestClass]
    public class VoteControllerTest
    {
        [TestInitialize]
        public void Setup()
        {
            TestHelper.SetupTestEnvironment();
        }

        [TestMethod]
        public void FindVoteTest()
        {
            VoteDataViewmodel? voteData = TestHelper.Client.GetFromJsonAsync<VoteDataViewmodel>($"vote?joinCode={TestHelper.VoteJoinData.JoinCode}&accessCode={TestHelper.VoteJoinData.AccessCode}").Result;
            Assert.IsNotNull(voteData);
        }

        [TestMethod]
        public void VoteOnPlaylistTest()
        {
            TestVoteData voteData = new();
            PlaylistVote playlistVote = new()
            {
                JoinData = TestHelper.VoteJoinData,
                SpotifyPlaylistUuid = voteData.VoteData.VoteablePlaylistCollection.First().Uuid,
                VoteDataUuid = voteData.VoteData.Uuid
            };

            HttpResponseMessage response = TestHelper.Client.PostAsJsonAsync("vote/vote", playlistVote).Result;
            response.EnsureSuccessStatusCode();
        }
    }
}
