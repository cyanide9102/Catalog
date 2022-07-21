using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Catalog.WebUI.ViewModels.AccountViewModels
{
    public class RegisterViewModel
    {
        private const string MAX_LENGTH_ERROR_MESSAGE = "{0} cannot have more than {1} characters.";

        [Required]
        [StringLength(50, ErrorMessage = MAX_LENGTH_ERROR_MESSAGE, MinimumLength = 4)]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Password length must be greater than or equal to {2} characters.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone")]
        public string PhoneNumber { get; set; }

        public string ReturnUrl { get; set; } = "/";
    }
}
