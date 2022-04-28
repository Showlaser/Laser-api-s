using Auth_API.Models.Dto;

namespace Auth_API.Interfaces.Dal
{
    public interface IUserActivationDal
    {
        /// <summary>
        /// Adds the activation reset object in the database
        /// </summary>
        /// <param name="userActivation">The activation object to add</param>
        Task Add(UserActivationDto userActivation);

        /// <summary>
        /// Finds the activation object by code
        /// </summary>
        /// <param name="code">The code to search for</param>
        /// <returns>The found activation object, null if nothing is returned</returns>
        Task<UserActivationDto?> Find(Guid code);

        /// <summary>
        /// Deletes the activation object by uuid
        /// </summary>
        /// <param name="userUuid">The uuid of the user</param>
        Task Remove(Guid userUuid);
    }
}
