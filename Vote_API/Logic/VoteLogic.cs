using System.Security;
using Vote_API.Interfaces.Dal;
using Vote_API.Models.Dto;
using Vote_API.Models.FromFrontend;
using Vote_API.Models.Helper;
using Vote_API.Models.ToFrontend;

namespace Vote_API.Logic
{
    public class VoteLogic
    {
        private readonly IVoteDal _voteDal;
        private readonly IPlaylistVoteDal _playListVoteDal;
        private readonly WebsocketVoteEventSubscriber _websocketVoteEventSubscriber;

        public VoteLogic(IVoteDal voteDal, IPlaylistVoteDal playListVoteDal, WebsocketVoteEventSubscriber websocketVoteEventSubscriber)
        {
            _voteDal = voteDal;
            _playListVoteDal = playListVoteDal;
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

        public async Task<VoteJoinDataViewmodel> Add(VoteDataDto data)
        {
            ValidateVoteData(data);

            string accessCode = SecurityLogic.GenerateRandomString(4);
            data.Salt = SecurityLogic.GetSalt();
            data.Password = SecurityLogic.HashPassword(accessCode, data.Salt);

            data.JoinCode = SecurityLogic.GenerateRandomString(6);
            await _voteDal.Add(data);

            return new()
            {
                AccessCode = accessCode,
                JoinCode = data.JoinCode
            };
        }

        public async Task<VoteDataDto?> Find(VoteJoinData joinData)
        {
            VoteDataDto data = await _voteDal.Find(joinData.JoinCode);
            if (data == null)
            {
                throw new KeyNotFoundException();
            }

            SecurityLogic.ValidatePassword(data.Password, data.Salt, joinData.AccessCode);
            return data;
        }

        public async Task VoteOnPlaylist(PlaylistVoteDto vote, string password)
        {
            VoteDataDto? data = await _voteDal.Find(vote.VoteDataUuid);
            if (data == null)
            {
                throw new KeyNotFoundException();
            }
            SecurityLogic.ValidatePassword(data.Password, data.Salt, password);

            await _playListVoteDal.Add(vote);
            await _websocketVoteEventSubscriber.OnUpdate(data);
        }

        public async Task Update(VoteDataDto data, Guid userUuid)
        {
            ValidateVoteData(data);
            VoteDataDto? voteDataDto = await _voteDal.Find(data.JoinCode);
            if (voteDataDto == null || voteDataDto.AuthorUserUuid != userUuid)
            {
                throw new SecurityException();
            }

            await _voteDal.Update(data);
        }

        public async Task Remove(Guid uuid)
        {
            await _voteDal.Remove(uuid);
        }
    }
}
