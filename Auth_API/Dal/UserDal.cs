using Auth_API.Interfaces.Dal;
using Auth_API.Models.Dto.User;
using Microsoft.EntityFrameworkCore;

namespace Auth_API.Dal
{
    public class UserDal : IUserDal
    {
        private readonly DataContext _context;

        public UserDal(DataContext context)
        {
            _context = context;
        }

        public async Task Add(UserDto user)
        {
            _context.User.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<UserDto?> Find(Guid userUuid)
        {
            return await _context.User.SingleOrDefaultAsync(u => u.Uuid == userUuid);
        }

        public async Task<UserDto?> Find(string username)
        {
            return await _context.User.SingleOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<UserDto?> FindByEmail(string email)
        {
            return await _context.User.SingleAsync(u => u.Email == email);
        }

        public async Task Update(UserDto user)
        {
            UserDto userToUpdate = await _context.User.FindAsync(user.Uuid) ?? throw new KeyNotFoundException();
            userToUpdate.UserName = user.UserName;
            userToUpdate.Password = user.Password;
            userToUpdate.Salt = user.Salt;

            _context.User.Update(userToUpdate);
            await _context.SaveChangesAsync();
        }

        public async Task Remove(Guid userUuid)
        {
            UserDto userToRemove = await _context.User.FindAsync(userUuid) ?? throw new KeyNotFoundException();
            _context.User.Remove(userToRemove);
            await _context.SaveChangesAsync();
        }
    }
}
