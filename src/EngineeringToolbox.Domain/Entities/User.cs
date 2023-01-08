using EngineeringToolbox.Domain.Validators;
using EngineeringToolbox.Domain.ValueObjects;

namespace EngineeringToolbox.Domain.Entities
{
    public class User : BaseEntity
    {
        public Email Email { get; private set; }
        public string Password { get; private set; }
        public bool IsEmailConfirmed { get; private set; }

        public User(Email email, string password, bool isEmailConfirmed = false)
        {
            Id = Guid.NewGuid();
            Email = email;
            Password = password;
            IsEmailConfirmed = isEmailConfirmed;

            this.Validate(this, new UserValidator());
        }

        public void ChangeEmail(string email)
        {
            Email = new Email(email);
            this.Validate(this, new UserValidator());
        }

        public void ChangeConfirmEmailStatus(bool isConfirmed)
        {
            IsEmailConfirmed = isConfirmed;
        }

        public void ChangePassword(string password)
        {
            Password = password;
            this.Validate(this, new UserValidator());
        }
    }
}
