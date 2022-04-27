using Auth_API.Interfaces.Dal;
using Auth_API.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace Auth_API.Dal
{
    public class PasswordResetDal : IPasswordResetDal
    {
        private readonly DataContext _context;

        public PasswordResetDal(DataContext context)
        {
            _context = context;
        }

        public async Task Add(PasswordResetDto passwordReset)
        {
            await _context.PasswordReset.AddAsync(passwordReset);
            await _context.SaveChangesAsync();
        }

        public async Task<PasswordResetDto?> Find(Guid code)
        {
            return await _context.PasswordReset.SingleOrDefaultAsync(p => p.Code == code);
        }

        public async Task Remove(Guid uuid)
        {
            PasswordResetDto passwordResetToRemove = await _context.PasswordReset.SingleAsync(p => p.Uuid == uuid);
            if (passwordResetToRemove == null)
            {
                throw new KeyNotFoundException();
            }

            _context.PasswordReset.Remove(passwordResetToRemove);
            await _context.SaveChangesAsync();
        }
    }
}
