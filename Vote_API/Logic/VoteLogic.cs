using Mapster;
using Vote_API.Interfaces.Dal;
using Vote_API.Models.Dto;
using Vote_API.Models.Helper;
using Vote_API.Models.ToFrontend;

namespace Vote_API.Logic
{
    public class VoteLogic
    {
        private readonly IVoteDal _voteDal;
        private readonly WebsocketVoteEventSubscriber _websocketVoteEventSubscriber;

        public VoteLogic(IVoteDal voteDal, WebsocketVoteEventSubscriber websocketVoteEventSubscriber)
        {
            _voteDal = voteDal;
            _websocketVoteEventSubscriber = websocketVoteEventSubscriber;
        }

        private static void ValidateVoteData(VoteDataDto data)
        {
            bool valid = (data.VoteablePlaylistCollection ?? throw new InvalidOperationException()).Any() &&
                   data.VoteablePlaylistCollection.TrueForAll(vp => (vp.SongsInPlaylist ?? throw new InvalidOperationException())
                       .Any());
            if (!valid)
            {
                throw new InvalidDataException();
            }
        }

        public async Task Add(VoteDataDto data)
        {
            ValidateVoteData(data);
            await _voteDal.Add(data);
        }

        public async Task<VoteDataDto?> Find(Guid userUuid)
        {
            return await _voteDal.Find(userUuid);
        }

        public async Task Update(VoteDataDto data)
        {
            ValidateVoteData(data);
            _websocketVoteEventSubscriber.OnUpdate(data);
            await _voteDal.Update(data);
        }

        public async Task Remove(Guid uuid)
        {
            await _voteDal.Remove(uuid);
        }
    }
}
