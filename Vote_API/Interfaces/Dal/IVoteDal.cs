using Vote_API.Models.Dto;

namespace Vote_API.Interfaces.Dal
{
    public interface IVoteDal
    {
        /// <summary>
        /// Data to add in the database
        /// </summary>
        /// <param name="data">The data to add</param>
        Task Add(VoteDataDto data);

        /// <summary>
        /// Finds the data by user uuid
        /// </summary>
        /// <param name="userUuid">The uuid of user in the object to find</param>
        /// <returns>The found data</returns>
        Task<VoteDataDto?> Find(Guid userUuid);

        /// <summary>
        /// Updates the data in the database
        /// </summary>
        /// <param name="data">The updated data</param>
        Task Update(VoteDataDto data);

        /// <summary>
        /// Removes the data with the matching uuid from the database, if nothing is found no remove will occur
        /// </summary>
        /// <param name="uuid">The uuid of the data to remove</param>
        Task Remove(Guid uuid);
    }
}
