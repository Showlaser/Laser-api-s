using Vote_API.Models.Dto;

namespace Vote_API.Interfaces.Dal
{
    public interface IPlaylistVoteDal
    {
        /// <summary>
        /// Add the vote to the database
        /// </summary>
        /// <param name="vote">The vote to add</param>
        Task Add(PlaylistVoteDto vote);
    }
}
