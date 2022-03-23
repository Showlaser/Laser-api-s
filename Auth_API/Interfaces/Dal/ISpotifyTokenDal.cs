using Auth_API.Models.Dto.Tokens;

namespace Auth_API.Interfaces.Dal
{
    public interface ISpotifyTokenDal
    {
        /// <summary>
        /// Adds the spotify tokens data to the database
        /// </summary>
        /// <param name="spotifyTokens">The spotifyTokens to add</param>
        Task Add(SpotifyTokensDto spotifyTokens);

        /// <summary>
        /// Finds the token data by user uuid
        /// </summary>
        /// <param name="userUuid">The uuid of the user</param>
        /// <returns>The found spotify tokens which contains the user uuid null if nothing is found</returns>
        Task<SpotifyTokensDto?> Find(Guid userUuid);

        /// <summary>
        /// Removes the spotify tokens
        /// </summary>
        /// <param name="userUuid">The user uuid</param>
        Task Remove(Guid userUuid);
    }
}
