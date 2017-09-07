using System.ComponentModel.DataAnnotations;

namespace NoteShareAPI.Models
{
    public class Credentials
    {
        [Required]
        [EmailAddress(ErrorMessage = "Please provide a valid email address.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
