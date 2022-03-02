using Auth_API.Models.Dto;

namespace Auth_API.Interfaces.Dal
{
    public interface ISpotifyDal
    {
        /// <summary>
        /// Adds the account data to the database
        /// </summary>
        /// <param name="account">The account to add</param>
        Task Add(SpotifyAccountDataDto account);

        /// <summary>
        /// Finds the spotify account data by user uuid
        /// </summary>
        /// <param name="userUuid">The uuid of the user</param>
        /// <returns>The found spotify account data which contains the useruuid</returns>
        Task<SpotifyAccountDataDto> Find(Guid userUuid);
        
        /// <summary>
        /// Removes the spotify account data by user uuid
        /// </summary>
        /// <param name="userUuid">The uuid of the user</param>
        Task Remove(Guid userUuid);
    }
}
