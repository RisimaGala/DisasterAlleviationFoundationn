using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DisasterAlleviationFoundationn.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Phone]
        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        public string? ProfileImage { get; set; }

        [StringLength(100)]
        public string? Location { get; set; }

        // public string Bio { get; set; } // I COMMENTED OUT Bcause the Database column doesn't exist yet

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;

        public bool IsEmailConfirmed { get; set; } = true;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [StringLength(50)]
        public string Role { get; set; } = "User";

        // Navigation properties
        public virtual ICollection<DisasterIncident>? ReportedIncidents { get; set; }
        public virtual ICollection<Donation>? Donations { get; set; }
        public virtual ICollection<Volunteer>? VolunteerApplications { get; set; }
    }
}