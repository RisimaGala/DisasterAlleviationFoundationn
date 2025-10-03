using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace DisasterAlleviationFoundationn.ViewModels
{
    public class IncidentReportViewModel
    {
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

        [Required(ErrorMessage = "Severity level is required")]
        [Range(1, 10, ErrorMessage = "Severity level must be between 1 and 10")]
        public int SeverityLevel { get; set; } = 1;

        [Required(ErrorMessage = "People affected is required")]
        [Range(1, int.MaxValue, ErrorMessage = "People affected must be at least 1")]
        public int PeopleAffected { get; set; }

        public IFormFile? Image { get; set; }
    }
}