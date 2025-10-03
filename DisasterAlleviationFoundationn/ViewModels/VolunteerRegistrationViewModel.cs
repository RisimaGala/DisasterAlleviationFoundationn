namespace DisasterAlleviationFoundationn.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    namespace DisasterAlleviationFoundationn.ViewModels
    {
        public class VolunteerRegistrationViewModel
        {
            public List<int>? SelectedSkillIds { get; set; }

            [Required(ErrorMessage = "Availability is required")]
            [Display(Name = "When are you available?")]
            public string Availability { get; set; } = string.Empty;

            [Required(ErrorMessage = "Location is required")]
            [Display(Name = "Your Location")]
            public string Location { get; set; } = string.Empty;

            [Display(Name = "I have reliable transportation")]
            public bool HasTransportation { get; set; }
        }
    }
}
