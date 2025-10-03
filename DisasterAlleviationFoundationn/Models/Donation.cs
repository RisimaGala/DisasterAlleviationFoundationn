using DisasterAlleviationFoundationn.Models.DisasterAlleviationFoundationn.Models;
using System.ComponentModel.DataAnnotations;

namespace DisasterAlleviationFoundationn.Models
{
    public class Donation
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Donation type is required")]
        public string Type { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Quantity is required")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Unit is required")]
        public string Unit { get; set; } = string.Empty;

        public DateTime DonatedOn { get; set; } = DateTime.Now;

        public DateTime? ExpiryDate { get; set; }

        public string Status { get; set; } = "Pending";

        // Foreign keys
        public int DonorUserId { get; set; }
        public int? AssignedDisasterId { get; set; }

        // Navigation properties
        public virtual User? Donor { get; set; }
        public virtual DisasterIncident? AssignedDisaster { get; set; }
    }
}