namespace DisasterAlleviationFoundationn.Models
{
    using System.ComponentModel.DataAnnotations;

    namespace DisasterAlleviationFoundationn.Models
    {
        public class VolunteerSkill
        {
            [Key]
            public int Id { get; set; }

            public int VolunteerId { get; set; }
            public int SkillId { get; set; }

            // Navigation properties
            public virtual Volunteer? Volunteer { get; set; }
            public virtual Skill? Skill { get; set; }
        }
    }
}
