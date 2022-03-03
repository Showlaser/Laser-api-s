using Auth_API.Interfaces.Dal;
using Auth_API.Models.Dto.User;
using System.Data;
using System.Security;

namespace Auth_API.Logic
{
    public class UserLogic
    {
        private readonly IUserDal _userDal;
        private readonly ITokenDal _tokenDal;

        public UserLogic(IUserDal userDal, ITokenDal tokenDal)
        {
            _userDal = userDal;
            _tokenDal = tokenDal;
        }

        /// <summary>
        /// Checks if the username is in use by another user in the database.
        /// This method throws an DuplicateNameException exception if an user with the same username and with a different uuid is found
        /// </summary>
        /// <param name="user">The user from the front-end</param>
        /// <param name="dbUser">The user found in the database. If null the user will be searched in the database</param>
        /// <exception cref="DuplicateNameException"></exception>
        private async Task UsernameExists(UserDto user, UserDto? dbUser = null)
        {
            dbUser ??= await _userDal.Find(user.UserName);
            if (dbUser != null &&
                dbUser.UserName == user.UserName &&
                dbUser.Uuid != user.Uuid)
            {
                throw new DuplicateNameException();
            }
        }

        public async Task Add(UserDto user)
        {
            await UsernameExists(user);

            user.Uuid = Guid.NewGuid();
            user.Salt = SecurityLogic.GetSalt();
            user.Password = SecurityLogic.HashPassword(user.Password, user.Salt);
            await _userDal.Add(user);
        }

        public async Task<string> Login(UserDto user)
        {
            UserDto dbUser = await _userDal.Find(user.UserName) ?? throw new SecurityException();
            SecurityLogic.ValidatePassword(dbUser.Password, dbUser.Salt, user.Password);
            return JwtLogic.GenerateJwtToken(dbUser.Uuid);
        }

        public async Task Update(UserDto user, string providedPassword)
        {
            UserDto dbUser = await _userDal.Find(user.Uuid) ?? throw new KeyNotFoundException();
            await UsernameExists(user, dbUser);

            SecurityLogic.ValidatePassword(dbUser.Password, dbUser.Salt, providedPassword);
            user.Salt = SecurityLogic.GetSalt();
            user.Password = SecurityLogic.HashPassword(user.Password, user.Salt);

            await _userDal.Update(user);
        }

        public async Task Remove(UserDto user)
        {
            await _userDal.Remove(user.Uuid);
        }
    }
}
