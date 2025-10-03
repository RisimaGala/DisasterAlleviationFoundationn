using DisasterAlleviationFoundationn.Data;
using DisasterAlleviationFoundationn.Models;
using DisasterAlleviationFoundationn.Models.DisasterAlleviationFoundationn.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DisasterAlleviationFoundationn.Tests.Data
{
    public class ApplicationDbContextTests
    {
        private ApplicationDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task CanAddAndRetrieveUser()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var user = new User
            {
                FirstName = "Test",
                LastName = "User",
                Email = "test@example.com",
                PasswordHash = "hashed_password",
                Role = "User"
            };

            // Act
            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Assert
            var savedUser = await context.Users.FirstOrDefaultAsync(u => u.Email == "test@example.com");
            Assert.NotNull(savedUser);
            Assert.Equal("Test", savedUser.FirstName);
            Assert.Equal("User", savedUser.LastName);
        }

        [Fact]
        public async Task CanAddAndRetrieveDisasterIncident()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var incident = new DisasterIncident
            {
                Title = "Test Disaster",
                Description = "Test Description",
                Location = "Test Location",
                DisasterType = "Earthquake",
                OccurredOn = DateTime.Now,
                SeverityLevel = "",
                PeopleAffected = 100,
                ReportedByUserId = 1,
                Status = "Reported"
            };

            // Act
            context.DisasterIncidents.Add(incident);
            await context.SaveChangesAsync();

            // Assert
            var savedIncident = await context.DisasterIncidents.FirstOrDefaultAsync();
            Assert.NotNull(savedIncident);
            Assert.Equal("Test Disaster", savedIncident.Title);
            Assert.Equal("Reported", savedIncident.Status);
        }

        [Fact]
        public async Task CanAddAndRetrieveDonation()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var donation = new Donation
            {
                Type = "Money",
                Description = "Test donation",
                Quantity = 100,
                Unit = "USD",
                DonorUserId = 1,
                Status = "Pending"
            };

            // Act
            context.Donations.Add(donation);
            await context.SaveChangesAsync();

            // Assert
            var savedDonation = await context.Donations.FirstOrDefaultAsync();
            Assert.NotNull(savedDonation);
            Assert.Equal("Money", savedDonation.Type);
            Assert.Equal("Pending", savedDonation.Status);
        }

        [Fact]
        public async Task CanAddAndRetrieveVolunteer()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var volunteer = new Volunteer
            {
                UserId = 1,
                Availability = "Weekends",
                Location = "Test Location",
                HasTransportation = true,
                Status = "Pending"
            };

            // Act
            context.Volunteers.Add(volunteer);
            await context.SaveChangesAsync();

            // Assert
            var savedVolunteer = await context.Volunteers.FirstOrDefaultAsync();
            Assert.NotNull(savedVolunteer);
            Assert.Equal("Weekends", savedVolunteer.Availability);
            Assert.True(savedVolunteer.HasTransportation);
        }

        [Fact]
        public async Task SkillsAreSeeded()
        {
            // Arrange
            using var context = CreateInMemoryContext();

            // Act
            var skills = await context.Skills.ToListAsync();

            // Assert
            Assert.NotNull(skills);
            Assert.NotEmpty(skills);
            Assert.Contains(skills, s => s.Name == "First Aid/CPR");
            Assert.Contains(skills, s => s.Name == "Medical Assistance");
            Assert.Contains(skills, s => s.Name == "Construction");
        }

        [Fact]
        public async Task UserVolunteerRelationship_WorksCorrectly()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var user = new User
            {
                FirstName = "Test",
                LastName = "User",
                Email = "test@example.com",
                PasswordHash = "hashed_password",
                Role = "User"
            };

            var volunteer = new Volunteer
            {
                User = user,
                Availability = "Weekends",
                Location = "Test Location",
                HasTransportation = true,
                Status = "Pending"
            };

            // Act
            context.Users.Add(user);
            context.Volunteers.Add(volunteer);
            await context.SaveChangesAsync();

            // Assert
            var savedVolunteer = await context.Volunteers
                .Include(v => v.User)
                .FirstOrDefaultAsync(v => v.UserId == user.Id);

            Assert.NotNull(savedVolunteer);
            Assert.NotNull(savedVolunteer.User);
            Assert.Equal(user.Email, savedVolunteer.User.Email);
        }

        [Fact]
        public async Task UserDisasterIncidentRelationship_WorksCorrectly()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var user = new User
            {
                FirstName = "Test",
                LastName = "User",
                Email = "test@example.com",
                PasswordHash = "hashed_password",
                Role = "User"
            };

            var incident = new DisasterIncident
            {
                Title = "Test Disaster",
                Description = "Test Description",
                Location = "Test Location",
                DisasterType = "Earthquake",
                OccurredOn = DateTime.Now,
                SeverityLevel = "",
                PeopleAffected = 100,
                ReportedByUser = user
            };

            // Act
            context.Users.Add(user);
            context.DisasterIncidents.Add(incident);
            await context.SaveChangesAsync();

            // Assert
            var savedIncident = await context.DisasterIncidents
                .Include(i => i.ReportedByUser)
                .FirstOrDefaultAsync(i => i.ReportedByUserId == user.Id);

            Assert.NotNull(savedIncident);
            Assert.NotNull(savedIncident.ReportedByUser);
            Assert.Equal(user.Email, savedIncident.ReportedByUser.Email);
        }

        [Fact]
        public async Task VolunteerSkillManyToManyRelationship_WorksCorrectly()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var volunteer = new Volunteer
            {
                UserId = 1,
                Availability = "Weekends",
                Location = "Test Location",
                HasTransportation = true
            };

            var skill = await context.Skills.FirstAsync(s => s.Name == "First Aid/CPR");
            var volunteerSkill = new VolunteerSkill
            {
                Volunteer = volunteer,
                Skill = skill
            };

            // Act
            context.Volunteers.Add(volunteer);
            context.VolunteerSkills.Add(volunteerSkill);
            await context.SaveChangesAsync();

            // Assert
            var savedVolunteerSkill = await context.VolunteerSkills
                .Include(vs => vs.Volunteer)
                .Include(vs => vs.Skill)
                .FirstOrDefaultAsync(vs => vs.VolunteerId == volunteer.Id && vs.SkillId == skill.Id);

            Assert.NotNull(savedVolunteerSkill);
            Assert.NotNull(savedVolunteerSkill.Volunteer);
            Assert.NotNull(savedVolunteerSkill.Skill);
            Assert.Equal("First Aid/CPR", savedVolunteerSkill.Skill.Name);
        }
    }
}
