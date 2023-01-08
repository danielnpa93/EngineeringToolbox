using EngineeringToolbox.Domain.Entities;
using FluentValidation;

namespace EngineeringToolbox.Domain.Validators
{
    public class UserValidator :  AbstractValidator<User>
    {
        public UserValidator()
        {

            RuleFor(x => x.Email)
                 .Length(5, 254)
                .Matches(@"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$")
                .WithMessage("Invalid Email");
        }
    }
}
