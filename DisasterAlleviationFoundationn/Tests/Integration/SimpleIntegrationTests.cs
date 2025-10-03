using Xunit;
using Microsoft.EntityFrameworkCore;
using DisasterAlleviationFoundationn.Data;
using DisasterAlleviationFoundationn.Models;
using System.Threading.Tasks;
using System.Linq;

namespace DisasterAlleviationFoundationn.Tests.Integration
{
    public class SimpleIntegrationTests
    {
        private ApplicationDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + System.Guid.NewGuid())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task Database_CanConnect_Success()
        {
            // Arrange
            using var context = CreateInMemoryContext();

            // Act & Assert
            Assert.True(await context.Database.CanConnectAsync());
        }

        [Fact]
        public async Task User_CRUD_Operations_Work()
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

            // Act - Create
            context.Users.Add(user);
            var saveResult = await context.SaveChangesAsync();

            // Assert - Create
            Assert.True(saveResult > 0);
            Assert.True(user.Id > 0);

            // Act - Read
            var retrievedUser = await context.Users.FindAsync(user.Id);

            // Assert - Read
            Assert.NotNull(retrievedUser);
            Assert.Equal("Test", retrievedUser.FirstName);
            Assert.Equal("test@example.com", retrievedUser.Email);
        }

        [Fact]
        public async Task DisasterIncident_CRUD_Operations_Work()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var incident = new DisasterIncident
            {
                Title = "Test Disaster",
                Description = "Test Description",
                Location = "Test Location",
                DisasterType = "Earthquake",
                SeverityLevel = "",
                PeopleAffected = 100,
                ReportedByUserId = 1
            };

            // Act - Create
            context.DisasterIncidents.Add(incident);
            var saveResult = await context.SaveChangesAsync();

            // Assert - Create
            Assert.True(saveResult > 0);
            Assert.True(incident.Id > 0);

            // Act - Read
            var retrievedIncident = await context.DisasterIncidents.FindAsync(incident.Id);

            // Assert - Read
            Assert.NotNull(retrievedIncident);
            Assert.Equal("Test Disaster", retrievedIncident.Title);
            Assert.Equal("Earthquake", retrievedIncident.DisasterType);
        }

        [Fact]
        public async Task MultipleEntities_CanBeQueriesTogether()
        {
            // Arrange
            using var context = CreateInMemoryContext();

            // Add a user
            var user = new User
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@test.com",
                PasswordHash = "hash",
                Role = "User"
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Add an incident for that user
            var incident = new DisasterIncident
            {
                Title = "User's Incident",
                Description = "Description",
                Location = "Location",
                DisasterType = "Flood",
                SeverityLevel = "2",
                PeopleAffected = 50,
                ReportedByUserId = user.Id
            };
            context.DisasterIncidents.Add(incident);
            await context.SaveChangesAsync();

            // Act - Query both entities
            var userCount = await context.Users.CountAsync();
            var incidentCount = await context.DisasterIncidents.CountAsync();

            // Assert
            Assert.Equal(1, userCount);
            Assert.Equal(1, incidentCount);
        }

        [Fact]
        public async Task Skills_AreSeededCorrectly()
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
        }
    }
}