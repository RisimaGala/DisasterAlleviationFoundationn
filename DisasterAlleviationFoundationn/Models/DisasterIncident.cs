using System.ComponentModel.DataAnnotations;

namespace DisasterAlleviationFoundationn.Models
{
    public class DisasterIncident
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Location is required")]
        public string Location { get; set; } = string.Empty;

        [Required(ErrorMessage = "Disaster type is required")]
        public string DisasterType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Occurrence date is required")]
        public DateTime OccurredOn { get; set; } = DateTime.Now;

        public DateTime ReportedOn { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Severity level is required")]
        public int SeverityLevel { get; set; } = 1;

        [Required(ErrorMessage = "People affected is required")]
        public int PeopleAffected { get; set; }

        public string Status { get; set; } = "Reported";
        public string? ImageUrl { get; set; }

        public int ReportedByUserId { get; set; }
        public virtual User? ReportedByUser { get; set; }
    }
}