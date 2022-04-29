using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vote_API.Logic;
using Vote_API.Models.Dto;
using Vote_API.Models.FromFrontend;
using Vote_API.Models.Helper;
using Vote_API.Models.ToFrontend;

namespace Vote_API.Controllers
{
    [AuthorizedAction]
    [Route("vote")]
    [ApiController]
    public class VoteController : ControllerBase
    {
        private readonly VoteLogic _voteLogic;
        private readonly ControllerResultHelper _controllerResultHelper;

        public VoteController(VoteLogic voteLogic, ControllerResultHelper controllerResultHelper)
        {
            _voteLogic = voteLogic;
            _controllerResultHelper = controllerResultHelper;
        }

        [HttpPost]
        public async Task<ActionResult<VoteJoinDataViewmodel>> Add([FromBody] VoteData data)
        {
            async Task<VoteJoinDataViewmodel> Action()
            {
                VoteDataDto dataDto = data.Adapt<VoteDataDto>();
                UserModel user = ControllerHelper.GetUserModelFromJwtClaims(this);

                dataDto.AuthorUserUuid = user.Uuid;
                return await _voteLogic.Add(dataDto);
            }

            return await _controllerResultHelper.Execute(Action());
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<VoteDataViewmodel?>> Find([FromQuery] string joinCode, [FromQuery] string accessCode)
        {
            async Task<VoteDataViewmodel?> Action()
            {
                VoteDataDto data = await _voteLogic.Find(new VoteJoinData
                {
                    JoinCode = joinCode,
                    AccessCode = accessCode
                });

                return data.Adapt<VoteDataViewmodel>();
            }

            return await _controllerResultHelper.Execute(Action());
        }

        [AllowAnonymous]
        [HttpPost("vote")]
        public async Task<ActionResult> VoteOnPlaylist([FromBody] PlaylistVote vote)
        {
            async Task Action()
            {
                PlaylistVoteDto? voteDto = vote.Adapt<PlaylistVoteDto>();
                voteDto.Uuid = Guid.NewGuid();
                await _voteLogic.VoteOnPlaylist(voteDto, vote.JoinData.AccessCode);
            }

            return await _controllerResultHelper.Execute(Action());
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] VoteData data)
        {
            async Task Action()
            {
                UserModel user = ControllerHelper.GetUserModelFromJwtClaims(this);
                VoteDataDto dataDto = data.Adapt<VoteDataDto>();
                await _voteLogic.Update(dataDto, user.Uuid);
            }

            return await _controllerResultHelper.Execute(Action());
        }

        [HttpDelete]
        public async Task<ActionResult> Remove()
        {
            async Task Action()
            {
                UserModel user = ControllerHelper.GetUserModelFromJwtClaims(this);
                await _voteLogic.Remove(user.Uuid);
            }

            return await _controllerResultHelper.Execute(Action());
        }
    }
}
