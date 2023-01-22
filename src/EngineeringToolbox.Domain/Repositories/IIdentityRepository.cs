using EngineeringToolbox.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace EngineeringToolbox.Domain.Repositories
{
    public interface IIdentityRepository
    {
        Task<SignInResult> ValidateUserLogin(User user, bool lockOnFailure = true);
        Task<IdentityResult> RegisterUser(User user);
        Task<User> GetUserByEmail(string email);
        Task<IList<Claim>> GetUserClaims(User user);
        Task<IEnumerable<string>> GetUserRoles(User user);
        Task<User> GetUserById(string id);
        Task<IdentityResult> UpdateUser(User user);
        Task<IdentityResult> UpdatePassword(User user, string oldPassword, string newPassword);
        Task<IdentityResult> ResetPassword(User user, string token, string newPassword);
        Task<string> GetResetPasswordToken(User user);
        Task<string> GetChangeEmailToken(User user, string newEmail);
        Task<bool> ChangeEmail(User user, string token, string email);
    }
}
