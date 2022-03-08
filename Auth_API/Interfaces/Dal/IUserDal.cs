using Auth_API.Models.Dto.User;

namespace Auth_API.Interfaces.Dal
{
    public interface IUserDal
    {
        /// <summary>
        /// Adds the user to the database
        /// </summary>
        /// <param name="user">The user to add</param>
        Task Add(UserDto user);

        /// <summary>
        /// Finds the user in the database by uuid
        /// </summary>
        /// <param name="userUuid">The uuid of the user</param>
        /// <returns>The found user, null if nothing is found</returns>
        Task<UserDto?> Find(Guid userUuid);

        /// <summary>
        /// Finds the user in the database by username
        /// </summary>
        /// <param name="username">The username of the user</param>
        /// <returns>The found user, null if nothing is found</returns>
        Task<UserDto?> Find(string username);

        /// <summary>
        /// Updates the user
        /// </summary>
        /// <param name="user">The updated user model</param>
        Task Update(UserDto user);

        /// <summary>
        /// Removes the user from the database
        /// </summary>
        /// <param name="userUuid">The uuid of the user to remove</param>
        Task Remove(Guid userUuid);
    }
}
