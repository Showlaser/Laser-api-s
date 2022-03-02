using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Auth_API.Controllers
{
    [Route("spotify")]
    [ApiController]
    public class SpotifyController : ControllerBase
    {
        public SpotifyController()
        {
            
        }

        [HttpPost]
        public async Task<string> RefreshAccessToken()
        {

        }
    }
}
