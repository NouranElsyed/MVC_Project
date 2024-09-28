using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage ="New Password is required")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required(ErrorMessage ="Confirm New password is required")]
        [DataType(DataType.Password)]
        [Compare("NewPassword",ErrorMessage ="Password doesn't match")]
        public string ConfirmPassword { get; set; }
    }
}
