using Vote_API.Interfaces.Dal;
using Vote_API.Models.Dto;

namespace Vote_API.Logic
{
    public class VoteLogic
    {
        private readonly IVoteDal _voteDal;

        public VoteLogic(IVoteDal voteDal)
        {
            _voteDal = voteDal;
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
            await _voteDal.Update(data);
        }

        public async Task Remove(Guid uuid)
        {
            await _voteDal.Remove(uuid);
        }
    }
}
