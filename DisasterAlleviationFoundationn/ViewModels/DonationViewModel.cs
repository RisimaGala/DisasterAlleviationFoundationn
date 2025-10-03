using System.ComponentModel.DataAnnotations;

namespace DisasterAlleviationFoundationn.ViewModels
{
    public class DonationViewModel
    {
        [Required(ErrorMessage = "Donation type is required")]
        [Display(Name = "Donation Type")]
        public string DonationType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Amount is required")]
        [Range(1, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Donor name is required")]
        [Display(Name = "Donor Name")]
        [StringLength(100, ErrorMessage = "Donor name cannot exceed 100 characters")]
        public string DonorName { get; set; } = string.Empty;

        [Display(Name = "Description")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        [Display(Name = "Contact Information")]
        public string? ContactInfo { get; set; }

        [Display(Name = "Quantity")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; } = 1;
    }
}