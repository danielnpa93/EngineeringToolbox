using EngineeringToolbox.Domain.Entities;
using System.Security.Claims;

namespace EngineeringToolbox.Domain.Repositories
{
    public interface IIdentityRepository
    {
        Task<bool> ValidateUserLogin(User user);
        Task<bool> RegisterUser(User user);
        Task<User> GetUserByEmail(string email);
        Task<IList<Claim>> GetUserClaims(User user);
        Task<IEnumerable<string>> GetUserRoles(User user);
    }
}
