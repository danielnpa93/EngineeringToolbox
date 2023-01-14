using EngineeringToolbox.Application.ViewModels;
using EngineeringToolbox.Shared.Token;

namespace EngineeringToolbox.Application.Interfaces
{
    public interface IAuthServices
    {
        Task<Guid> RegisterClient(ClientRegisterViewModel model);
        Task<Guid> RegisterUser(UserViewModel model);
        Task<TokenModel> UserLogin(UserLoginViewModel model);
        Task ResetPassword(ResetPasswordViewModel model);
        Task<UserViewModel> UpdateUser(UserViewModel model);
    }
}
