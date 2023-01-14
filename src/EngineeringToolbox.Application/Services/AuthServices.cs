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

            var success = await _identityRepository.RegisterUser(user);

            if (!success)
                return default;


            var message = GenerateRegistrationEmail(user);

            var sendEmail = await EmailHandler.SendEmail(_settings.EmailAdress, user.Email, "Engineering Toolbox Registration",
                message, _settings.EmailPassword, "DNSoft");

            if (!sendEmail)
            {
                _notification.AddNotification("Error to send registration email");
                return default;

            }

            return Guid.Parse(user.Id);
        }

        public async Task ResetPassword(ResetPasswordViewModel model)
        {
            if (model.NewPassword != model.ConfirmPassword)
            {
                _notification.AddNotification("Passwords not matches");
                return;
            }

            var user = await _identityRepository.GetUserById(_user.GetUserId().ToString());
            user.ChangePassword(model.OldPassword);

            var result = await _identityRepository.ValidateUserLogin(user, false);

            if (!result.Succeeded)
            {
                _notification.AddNotification("Invalid password");
                return;
            }

            if(model.NewPassword == model.OldPassword)
            {
                _notification.AddNotification("Use another password");
                return;
            }

            user.ChangePassword(model.NewPassword);

            var updateResult = await _identityRepository.UpdatePassword(user, model.OldPassword, model.NewPassword);

            if (!updateResult.Succeeded)
            {
                _notification.AddNotifications(updateResult.Errors.Select(x => x.Description));
                return;
            }

            if (!user.EmailConfirmed)
            {
                user.EmailConfirmed = true;
                await _identityRepository.UpdateUser(user);
            }

            return;
        }


        private async Task<TokenModel> GetToken(string userEmail)
        {
            var user = await _identityRepository.GetUserByEmail(userEmail);
            var claims = await _identityRepository.GetUserClaims(user);
            var userRoles = await _identityRepository.GetUserRoles(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, userEmail));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim("role", userRole));
            }

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);
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

            return new TokenModel
            {
                ExpiresIn = token.ValidTo,
                Token = tokenHandler.WriteToken(token),
                PasswordExpired = user.PasswordExpiresIn < DateTime.UtcNow,
            };
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

        public Task<UserViewModel> UpdateUser(UserViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}
