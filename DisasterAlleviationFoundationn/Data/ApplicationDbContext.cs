using DisasterAlleviationFoundationn.Models;
using DisasterAlleviationFoundationn.Models.DisasterAlleviationFoundationn.Models;
using Microsoft.EntityFrameworkCore;

namespace DisasterAlleviationFoundationn.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<DisasterIncident> DisasterIncidents { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<Volunteer> Volunteers { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<VolunteerSkill> VolunteerSkills { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configures relationships
            modelBuilder.Entity<Volunteer>()
                .HasOne(v => v.User)
                .WithMany(u => u.VolunteerApplications)
                .HasForeignKey(v => v.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Many-to-many relationship between Volunteer and Skill
            modelBuilder.Entity<VolunteerSkill>()
                .HasKey(vs => new { vs.VolunteerId, vs.SkillId });

            modelBuilder.Entity<VolunteerSkill>()
                .HasOne(vs => vs.Volunteer)
                .WithMany(v => v.Skills)
                .HasForeignKey(vs => vs.VolunteerId);

            modelBuilder.Entity<VolunteerSkill>()
                .HasOne(vs => vs.Skill)
                .WithMany(s => s.Volunteers)
                .HasForeignKey(vs => vs.SkillId);

            // Seeds initial skills
            modelBuilder.Entity<Skill>().HasData(
                new Skill { Id = 1, Name = "First Aid/CPR" },
                new Skill { Id = 2, Name = "Search and Rescue" },
                new Skill { Id = 3, Name = "Medical Assistance" },
                new Skill { Id = 4, Name = "Construction" },
                new Skill { Id = 5, Name = "Counseling" },
                new Skill { Id = 6, Name = "Logistics" },
                new Skill { Id = 7, Name = "Communication" },
                new Skill { Id = 8, Name = "Cooking" }
            );
        }
    }
}