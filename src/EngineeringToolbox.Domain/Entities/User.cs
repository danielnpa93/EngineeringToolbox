using EngineeringToolbox.Domain.Validators;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace EngineeringToolbox.Domain.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public DateTime PasswordExpiresIn { get; private set; }

        [NotMapped]
        public string Password { get; private set; }

        [NotMapped]
        public bool Valid { get; private set; }

        [NotMapped]
        public ValidationResult ValidationResult { get; private set; }

        //EF
        protected User() { }
        public User(string email,
            string password,
            string firstName,
            string lastName)
        {
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            PasswordExpiresIn = DateTime.UtcNow;
            Email = email;
            UserName = email;

            Validate(this, new UserValidator());
        }

        public void ChangeEmail(string email)
        {
            Email = email;
            Validate(this, new UserValidator());
        }

        public void ChangePassword(string password)
        {
            Password = password;
            Validate(this, new UserValidator());
        }

        private bool Validate<TModel>(TModel model, AbstractValidator<TModel> validator)
        {
            ValidationResult = validator.Validate(model);
            return Valid = ValidationResult.IsValid;
        }
    }
}
