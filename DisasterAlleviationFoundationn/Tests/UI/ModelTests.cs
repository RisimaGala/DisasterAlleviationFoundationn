using Xunit;
using System.ComponentModel.DataAnnotations;
using DisasterAlleviationFoundationn.Models;

namespace DisasterAlleviationFoundationn.Tests.Models
{
    public class ModelTests
    {
        [Fact]
        public void User_RequiredFields_Validation()
        {
            // Arrange
            var user = new User
            {
                FirstName = "", // This should fail validation
                LastName = "Doe",
                Email = "invalid-email", // This should fail validation
                PasswordHash = "hash"
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(user, new ValidationContext(user), validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(validationResults, v => v.MemberNames.Contains("FirstName"));
            Assert.Contains(validationResults, v => v.MemberNames.Contains("Email"));
        }

        [Fact]
        public void DisasterIncident_ValidModel_PassesValidation()
        {
            // Arrange
            var incident = new DisasterIncident
            {
                Title = "Valid Title",
                Description = "Valid Description",
                Location = "Valid Location",
                DisasterType = "Earthquake",
                SeverityLevel = "3",
                PeopleAffected = 100,
                ReportedByUserId = 1
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(incident, new ValidationContext(incident), validationResults, true);

            // Assert
            Assert.True(isValid);
            Assert.Empty(validationResults);
        }
    }
}