using Auth_API.Interfaces.Dal;
using Auth_API.Models.Dto.Tokens;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace Auth_API.Dal
{
    public class SpotifyTokenDal : ISpotifyTokenDal
    {
        private readonly DataContext _context;

        public SpotifyTokenDal(DataContext context)
        {
            _context = context;
        }

        public async Task Add(SpotifyTokensDto spotifyTokens)
        {
            await _context.SpotifyToken.AddAsync(spotifyTokens);
            await _context.SaveChangesAsync();
        }

        public async Task<SpotifyTokensDto?> Find(Guid userUuid)
        {
            return await _context.SpotifyToken.FirstOrDefaultAsync(s => s.UserUuid == userUuid);
        }

        public async Task Remove(Guid userUuid)
        {
            SpotifyTokensDto? tokens = await _context.SpotifyToken.FirstOrDefaultAsync(s => s.UserUuid == userUuid);
            if (tokens == null)
            {
                return;
            }

            string query = "DELETE FROM SpotifyToken WHERE UserUuid = @uuid";
            await _context.Database.ExecuteSqlRawAsync(query, new MySqlParameter("@uuid", userUuid));
        }
    }
}
