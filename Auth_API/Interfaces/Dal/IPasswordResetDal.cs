using Auth_API.Models.Dto;

namespace Auth_API.Interfaces.Dal
{
    public interface IPasswordResetDal
    {
        /// <summary>
        /// Adds the password reset object in the database
        /// </summary>
        /// <param name="passwordReset">The password reset object to add</param>
        Task Add(PasswordResetDto passwordReset);

        /// <summary>
        /// Finds the password reset object by code
        /// </summary>
        /// <param name="code">The code to search for</param>
        /// <returns>The found password reset object, null if nothing is returned</returns>
        Task<PasswordResetDto?> Find(Guid code);

        /// <summary>
        /// Deletes the password reset object by uuid
        /// </summary>
        /// <param name="uuid">The uuid of the password reset object to remove</param>
        Task Remove(Guid uuid);
    }
}
