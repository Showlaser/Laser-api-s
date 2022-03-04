using Auth_API.Interfaces.Dal;
using Auth_API.Models.Dto.User;
using Microsoft.EntityFrameworkCore;

namespace Auth_API.Dal
{
    public class TokenDal : ITokenDal
    {
        private readonly DataContext _context;

        public TokenDal(DataContext context)
        {
            _context = context;
        }

        public async Task Add(UserTokensDto account)
        {
            await _context.RefreshToken.AddAsync(account);
            await _context.SaveChangesAsync();
        }

        public async Task<UserTokensDto?> Find(Guid userUuid)
        {
            return await _context.RefreshToken.SingleOrDefaultAsync(s => s.UserUuid == userUuid);
        }

        public async Task Remove(Guid userUuid)
        {
            UserTokensDto spotifyDb = await _context.RefreshToken.SingleOrDefaultAsync(s => s.UserUuid == userUuid);
            if (spotifyDb == null)
            {
                return;
            }
            _context.RefreshToken.Remove(spotifyDb);
            await _context.SaveChangesAsync();
        }
    }
}
