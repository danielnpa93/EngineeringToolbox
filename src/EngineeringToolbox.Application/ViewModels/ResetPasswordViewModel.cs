using EngineeringToolbox.Shared.Utils;
using System.ComponentModel.DataAnnotations;

namespace EngineeringToolbox.Application.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [NotEqual("OldPassword")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Password not matches")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel : ResetPasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
       
    }
}
