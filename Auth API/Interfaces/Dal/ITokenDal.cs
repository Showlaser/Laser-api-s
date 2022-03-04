using Auth_API.Models.Dto.User;

namespace Auth_API.Interfaces.Dal
{
    public interface ITokenDal
    {
        /// <summary>
        /// Adds the account data to the database
        /// </summary>
        /// <param name="account">The account to add</param>
        Task Add(UserTokensDto account);

        /// <summary>
        /// Finds the token data by user uuid
        /// </summary>
        /// <param name="userUuid">The uuid of the user</param>
        /// <returns>The found spotify account data which contains the useruuid null if nothing is found</returns>
        Task<UserTokensDto?> Find(Guid userUuid);

        /// <summary>
        /// Removes the token data by user uuid
        /// </summary>
        /// <param name="userUuid">The uuid of the user</param>
        Task Remove(Guid userUuid);
    }
}
