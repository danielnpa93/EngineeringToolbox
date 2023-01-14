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
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly NotificationContext _notificationContext;
        private readonly IMapper _mapper;

        public IdentityRepository(SignInManager<User> signInManager,
            UserManager<User> userManager,
            NotificationContext notificationContext,
            IMapper mapper)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _notificationContext = notificationContext;
            _mapper = mapper;
        }

        public async Task<User> GetUserById(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<IList<Claim>> GetUserClaims(User user)
        {
            return await _userManager.GetClaimsAsync(user);
        }

        public async Task<IEnumerable<string>> GetUserRoles(User user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<IdentityResult> UpdateUser(User user)
        {
            return await _userManager.UpdateAsync(user);
        }


        public async Task<IdentityResult> UpdatePassword(User user, string oldPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        }


        public async Task<bool> RegisterUser(User user)
        {
            var result = await _userManager.CreateAsync(user, user.Password);

            if (!result.Succeeded)
            {
                _notificationContext.AddNotifications(result.Errors.Select(e => e.Description));
                return false;
            }

            return true;
        }

        public async Task<SignInResult> ValidateUserLogin(User user, bool lockOnFailure = true)
        {
            return await _signInManager.PasswordSignInAsync(user.Email, user.Password, false, lockOnFailure);

        }


    }
}
