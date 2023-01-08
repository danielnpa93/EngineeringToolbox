using AutoMapper;
using EngineeringToolbox.Domain.Entities;
using EngineeringToolbox.Domain.Nofication;
using EngineeringToolbox.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace EngineeringToolbox.Infrastructure.Repositories
{
    public class IdentityRepository : IIdentityRepository
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly NotificationContext _notificationContext;
        private readonly IMapper _mapper;

        public IdentityRepository(SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            NotificationContext notificationContext,
            IMapper mapper)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _notificationContext = notificationContext;
            _mapper = mapper;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var identityUser = await _userManager.FindByEmailAsync(email);
            return _mapper.Map<User>(identityUser);

        }

        public async Task<IList<Claim>> GetUserClaims(User user)
        {
            var IdentityUser = _mapper.Map<IdentityUser>(user);
            return await _userManager.GetClaimsAsync(IdentityUser);
        }

        public async Task<IEnumerable<string>> GetUserRoles(User user)
        {
            var IdentityUser = _mapper.Map<IdentityUser>(user);
            return await _userManager.GetRolesAsync(IdentityUser);
        }


        public async Task<Guid> RegisterUser(User user)
        {
            var identityUser = _mapper.Map<IdentityUser>(user);

            var result = await _userManager.CreateAsync(identityUser, user.Password);

            if (!result.Succeeded)
            {
                _notificationContext.AddNotifications(result.Errors.Select(e => e.Description));
                return default;
            }

            return Guid.Parse(identityUser.Id);
        }

        public async Task<bool> ValidateUserLogin(User user)
        {
            var result = await _signInManager.PasswordSignInAsync(user.Email.Value, user.Password, false, true);

            if (result.IsLockedOut)
            {
                _notificationContext.AddNotification("Too many failed attempts. Your account is temporarily locked");
                return false;
            }

            if (!result.Succeeded)
            {
                _notificationContext.AddNotification("Invalid username or password");
                return false;
            }

            return true;
        }
    }
}
