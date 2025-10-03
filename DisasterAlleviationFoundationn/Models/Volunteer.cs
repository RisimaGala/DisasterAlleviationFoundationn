using DisasterAlleviationFoundationn.Models.DisasterAlleviationFoundationn.Models;
using System.ComponentModel.DataAnnotations;

namespace DisasterAlleviationFoundationn.Models
{
    public class Volunteer
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "User ID is required")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Availability is required")]
        public string Availability { get; set; } = string.Empty;

        [Required(ErrorMessage = "Location is required")]
        public string Location { get; set; } = string.Empty;

        public bool HasTransportation { get; set; }

        public string Status { get; set; } = "Pending";

        public DateTime AppliedOn { get; set; } = DateTime.Now;

        // Navigation property to User
        public virtual User? User { get; set; }

        // Skills relationship (many-to-many)
        public virtual ICollection<VolunteerSkill>? Skills { get; set; }
    }
}