using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Vote_API.Logic;
using Vote_API.Models.Dto;
using Vote_API.Models.FromFrontend;
using Vote_API.Tests.UnitTests.LogicTest.MockedLogics;
using Vote_API.Tests.UnitTests.TestModels;

namespace Vote_API.Tests.UnitTests.LogicTest
{
    [TestClass]
    public class VoteLogicTest
    {
        private readonly VoteLogic _voteLogic;
        private readonly VoteDataDto _voteData;

        public VoteLogicTest()
        {
            _voteLogic = new MockedVoteLogic().VoteLogic;
            _voteData = new TestVoteDataDto().VoteData;
        }

        [TestMethod]
        public async Task AddTest()
        {
            await _voteLogic.Add(_voteData);
        }

        [TestMethod]
        public async Task FindTest()
        {
            VoteDataDto? data = await _voteLogic.Find(new VoteJoinData
            {
                JoinCode = _voteData.JoinCode
            });
            Assert.IsNotNull(data);
        }

        [TestMethod]
        public async Task UpdateTest()
        {
            await _voteLogic.Update(_voteData, _voteData.AuthorUserUuid);
        }

        [TestMethod]
        public async Task RemoveTests()
        {
            await _voteLogic.Remove(_voteData.Uuid);
        }
    }
}
