using DisasterAlleviationFoundationn.Controllers;
using DisasterAlleviationFoundationn.Data;
using DisasterAlleviationFoundationn.Models;
using DisasterAlleviationFoundationn.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Security.Claims;
using Xunit;

namespace DisasterAlleviationFoundationn.Tests.Controllers
{
    public class ProfileControllerTests
    {
        private readonly Mock<ApplicationDbContext> _mockContext;
        private readonly Mock<IWebHostEnvironment> _mockEnvironment;
        private readonly ProfileController _controller;

        public ProfileControllerTests()
        {
            _mockContext = new Mock<ApplicationDbContext>();
            _mockEnvironment = new Mock<IWebHostEnvironment>();
            _controller = new ProfileController(_mockContext.Object, _mockEnvironment.Object);

            // Setup user identity
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Email, "test@test.com")
            };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = claimsPrincipal }
            };
        }

        [Fact]
        public async Task Index_Get_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            var mockSet = new Mock<DbSet<User>>();
            _mockContext.Setup(m => m.Users).Returns(mockSet.Object);

            // Act
            var result = await _controller.Index();

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void ChangePassword_Get_ReturnsView()
        {
            // Act
            var result = _controller.ChangePassword();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Activity_Get_ReturnsView()
        {
            // Act
            var result = _controller.Activity();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public void DeleteAccount_Get_ReturnsView()
        {
            // Act
            var result = _controller.DeleteAccount();

            // Assert
            Assert.IsType<ViewResult>(result);
        }
    }
}