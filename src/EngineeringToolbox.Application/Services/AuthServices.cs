using AutoMapper;
using EngineeringToolbox.Application.Interfaces;
using EngineeringToolbox.Application.ViewModels;
using EngineeringToolbox.Domain.Entities;
using EngineeringToolbox.Domain.Extensions;
using EngineeringToolbox.Domain.Nofication;
using EngineeringToolbox.Domain.Repositories;
using EngineeringToolbox.Domain.Settings;
using EngineeringToolbox.Shared.Token;
using EngineeringToolbox.Shared.Utils;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EngineeringToolbox.Application.Services
{
    public class AuthServices : IAuthServices
    {
        private readonly IIdentityRepository _identityRepository;
        private readonly IMapper _mapper;
        private readonly ISettings _settings;
        private readonly IUser _user;
        private readonly NotificationContext _notification;

        public AuthServices(IIdentityRepository identityRepository,
            IMapper mapper,
            ISettings settings,
            IUser user,
            NotificationContext notification)
        {
            _identityRepository = identityRepository;
            _mapper = mapper;
            _settings = settings;
            _user = user;
            _notification = notification;
        }

        public async Task<TokenModel> UserLogin(UserLoginViewModel model)
        {
            var user = _mapper.Map<User>(model);
            var result = await _identityRepository.ValidateUserLogin(user);

            if (result.IsLockedOut)
            {
                _notification.AddNotification("Too many failed attempts. Your account is temporarily locked");
                return default;
            }

            if (!result.Succeeded)
            {
                _notification.AddNotification("Invalid username or password");
                return default;
            }

            return await GetToken(user.Email);
        }

        public Task<Guid> RegisterClient(ClientRegisterViewModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<Guid> RegisterUser(UserViewModel model)
        {
            var user = _mapper.Map<User>(model);

            if (!user.Valid)
            {
                _notification.AddNotifications(user.ValidationResult);
                return default;
            }

            var emailAlreadyExists = await _identityRepository.GetUserByEmail(user.Email);

            if (emailAlreadyExists != null)
            {
                _notification.AddNotification("Email already registered");
                return default;
            }

            var result = await _identityRepository.RegisterUser(user);

            if (!result.Succeeded)
            {
                _notification.AddNotifications(result.Errors.Select(e => e.Description));
                return default;
            }

            var message = GenerateRegistrationEmail(user);

            EmailHandler.SendEmail(_settings.EmailAdress, user.Email, "Engineering Toolbox Registration",
               message, _settings.EmailPassword, "DNSoft");

            return Guid.Parse(user.Id);
        }

        public async Task ChangePassword(ChangePasswordViewModel model)
        {
            var user = await _identityRepository.GetUserById(_user.GetUserId().ToString());
            user.ChangePassword(model.NewPassword);

            if (!user.Valid)
            {
                _notification.AddNotifications(user.ValidationResult);
                return;
            }

            user.EmailConfirmed = true;
            var result = await _identityRepository.UpdatePassword(user, model.OldPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                _notification.AddNotifications(result.Errors.Select(x => x.Description));
                return;
            }
        }

        public async Task ResetPassword(ResetPasswordViewModel model, string token, string userId)
        {
            var user = await _identityRepository.GetUserById(userId);
            user.ChangePassword(model.NewPassword);

            if (!user.Valid)
            {
                _notification.AddNotifications(user.ValidationResult);
                return;
            }

            user.EmailConfirmed = true;
            var result = await _identityRepository.ResetPassword(user, token, model.NewPassword);

            if (!result.Succeeded)
            {
                _notification.AddNotifications(result.Errors.Select(x => x.Description));
                return;
            }
        }

        private async Task<TokenModel> GetToken(string userEmail)
        {
            var user = await _identityRepository.GetUserByEmail(userEmail);
            var claims = await _identityRepository.GetUserClaims(user);

            var identityClaims = await GetUserClaims(claims, user);
            var encodedToken = EcondedToken(identityClaims);

            return new TokenModel
            {
                ExpiresIn = DateTime.UtcNow.AddMilliseconds(_settings.TokenExpiresInMiliSeconds),
                Token = encodedToken,
                PasswordExpired = user.PasswordExpiresIn < DateTime.UtcNow,
            };
        }

        private string EcondedToken(ClaimsIdentity identityClaims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_settings.IdentitySecret);

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _settings.TokenEmmiter,
                Audience = _settings.TokenAvailableDomains,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddMilliseconds(_settings.TokenExpiresInMiliSeconds),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            return tokenHandler.WriteToken(token);
        }

        private async Task<ClaimsIdentity> GetUserClaims(IList<Claim> claims, User user)
        {
            var userRoles = await _identityRepository.GetUserRoles(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim("role", userRole));
            }

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            return identityClaims;
        }

        private static long ToUnixEpochDate(DateTime date)
           => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

        private string GenerateRegistrationEmail(User user)
        {
            StringBuilder template = new StringBuilder();

            template.AppendLine($"<h4>Hello {user.FirstName} {user.LastName},</h4>");
            template.AppendLine($"<p>Welcome to Engineering Toolbox!</p>");
            template.AppendLine("<p>We received a registration in this email. To continue, use the password below: </p>");
            template.AppendLine($"<p><strong>{user.Password}</strong></p>");
            template.AppendLine("</br>");
            template.AppendLine("<p>Good Simulations!</p>");

            return template.ToString();
        }

        private string GenerateForgotPasswordEmail(string link)
        {
            StringBuilder template = new StringBuilder();

            template.AppendLine($"<h4>Password reset</h4>");
            template.AppendLine("<p>Click on link bellow to reset your password:</p>");
            template.AppendLine($"<p><strong>{link}</strong></p>"); //TODO LINK to spa form

            return template.ToString();
        }

        public Task<UserViewModel> UpdateUser(UserViewModel model)
        {
            throw new NotImplementedException();
        }

        public async Task ForgotPassword(string email)
        {
            var user = await _identityRepository.GetUserByEmail(email);

            if (user == null)
            {
                return; //do not send message if user exists
            }
            var token = await _identityRepository.GetResetPasswordToken(user); //TODO encode token

            var message = GenerateForgotPasswordEmail($"https://localhost?token={token}&id={user.Id}");

            EmailHandler.SendEmail(_settings.EmailAdress, user.Email, "Engineering Toolbox Password Reset",
                message, _settings.EmailPassword, "DNSoft");

            return;

        }

        public async Task ChangeEmailRequest(string newEmail)
        {
            var user = await _identityRepository.GetUserById(_user.GetUserId().ToString());
            var oldEmail = user.Email;

            user.ChangeEmail(newEmail);

            if (!user.Valid)
            {
                _notification.AddNotifications(user.ValidationResult);
                return;
            }

            var alreadyHasUser = await _identityRepository.GetUserByEmail(newEmail);

            if (alreadyHasUser != null)
            {
                _notification.AddNotification("Email already in use");
                return;
            }

            var token = await _identityRepository.GetChangeEmailToken(user, newEmail);

            var code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            //TODO get spa link on app settings
            string link = $"https://localhost:7128/api/v1/identity/change_email?token={code}&email={newEmail}&userId={user.Id}";

            var message = GenerateChangeEmailConfirmation(link, oldEmail, newEmail);

            EmailHandler.SendEmail(_settings.EmailAdress, oldEmail, "Engineering Toolbox Email Change",
                message, _settings.EmailPassword, "DNSoft");

        }

        private string GenerateChangeEmailConfirmation(string link, string oldEmail, string newEmail)
        {
            StringBuilder template = new StringBuilder();

            template.AppendLine($"<h4>Change email confirmation</h4>");
            template.AppendLine($"<p>We receive a change email request from <b>{oldEmail}</b> to <b>{newEmail}</b></p>");
            template.AppendLine("<p>Click on link bellow to confirm the change, if was not you ignore this email.</p>");
            template.AppendLine($"<a href={link}>Click here</a>");

            return template.ToString();
        }

        public async Task<bool> ChangeEmail(string token, string email, string userId)
        {
            var user = await _identityRepository.GetUserById(userId);

            var code = Encoding.ASCII.GetString(WebEncoders.Base64UrlDecode(token));

            return await _identityRepository.ChangeEmail(user, code, email);
        }
    }
}
