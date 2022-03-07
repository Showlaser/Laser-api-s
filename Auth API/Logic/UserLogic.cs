using Auth_API.Interfaces.Dal;
using Auth_API.Models.Dto.Tokens;
using Auth_API.Models.Dto.User;
using Auth_API.Models.ToFrontend;
using System.Data;
using System.Net;
using System.Security;
using System.Security.Claims;

namespace Auth_API.Logic
{
    public class UserLogic
    {
        private readonly IUserDal _userDal;
        private readonly IUserTokenDal _userTokenDal;

        public UserLogic(IUserDal userDal, IUserTokenDal userTokenDal)
        {
            _userDal = userDal;
            _userTokenDal = userTokenDal;
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

        public async Task<UserTokensViewmodel> Login(UserDto user, IPAddress? ipAddress)
        {
            UserDto dbUser = await _userDal.Find(user.UserName) ?? throw new SecurityException();
            SecurityLogic.ValidatePassword(dbUser.Password, dbUser.Salt, user.Password);
            UserTokensDto userTokensDto = SecurityLogic.GenerateRefreshToken(dbUser.Uuid, ipAddress);
            UserTokensViewmodel tokens = new()
            {
                Jwt = JwtLogic.GenerateJwtToken(dbUser.Uuid),
                RefreshToken = userTokensDto.RefreshToken
            };

            await _userTokenDal.Remove(dbUser.Uuid);
            await _userTokenDal.Add(userTokensDto);
            return new UserTokensViewmodel
            {
                Jwt = tokens.Jwt,
                RefreshToken = tokens.RefreshToken
            };
        }

        public async Task<UserTokensViewmodel> RefreshToken(UserTokensViewmodel tokens, IPAddress ipAddress)
        {
            Claim userUuidClaim = JwtLogic.GetJwtClaims(tokens.Jwt).Single(c => c.Type == "uuid");
            Guid userUuid = Guid.Parse(userUuidClaim.Value);
            UserTokensDto dbTokens = await _userTokenDal.Find(userUuid) ?? throw new SecurityException();

            bool refreshTokenValid = dbTokens.RefreshToken == tokens.RefreshToken &&
                               dbTokens.RefreshTokenExpireDate > DateTime.Now &&
                               Equals(dbTokens.ClientIp, ipAddress);
            if (!refreshTokenValid)
            {
                throw new SecurityException();
            }

            UserTokensDto newTokens = SecurityLogic.GenerateRefreshToken(userUuid, ipAddress);
            await _userTokenDal.Remove(userUuid);
            await _userTokenDal.Add(newTokens);

            return new UserTokensViewmodel
            {
                Jwt = JwtLogic.GenerateJwtToken(userUuid),
                RefreshToken = newTokens.RefreshToken,
            };
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
