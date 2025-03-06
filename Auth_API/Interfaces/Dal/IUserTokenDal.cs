using Auth_API.Models.Dto.Tokens;

namespace Auth_API.Interfaces.Dal
{
    public interface IUserTokenDal
    {
        /// <summary>
        /// Adds the userTokens data to the database
        /// </summary>
        /// <param name="userTokens">The userTokens to add</param>
        Task Add(UserTokensDto userTokens);

        /// <summary>
        /// Finds the token data by user uuid
        /// </summary>
        /// <param name="userUuid">The uuid of the user</param>
        /// <returns>The found user tokens data, null if nothing is found</returns>
        Task<UserTokensDto?> Find(Guid userUuid);

        /// <summary>
        /// Finds the token data by refresh token
        /// </summary>
        /// <param name="refreshToken">The refresh token to search for</param>
        /// <returns>The found user tokens data, null if nothing is found</returns>
        Task<UserTokensDto?> Find(string refreshToken);

        /// <summary>
        /// Removes the token data by user uuid
        /// </summary>
        /// <param name="userUuid">The uuid of the user</param>
        Task Remove(Guid userUuid);
    }
}
