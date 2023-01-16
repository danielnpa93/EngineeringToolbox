using EngineeringToolbox.Application.ViewModels;
using EngineeringToolbox.Shared.Token;

namespace EngineeringToolbox.Application.Interfaces
{
    public interface IAuthServices
    {
        Task<Guid> RegisterClient(ClientRegisterViewModel model);
        Task<Guid> RegisterUser(UserViewModel model);
        Task<TokenModel> UserLogin(UserLoginViewModel model);
        Task ChangePassword(ChangePasswordViewModel model);
        Task ResetPassword(ResetPasswordViewModel model, string token, string userId);
        Task<UserViewModel> UpdateUser(UserViewModel model);
        Task ForgotPassword(string email);
    }
}
