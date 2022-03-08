using Mapster;
using Microsoft.AspNetCore.Mvc;
using Vote_API.Logic;
using Vote_API.Models.Dto;
using Vote_API.Models.FromFrontend;
using Vote_API.Models.Helper;
using Vote_API.Models.ToFrontend;

namespace Vote_API.Controllers
{

    [Route("vote")]
    [ApiController]
    public class VoteController : ControllerBase
    {
        private readonly VoteLogic _voteLogic;

        public VoteController(VoteLogic voteLogic)
        {
            _voteLogic = voteLogic;
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] VoteData data)
        {
            async Task Action()
            {
                VoteDataDto dataDto = data.Adapt<VoteDataDto>();
                await _voteLogic.Add(dataDto);
            }

            ControllerErrorHandler controllerErrorHandler = new();
            await controllerErrorHandler.Execute(Action());
            return StatusCode(controllerErrorHandler.StatusCode);
        }

        [HttpGet]
        public async Task<ActionResult<VoteDataViewmodel?>> Find()
        {
            async Task<VoteDataDto?> Action()
            {
                UserModel user = ControllerHelper.GetUserModelFromJwtClaims(this);
                return await _voteLogic.Find(user.Uuid);
            }

            ControllerErrorHandler controllerErrorHandler = new();
            VoteDataDto? data = await controllerErrorHandler.Execute(Action());
            return data?.Adapt<VoteDataViewmodel?>();
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] VoteData data)
        {
            async Task Action()
            {
                VoteDataDto dataDto = data.Adapt<VoteDataDto>();
                await _voteLogic.Update(dataDto);
            }

            ControllerErrorHandler controllerErrorHandler = new();
            await controllerErrorHandler.Execute(Action());
            return StatusCode(controllerErrorHandler.StatusCode);
        }

        [HttpDelete]
        public async Task<ActionResult> Remove()
        {
            async Task Action()
            {
                UserModel user = ControllerHelper.GetUserModelFromJwtClaims(this);
                await _voteLogic.Remove(user.Uuid);
            }

            ControllerErrorHandler controllerErrorHandler = new();
            await controllerErrorHandler.Execute(Action());
            return StatusCode(controllerErrorHandler.StatusCode);
        }
    }
}
