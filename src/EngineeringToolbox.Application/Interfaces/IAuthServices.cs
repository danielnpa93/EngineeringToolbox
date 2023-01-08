using EngineeringToolbox.Application.ViewModels;
using EngineeringToolbox.Shared.Token;

namespace EngineeringToolbox.Application.Interfaces
{
    public interface IAuthServices
    {
        Task<Guid> RegisterClient(ClientRegisterViewModel model);
        Task<Guid> RegisterUser(UserRegisterViewModel model);
        Task<TokenModel> UserLogin(UserLoginViewModel model);

    }
}
