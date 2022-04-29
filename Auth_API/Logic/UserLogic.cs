using Auth_API.CustomExceptions;
using Auth_API.Enums;
using Auth_API.Interfaces.Dal;
using Auth_API.Models.Dto;
using Auth_API.Models.Dto.Tokens;
using Auth_API.Models.Dto.User;
using Auth_API.Models.Helper;
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
        private readonly IUserActivationDal _userActivationDal;
        private readonly IDisabledUserDal _disabledUserDal;

        private static readonly string FrontEndUrl = Environment.GetEnvironmentVariable("FRONTENDURL") ?? throw new NoNullAllowedException("Environment variable" +
                                                                                                                                           "FRONTENDURL was empty. Set it using the FRONTENDURL environment variable");

        public UserLogic(IUserDal userDal, IUserTokenDal userTokenDal, IUserActivationDal userActivationDal, IDisabledUserDal disabledUserDal)
        {
            _userDal = userDal;
            _userTokenDal = userTokenDal;
            _userActivationDal = userActivationDal;
            _disabledUserDal = disabledUserDal;
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
            dbUser ??= await _userDal.Find(user.Username);
            if (dbUser != null &&
                dbUser.Username == user.Username &&
                dbUser.Uuid != user.Uuid)
            {
                throw new DuplicateNameException();
            }
        }

        private static void ValidateUser(UserDto user)
        {
            bool userValid = !string.IsNullOrEmpty(user.Password) &&
                             !string.IsNullOrEmpty(user.Email)
                             && !string.IsNullOrEmpty(user.Username);

            if (!userValid)
            {
                throw new InvalidDataException();
            }
        }

        public async Task Add(UserDto user)
        {
            ValidateUser(user);
            await UsernameExists(user);

            user.Uuid = Guid.NewGuid();
            user.Salt = SecurityLogic.GetSalt();
            user.Password = SecurityLogic.HashPassword(user.Password, user.Salt);
            await _userDal.Add(user);
        }

        public async Task<UserTokensViewmodel> Login(UserDto user, IPAddress? ipAddress)
        {
            UserDto dbUser = await _userDal.Find(user.Username) ?? throw new SecurityException();
            DisabledUserDto? disabledUser = await _disabledUserDal.Find(dbUser.Uuid);
            if (disabledUser != null)
            {
                throw new UserDisabledException(DisabledUserHelper.ConvertEnumToFriendlyMessage(disabledUser.DisabledReason));
            }

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

        public async Task<UserDto?> Find(Guid uuid)
        {
            return await _userDal.Find(uuid);
        }

        public async Task<UserTokensViewmodel> RefreshToken(UserTokensViewmodel tokens, IPAddress ipAddress)
        {
            Claim userUuidClaim = JwtLogic.GetJwtClaims(tokens.Jwt).Single(c => c.Type == "uuid");
            Guid userUuid = Guid.Parse(userUuidClaim.Value);
            DisabledUserDto? disabledUser = await _disabledUserDal.Find(userUuid);
            if (disabledUser != null)
            {
                throw new UserDisabledException(DisabledUserHelper.ConvertEnumToFriendlyMessage(disabledUser.DisabledReason));
            }

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

        public async Task Update(UserDto user, string newPassword)
        {
            ValidateUser(user);
            UserDto dbUser = await _userDal.Find(user.Uuid) ?? throw new KeyNotFoundException();
            await UsernameExists(user, dbUser);

            SecurityLogic.ValidatePassword(dbUser.Password, dbUser.Salt, user.Password);
            if (!string.IsNullOrEmpty(newPassword))
            {
                dbUser.Salt = SecurityLogic.GetSalt();
                dbUser.Password = SecurityLogic.HashPassword(newPassword, dbUser.Salt);
            }
            if (dbUser.Email != user.Email)
            {
                dbUser.Email = user.Email;
                await SetEmailChangeState(dbUser);
            }

            await _userDal.Update(dbUser);
        }

        /// <summary>
        /// Disables the user and send an activation email
        /// </summary>
        /// <param name="dbUser">The user to disable</param>
        private async Task SetEmailChangeState(UserDto dbUser)
        {
            try
            {
                await _disabledUserDal.Add(new DisabledUserDto
                {
                    Uuid = Guid.NewGuid(),
                    UserUuid = dbUser.Uuid,
                    DisabledReason = DisabledReason.EmailNeedsToBeValidated
                });

                UserActivationDto activation = new()
                {
                    Uuid = Guid.NewGuid(),
                    UserUuid = dbUser.Uuid,
                    Code = Guid.NewGuid()
                };
                await _userActivationDal.Add(activation);

                Dictionary<string, string> keyWordDictionary = new()
                {
                    { "Url", $"{FrontEndUrl}account-activation?code={activation.Code}" }
                };

                string body = EmailLogic.GetHtmlFormattedEmailBody(EmailTemplatePath.EmailValidation, keyWordDictionary);
                EmailLogic.Send(new Email
                {
                    EmailAddress = dbUser.Email,
                    Message = body,
                    Subject = "Email validation"
                });
            }
            catch (Exception)
            {
                await _disabledUserDal.Remove(dbUser.Uuid);
                throw;
            }
        }

        public async Task ActivateUserAccount(Guid code)
        {
            UserActivationDto userActivation = await _userActivationDal.Find(code) ?? throw new KeyNotFoundException();
            await _disabledUserDal.Remove(userActivation.UserUuid);
            await _userActivationDal.Remove(userActivation.UserUuid);
        }

        public async Task ResetPassword(Guid code, string newPassword)
        {
            UserActivationDto? passwordReset = await _userActivationDal.Find(code);
            if (passwordReset == null)
            {
                throw new KeyNotFoundException();
            }

            UserDto? userToReset = await _userDal.Find(passwordReset.UserUuid);
            if (userToReset == null)
            {
                throw new KeyNotFoundException();
            }

            userToReset.Salt = SecurityLogic.GetSalt();
            userToReset.Password = SecurityLogic.HashPassword(newPassword, userToReset.Salt);
            await _userDal.Update(userToReset);
            await _userActivationDal.Remove(passwordReset.UserUuid);
        }

        public async Task RequestPasswordReset(string email)
        {
            UserDto? user = await _userDal.FindByEmail(email);
            if (user == null)
            {
                throw new KeyNotFoundException();
            }

            UserActivationDto? existingPasswordReset = await _userActivationDal.Find(user.Uuid);
            if (existingPasswordReset != null)
            {
                await _userActivationDal.Remove(existingPasswordReset.Uuid);
            }

            UserActivationDto userActivation = new()
            {
                Code = Guid.NewGuid(),
                UserUuid = user.Uuid,
                Uuid = Guid.NewGuid()
            };

            await _userActivationDal.Add(userActivation);
            Dictionary<string, string> keyWordDictionary = new() { { "ResetUrl", $"{FrontEndUrl}reset-password?code={userActivation.Code}" } };
            string emailBody = EmailLogic.GetHtmlFormattedEmailBody(EmailTemplatePath.ForgotPassword, keyWordDictionary);

            try
            {
                EmailLogic.Send(new Email
                {
                    EmailAddress = user.Email,
                    Subject = "Password reset",
                    Message = emailBody
                });
            }
            catch (Exception)
            {
                await _userActivationDal.Remove(userActivation.Uuid);
            }
        }

        public async Task Remove(UserDto user)
        {
            await _userDal.Remove(user.Uuid);
        }
    }
}
