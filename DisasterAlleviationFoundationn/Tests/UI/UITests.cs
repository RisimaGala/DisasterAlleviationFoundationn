using Xunit;
using DisasterAlleviationFoundationn.Models;
using DisasterAlleviationFoundationn.ViewModels;

namespace DisasterAlleviationFoundationn.Tests.UI
{
    public class UITests
    {
        [Fact]
        public void LoginPage_ViewModel_HasCorrectProperties()
        {
            // Arrange & Act
            var viewModel = new LoginViewModel
            {
                Email = "test@example.com",
                Password = "password123",
                RememberMe = true
            };

            // Assert
            Assert.Equal("test@example.com", viewModel.Email);
            Assert.Equal("password123", viewModel.Password);
            Assert.True(viewModel.RememberMe);
        }

        [Fact]
        public void RegisterPage_ViewModel_HasCorrectProperties()
        {
            // Arrange & Act
            var viewModel = new RegisterViewModel
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Password = "password123",
                ConfirmPassword = "password123"
            };

            // Assert
            Assert.Equal("John", viewModel.FirstName);
            Assert.Equal("Doe", viewModel.LastName);
            Assert.Equal("john@example.com", viewModel.Email);
        }

        [Fact]
        public void IncidentReport_ViewModel_HasCorrectProperties()
        {
            // Arrange & Act
            var viewModel = new IncidentReportViewModel
            {
                Title = "Test Disaster",
                Description = "Test Description",
                Location = "Test Location",
                DisasterType = "Earthquake",
                SeverityLevel = "3",
                PeopleAffected = 100
            };

            // Assert
            Assert.Equal("Test Disaster", viewModel.Title);
            Assert.Equal("Test Description", viewModel.Description);
            Assert.Equal("Earthquake", viewModel.DisasterType);
            Assert.Equal("3", viewModel.SeverityLevel);
        }

        [Fact]
        public void UserModel_HasCorrectDefaultValues()
        {
            // Arrange & Act
            var user = new User();

            // Assert
            Assert.True(user.IsActive);
            Assert.True(user.IsEmailConfirmed);
            Assert.Equal("User", user.Role);
        }
    }
}