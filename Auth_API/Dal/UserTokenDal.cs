using Auth_API.Interfaces.Dal;
using Auth_API.Models.Dto.Tokens;
using Microsoft.EntityFrameworkCore;

namespace Auth_API.Dal
{
    public class UserTokenDal : IUserTokenDal
    {
        private readonly DataContext _context;

        public UserTokenDal(DataContext context)
        {
            _context = context;
        }

        public async Task Add(UserTokensDto userTokens)
        {
            await _context.UserToken.AddAsync(userTokens);
            await _context.SaveChangesAsync();
        }

        public async Task<UserTokensDto?> Find(Guid userUuid)
        {
            return await _context.UserToken.SingleOrDefaultAsync(s => s.UserUuid == userUuid);
        }

        public async Task<UserTokensDto?> Find(string refreshToken)
        {
            return await _context.UserToken.SingleOrDefaultAsync(s => s.RefreshToken == refreshToken);
        }

        public async Task Remove(Guid userUuid)
        {
            List<UserTokensDto> spotifyTokens = await _context.UserToken.Where(s => s.UserUuid == userUuid).ToListAsync();
            if (!spotifyTokens.Any())
            {
                return;
            }

            _context.UserToken.RemoveRange(spotifyTokens);
            await _context.SaveChangesAsync();
        }
    }
}
