using System.ComponentModel.DataAnnotations;

namespace DisasterAlleviationFoundationn.Models
{
    public class DisasterIncident
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Location { get; set; } = string.Empty;

        [Required]
        public string DisasterType { get; set; } = string.Empty;

        [Required]
        public DateTime OccurredOn { get; set; }

        public DateTime ReportedOn { get; set; }

        [Required]
        public string SeverityLevel { get; set; } = string.Empty;

        public int PeopleAffected { get; set; }

        public string Status { get; set; } = "Reported";

        public string? ImageUrl { get; set; }

        public int ReportedByUserId { get; set; }
        public User ReportedByUser { get; internal set; }
    }
}