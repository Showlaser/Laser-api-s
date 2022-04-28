using Auth_API.Interfaces.Dal;
using Auth_API.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace Auth_API.Dal
{
    public class DisabledUserDal : IDisabledUserDal
    {
        private readonly DataContext _context;

        public DisabledUserDal(DataContext context)
        {
            _context = context;
        }

        public async Task Add(DisabledUserDto disabledUser)
        {
            await _context.DisabledUser.AddAsync(disabledUser);
            await _context.SaveChangesAsync();
        }

        public async Task<DisabledUserDto?> Find(Guid userUuid)
        {
            return await _context.DisabledUser.FirstOrDefaultAsync(du => du.UserUuid == userUuid);
        }

        public async Task Remove(Guid userUuid)
        {
            DisabledUserDto? disabledUser = await _context.DisabledUser.FirstOrDefaultAsync(du => du.UserUuid == userUuid) ?? throw new KeyNotFoundException();
            _context.DisabledUser.Remove(disabledUser);
            await _context.SaveChangesAsync();
        }
    }
}
