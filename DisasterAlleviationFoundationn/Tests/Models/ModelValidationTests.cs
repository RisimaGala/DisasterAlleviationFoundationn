using DisasterAlleviationFoundationn.Data;
using DisasterAlleviationFoundationn.Models;
using DisasterAlleviationFoundationn.Models.DisasterAlleviationFoundationn.Models;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace DisasterAlleviationFoundationn.Tests.Models
{
    public class ModelValidationTests
    {
        [Fact]
        public void DisasterIncident_ValidModel_PassesValidation()
        {
            // Arrange
            var incident = new DisasterIncident
            {
                Title = "Test Disaster",
                Description = "Test Description",
                Location = "Test Location",
                DisasterType = "Earthquake",
                OccurredOn = DateTime.Now,
                SeverityLevel = "3",
                PeopleAffected = 100,
                ReportedByUserId = 1
            };

            // Act
            var validationResults = ValidateModel(incident);

            // Assert
            Assert.Empty(validationResults);
        }

        [Theory]
        [InlineData("", "Description", "Location", "DisasterType")] // Empty title
        [InlineData("Title", "", "Location", "DisasterType")] // Empty description
        [InlineData("Title", "Description", "", "DisasterType")] // Empty location
        [InlineData("Title", "Description", "Location", "")] // Empty disaster type
        public void DisasterIncident_InvalidRequiredFields_FailsValidation(
            string title, string description, string location, string disasterType)
        {
            // Arrange
            var incident = new DisasterIncident
            {
                Title = title,
                Description = description,
                Location = location,
                DisasterType = disasterType,
                OccurredOn = DateTime.Now,
                SeverityLevel = "1",
                PeopleAffected = 10,
                ReportedByUserId = 1
            };

            // Act
            var validationResults = ValidateModel(incident);

            // Assert
            Assert.NotEmpty(validationResults);
        }

        [Fact]
        public void User_ValidModel_PassesValidation()
        {
            // Arrange
            var user = new User
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PasswordHash = "hashed_password",
                Role = "User"
            };

            // Act
            var validationResults = ValidateModel(user);

            // Assert
            Assert.Empty(validationResults);
        }

        [Theory]
        [InlineData("", "Doe", "john.doe@example.com")] // Empty first name
        [InlineData("John", "", "john.doe@example.com")] // Empty last name
        [InlineData("John", "Doe", "invalid-email")] // Invalid email
        public void User_InvalidFields_FailsValidation(string firstName, string lastName, string email)
        {
            // Arrange
            var user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PasswordHash = "hashed_password",
                Role = "User"
            };

            // Act
            var validationResults = ValidateModel(user);

            // Assert
            Assert.NotEmpty(validationResults);
        }

        [Fact]
        public void Donation_ValidModel_PassesValidation()
        {
            // Arrange
            var donation = new Donation
            {
                Type = "Money",
                Description = "Test donation",
                Quantity = 100,
                Unit = "USD",
                DonorUserId = 1
            };

            // Act
            var validationResults = ValidateModel(donation);

            // Assert
            Assert.Empty(validationResults);
        }

        [Fact]
        public void Volunteer_ValidModel_PassesValidation()
        {
            // Arrange
            var volunteer = new Volunteer
            {
                UserId = 1,
                Availability = "Weekends",
                Location = "Test Location",
                HasTransportation = true
            };

            // Act
            var validationResults = ValidateModel(volunteer);

            // Assert
            Assert.Empty(validationResults);
        }

        [Fact]
        public void Skill_ValidModel_PassesValidation()
        {
            // Arrange
            var skill = new Skill
            {
                Name = "Test Skill",
                Description = "Test Description"
            };

            // Act
            var validationResults = ValidateModel(skill);

            // Assert
            Assert.Empty(validationResults);
        }

        [Theory]
        [InlineData("")] // Empty name
        [InlineData(null)] // Null name
        [InlineData("This is a very long skill name that exceeds the maximum allowed length of 100 characters and should fail validation")] // Too long
        public void Skill_InvalidName_FailsValidation(string name)
        {
            // Arrange
            var skill = new Skill
            {
                Name = name,
                Description = "Test Description"
            };

            // Act
            var validationResults = ValidateModel(skill);

            // Assert
            Assert.NotEmpty(validationResults);
        }

        [Fact]
        public void VolunteerSkill_ValidModel_PassesValidation()
        {
            // Arrange
            var volunteerSkill = new VolunteerSkill
            {
                VolunteerId = 1,
                SkillId = 1
            };

            // Act
            var validationResults = ValidateModel(volunteerSkill);

            // Assert
            Assert.Empty(validationResults);
        }

        [Fact]
        public void ErrorViewModel_ShowRequestId_ReturnsCorrectValue()
        {
            // Arrange
            var modelWithId = new ErrorViewModel { RequestId = "123" };
            var modelWithoutId = new ErrorViewModel { RequestId = null };

            // Act & Assert
            Assert.True(modelWithId.ShowRequestId);
            Assert.False(modelWithoutId.ShowRequestId);
        }

        private List<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, validationContext, validationResults, true);
            return validationResults;
        }
    }
}
