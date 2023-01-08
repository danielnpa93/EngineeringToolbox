using AutoMapper;
using EngineeringToolbox.Domain.Entities;
using EngineeringToolbox.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace EngineeringToolbox.Infrastructure.Mapping
{
    public class IdentityUserProfile : Profile
    {
        public IdentityUserProfile()
        {
            CreateMap<User, IdentityUser>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id == Guid.Empty ? Guid.NewGuid() : src.Id))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email.Value));

            CreateMap<IdentityUser, User>()
                .ConstructUsing(src => new User(new Email(src.Email), null, src.EmailConfirmed));

        }
    }
}
