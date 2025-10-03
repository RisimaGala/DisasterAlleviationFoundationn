using DisasterAlleviationFoundationn.Controllers;
using DisasterAlleviationFoundationn.Data;
using DisasterAlleviationFoundationn.Models;
using DisasterAlleviationFoundationn.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using Xunit;

namespace DisasterAlleviationFoundationn.Tests.Controllers
{
    public class IncidentControllerTests
    {
        private readonly Mock<ApplicationDbContext> _mockContext;
        private readonly Mock<IWebHostEnvironment> _mockEnvironment;
        private readonly IncidentController _controller;

        public IncidentControllerTests()
        {
            _mockContext = new Mock<ApplicationDbContext>();
            _mockEnvironment = new Mock<IWebHostEnvironment>();
            _controller = new IncidentController(_mockContext.Object, _mockEnvironment.Object);

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
        public void Report_Get_ReturnsView()
        {
            // Act
            var result = _controller.Report();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Report_Post_InvalidModel_ReturnsViewWithModel()
        {
            // Arrange
            var model = new IncidentReportViewModel();
            _controller.ModelState.AddModelError("Title", "Title is required");

            // Act
            var result = await _controller.Report(model);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(model, viewResult.Model);
        }

        [Fact]
        public async Task Report_Post_UserNotAuthenticated_RedirectsToLogin()
        {
            // Arrange
            var controller = new IncidentController(_mockContext.Object, _mockEnvironment.Object);
            var model = new IncidentReportViewModel
            {
                Title = "Test Incident",
                Description = "Test Description",
                Location = "Test Location",
                DisasterType = "Earthquake"
            };

            // Act
            var result = await controller.Report(model);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Login", redirectResult.ActionName);
            Assert.Equal("Auth", redirectResult.ControllerName);
        }
    }
}