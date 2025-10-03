using System.ComponentModel.DataAnnotations;

namespace DisasterAlleviationFoundationn.ViewModels
{
    public class ProfileViewModel
    {
        [Required(ErrorMessage = "First name is required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Invalid phone number")]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Profile Picture")]
        public IFormFile? ProfileImage { get; set; }

        public string? CurrentProfileImage { get; set; }

        // Profile statistics
        public int TotalIncidentsReported { get; set; }
        public int TotalDonationsMade { get; set; }
        public int TotalVolunteerApplications { get; set; }
        public DateTime MemberSince { get; set; }
        public string? Location { get; set; }
        
        //public string? Bio { get; set; } -
        /// </summary>

        [Display(Name = "Location")]
        public string? NewLocation { get; set; }

        //[Display(Name = "Bio")]
        //[StringLength(500, ErrorMessage = "Bio cannot exceed 500 characters")]
        //public string? NewBio { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Current password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "New password is required")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public class DeleteAccountViewModel
    {
        [Required(ErrorMessage = "Please confirm your password")]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please type 'DELETE' to confirm")]
        [Display(Name = "Type 'DELETE' to confirm")]
        [RegularExpression("DELETE", ErrorMessage = "Please type DELETE in uppercase to confirm account deletion")]
        public string Confirmation { get; set; } = string.Empty;
    }
}