using AutoMapper;
using EngineeringToolbox.Application.ViewModels;
using EngineeringToolbox.Domain.Entities;
using EngineeringToolbox.Domain.ValueObjects;
using EngineeringToolbox.Shared.Utils;

namespace EngineeringToolbox.Application.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserRegisterViewModel, User>()
                .ConstructUsing(src => new User(GetEmail(src.Email), GetUserRegisterPassword(), false));

            CreateMap<UserLoginViewModel, User>()
                .ConstructUsing(src => new User(GetEmail(src.Email), src.Password, false));
        }

        private Email GetEmail(string email)
        {
            return new Email(email);
        }

        private string GetUserRegisterPassword()
        {
            return PasswordGenerator.GeneratePassword();
        }

    }
}
