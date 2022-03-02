using Auth_API.Interfaces.Dal;

namespace Auth_API.Logic
{
    public class SpotifyLogic
    {
        private readonly ISpotifyDal _spotifyDal;

        public SpotifyLogic(ISpotifyDal spotifyDal)
        {
            _spotifyDal = spotifyDal;
        }
    }
}
