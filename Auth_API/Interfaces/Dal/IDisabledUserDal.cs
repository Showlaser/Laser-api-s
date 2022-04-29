using Auth_API.Models.Dto;

namespace Auth_API.Interfaces.Dal
{
    public interface IDisabledUserDal
    {
        /// <summary>
        /// Adds the disabled user to the database
        /// </summary>
        /// <param name="disabledUser">The disabled user to add</param>
        public Task Add(DisabledUserDto disabledUser);

        /// <summary>
        /// Finds the disabled user by user uuid
        /// </summary>
        /// <param name="userUuid">The uuid of the user to search for</param>
        /// <returns>The found disabled user, null if nothing is found</returns>
        public Task<DisabledUserDto?> Find(Guid userUuid);

        /// <summary>
        /// Removes the disabled user by uuid
        /// </summary>
        /// <param name="userUuid">The uuid of the disabled user</param>
        public Task Remove(Guid userUuid);
    }
}
