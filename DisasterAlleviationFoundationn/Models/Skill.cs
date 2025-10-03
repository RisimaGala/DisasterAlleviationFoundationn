using DisasterAlleviationFoundationn.Models.DisasterAlleviationFoundationn.Models;
using System.ComponentModel.DataAnnotations;

namespace DisasterAlleviationFoundationn.Models
{
    public class Skill
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Skill name is required")]
        [StringLength(100, ErrorMessage = "Skill name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        // Navigation property for volunteers with this skill (many-to-many)
        public virtual ICollection<VolunteerSkill>? Volunteers { get; set; }
    }
}