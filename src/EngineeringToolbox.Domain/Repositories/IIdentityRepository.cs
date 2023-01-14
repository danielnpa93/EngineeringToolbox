using EngineeringToolbox.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace EngineeringToolbox.Domain.Repositories
{
    public interface IIdentityRepository
    {
        Task<SignInResult> ValidateUserLogin(User user, bool lockOnFailure = true);
        Task<bool> RegisterUser(User user);
        Task<User> GetUserByEmail(string email);
        Task<IList<Claim>> GetUserClaims(User user);
        Task<IEnumerable<string>> GetUserRoles(User user);
        Task<User> GetUserById(string id);
        Task<IdentityResult> UpdateUser(User user);
        Task<IdentityResult> UpdatePassword(User user, string oldPassword, string newPassword);
    }
}
