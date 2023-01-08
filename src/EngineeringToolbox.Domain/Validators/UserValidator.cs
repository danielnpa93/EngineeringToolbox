using EngineeringToolbox.Domain.Entities;
using FluentValidation;

namespace EngineeringToolbox.Domain.Validators
{
    public class UserValidator :  AbstractValidator<User>
    {
        public UserValidator()
        {

            RuleFor(x => x.Email)
                .SetValidator(x => new EmailValidator());
        }
    }
}
