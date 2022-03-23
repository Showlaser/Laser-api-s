using Vote_API.Interfaces.Dal;
using Vote_API.Models.Dto;

namespace Vote_API.Dal
{
    public class PlaylistVoteDal : IPlaylistVoteDal
    {
        private readonly DataContext _context;

        public PlaylistVoteDal(DataContext context)
        {
            _context = context;
        }

        public async Task Add(PlaylistVoteDto vote)
        {
            await _context.PlaylistVote.AddAsync(vote);
            await _context.SaveChangesAsync();
        }
    }
}
