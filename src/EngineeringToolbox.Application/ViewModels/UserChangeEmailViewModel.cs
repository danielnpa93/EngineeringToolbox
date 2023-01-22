using System.ComponentModel.DataAnnotations;

namespace EngineeringToolbox.Application.ViewModels
{
    public class UserChangeEmailViewModel
    {
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.EmailAddress)]
        public string NewEmail { get; set; }
    }
}
