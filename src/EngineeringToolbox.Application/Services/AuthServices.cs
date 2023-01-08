using AutoMapper;
using EngineeringToolbox.Application.Interfaces;
using EngineeringToolbox.Application.ViewModels;
using EngineeringToolbox.Domain.Entities;
using EngineeringToolbox.Domain.Nofication;
using EngineeringToolbox.Domain.Repositories;
using EngineeringToolbox.Domain.Settings;
using EngineeringToolbox.Shared.Token;
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
        private readonly NotificationContext _notification;

        public AuthServices(IIdentityRepository identityRepository,
            IMapper mapper,
            ISettings settings,
            NotificationContext notification)
        {
            _identityRepository = identityRepository;
            _mapper = mapper;
            _settings = settings;
            _notification = notification;
        }

        public async Task<TokenModel> UserLogin(UserLoginViewModel model)
        {
            var user = _mapper.Map<User>(model);
            var isUserValid = await _identityRepository.ValidateUserLogin(user);

            if (!isUserValid)
                return default;

            return await GetToken(user.Email.Value);
        }

        public Task<Guid> RegisterClient(ClientRegisterViewModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<Guid> RegisterUser(UserRegisterViewModel model)
        {
            var user = _mapper.Map<User>(model);

            if (!user.Valid)
            {
                _notification.AddNotifications(user.ValidationResult);
                return default;
            }

            var emailAlreadyExists = await _identityRepository.GetUserByEmail(user.Email.Value);

            if(emailAlreadyExists != null)
            {
                _notification.AddNotification("Email already registered");
                return default;
            }

            return await _identityRepository.RegisterUser(user);
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
                EmailConfirmed = user.IsEmailConfirmed
            };
        }

        private static long ToUnixEpochDate(DateTime date)
           => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}
