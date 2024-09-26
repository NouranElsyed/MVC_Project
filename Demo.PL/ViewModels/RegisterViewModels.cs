using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
    public class RegisterViewModels
    {
        [Required(ErrorMessage = "Name is required")]

        public string Name { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage ="Invalid Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Repeat Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Password doesn't match")]
        public string RepeatPassword { get; set; }

        public bool IAgree{ get; set; }

    }
}
