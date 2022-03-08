using Microsoft.EntityFrameworkCore;
using Vote_API.Interfaces.Dal;
using Vote_API.Models.Dto;

namespace Vote_API.Dal
{
    public class VoteDal : IVoteDal
    {
        private readonly DataContext _context;

        public VoteDal(DataContext dataContext)
        {
            _context = dataContext;
        }

        public async Task Add(VoteDataDto data)
        {
            await _context.VoteData.AddAsync(data);
            await _context.SaveChangesAsync();
        }

        public async Task<VoteDataDto?> Find(Guid userUuid)
        {
            return await _context.VoteData.FirstOrDefaultAsync(e => e.AuthorUserUuid == userUuid);
        }

        public async Task Update(VoteDataDto data)
        {
            VoteDataDto dbData = await _context.VoteData.FindAsync(data.Uuid) ?? throw new KeyNotFoundException();
            dbData.VoteablePlaylistCollection = data.VoteablePlaylistCollection;
            dbData.ValidUntil = data.ValidUntil;

            _context.VoteData.Update(dbData);
            await _context.SaveChangesAsync();
        }

        public async Task Remove(Guid uuid)
        {
            VoteDataDto? dbData = await _context.VoteData.FindAsync(uuid);
            if (dbData == null)
            {
                return;
            }

            _context.VoteData.Remove(dbData);
            await _context.SaveChangesAsync();
        }
    }
}
