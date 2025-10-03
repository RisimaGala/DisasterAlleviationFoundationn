using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DisasterAlleviationFoundationn.ViewModels
{
    public class IncidentReportViewModel
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Disaster type is required")]
        [Display(Name = "Disaster Type")]
        public string DisasterType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Occurrence date is required")]
        [Display(Name = "Occurrence Date")]
        [DataType(DataType.Date)]
        public DateTime OccurredOn { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Location is required")]
        public string Location { get; set; } = string.Empty;

        [Required(ErrorMessage = "Severity level is required")]
        [Display(Name = "Severity Level")]
        public string SeverityLevel { get; set; } = string.Empty;

        [Display(Name = "People Affected")]
        [Range(0, int.MaxValue, ErrorMessage = "People affected must be a positive number")]
        public int PeopleAffected { get; set; }

        [Display(Name = "Upload Image (Optional)")]
        public IFormFile? Image { get; set; }
    }
}