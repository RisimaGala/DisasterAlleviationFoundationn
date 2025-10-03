using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using DisasterAlleviationFoundationn.Controllers;
using DisasterAlleviationFoundationn.Models;
using DisasterAlleviationFoundationn.ViewModels;

namespace DisasterAlleviationFoundationn.Tests.Controllers
{
    public class UserTests
    {
        [Fact]
        public void User_ValidCreation_Success()
        {
            // Arrange & Act
            var user = new User
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@test.com",
                PasswordHash = "hashed_password",
                Role = "User"
            };

            // Assert
            Assert.Equal("John", user.FirstName);
            Assert.Equal("Doe", user.LastName);
            Assert.Equal("john.doe@test.com", user.Email);
            Assert.Equal("User", user.Role);
        }

        [Fact]
        public void User_DefaultValues_AreSetCorrectly()
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