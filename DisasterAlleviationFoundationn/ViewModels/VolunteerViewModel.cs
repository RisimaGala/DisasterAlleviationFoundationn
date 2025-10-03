using System.ComponentModel.DataAnnotations;

namespace DisasterAlleviationFoundationn.ViewModels
{
    public class VolunteerViewModel
    {
        [Required(ErrorMessage = "Area of interest is required")]
        [Display(Name = "Area of Interest")]
        public string InterestArea { get; set; } = string.Empty;

        [Required(ErrorMessage = "Availability is required")]
        public string Availability { get; set; } = string.Empty;

        [Display(Name = "Skills & Experience")]
        public string? Skills { get; set; }

        [Display(Name = "I have reliable transportation")]
        public bool HasTransportation { get; set; }

        [Display(Name = "Contact Information")]
        public string? ContactInfo { get; set; }
        public string FullName { get; internal set; }
        public string Phone { get; internal set; }
        public string Email { get; internal set; }
    }
}