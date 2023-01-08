using AutoMapper;
using EngineeringToolbox.Application.ViewModels;
using EngineeringToolbox.Domain.Entities;
using EngineeringToolbox.Shared.Utils;

namespace EngineeringToolbox.Application.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserRegisterViewModel, User>()
                .ConstructUsing(src => new User(src.Email, GetUserRegisterPassword(), src.FirstName, src.LastName));

            CreateMap<UserLoginViewModel, User>()
                .ConstructUsing(src => new User(src.Email, src.Password, null, null));
        }

        private string GetUserRegisterPassword()
        {
            return PasswordGenerator.GeneratePassword();
        }

    }
}
