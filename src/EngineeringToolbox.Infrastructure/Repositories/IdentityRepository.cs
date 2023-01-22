using AutoMapper;
using EngineeringToolbox.Domain.Entities;
using EngineeringToolbox.Domain.Extensions;
using EngineeringToolbox.Domain.Nofication;
using EngineeringToolbox.Domain.Repositories;
using EngineeringToolbox.Infrastructure.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Transactions;

namespace EngineeringToolbox.Infrastructure.Repositories
{
    public class IdentityRepository : IIdentityRepository
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly NotificationContext _notificationContext;
        private readonly IMapper _mapper;

        public IdentityRepository(SignInManager<User> signInManager,
            UserManager<User> userManager,
            ApplicationDbContext context,
            NotificationContext notificationContext,
            IMapper mapper)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
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

        public async Task<IdentityResult> ResetPassword(User user, string token, string newPassword)
        {
            return await _userManager.ResetPasswordAsync(user, token, newPassword);
        }

        public async Task<IdentityResult> RegisterUser(User user)
        {
            return await _userManager.CreateAsync(user, user.Password);
        }

        public async Task<SignInResult> ValidateUserLogin(User user, bool lockOnFailure = true)
        {
            return await _signInManager.PasswordSignInAsync(user.Email, user.Password, false, lockOnFailure);

        }

        public async Task<string> GetResetPasswordToken(User user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<string> GetChangeEmailToken(User user, string newEmail)
        {
            return await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);
        }

        public async Task<bool> ChangeEmail(User user, string token, string email)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var changeEmail = await _userManager.ChangeEmailAsync(user, email,token);
                    var changeUserName = await _userManager.SetUserNameAsync(user, email);

                    if (!changeUserName.Succeeded || !changeEmail.Succeeded)
                    {
                        throw new Exception();
                    }

                    scope.Complete();
                    return true;
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    return false;
                }
            }
        }
    }
}
