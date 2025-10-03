using System.ComponentModel.DataAnnotations;

namespace DisasterAlleviationFoundationn.ViewModels
{
    public class DonationViewModel
    {
        [Required(ErrorMessage = "Donation type is required")]
        [Display(Name = "Donation Type")]
        public string DonationType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Quantity/Amount is required")]
        [Display(Name = "Quantity/Amount")]
        public string Quantity { get; set; } = string.Empty;

        [Display(Name = "Your Contact Information")]
        public string? ContactInfo { get; set; }
    }
}