using Auth_API.Models.Dto.User;

namespace Auth_API.Interfaces.Dal
{
    public interface IRefreshTokenDal
    {
        /// <summary>
        /// Adds the refreshtoken to the database
        /// </summary>
        /// <param name="refreshTokenDto">The refreshtoken model to add</param>
        Task Add(RefreshTokenDto refreshTokenDto);

        /// <summary>
        /// Finds the refreshtoken model which contains the useruuid
        /// </summary>
        /// <param name="userUuid">The useruuid to search for</param>
        /// <returns>The found refreshtoken model, null if nothing is found</returns>
        Task<RefreshTokenDto> Find(Guid userUuid);

        /// <summary>
        /// Removes the refreshtoken model from the database
        /// </summary>
        /// <param name="userUuid">The useruuid to search for</param>
        Task Remove(Guid userUuid);
    }
}
