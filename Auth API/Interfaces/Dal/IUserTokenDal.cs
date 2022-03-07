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
        /// <returns>The found spotify userTokens data which contains the useruuid null if nothing is found</returns>
        Task<UserTokensDto?> Find(Guid userUuid);

        /// <summary>
        /// Removes the token data by user uuid
        /// </summary>
        /// <param name="userUuid">The uuid of the user</param>
        Task Remove(Guid userUuid);
    }
}
