using Auth_API.Interfaces.Dal;
using Auth_API.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace Auth_API.Dal
{
    public class UserActivationDal : IUserActivationDal
    {
        private readonly DataContext _context;

        public UserActivationDal(DataContext context)
        {
            _context = context;
        }

        public async Task Add(UserActivationDto userActivation)
        {
            await _context.UserActivation.AddAsync(userActivation);
            await _context.SaveChangesAsync();
        }

        public async Task<UserActivationDto?> Find(Guid code)
        {
            return await _context.UserActivation.SingleOrDefaultAsync(p => p.Code == code);
        }

        public async Task Remove(Guid userUuid)
        {
            UserActivationDto userActivationToRemove = await _context.UserActivation.SingleAsync(p => p.UserUuid == userUuid);
            if (userActivationToRemove == null)
            {
                throw new KeyNotFoundException();
            }

            _context.UserActivation.Remove(userActivationToRemove);
            await _context.SaveChangesAsync();
        }
    }
}
