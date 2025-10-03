using DisasterAlleviationFoundationn.Controllers;
using DisasterAlleviationFoundationn.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace DisasterAlleviationFoundationn.Tests.Controllers
{
    public class VolunteerControllerTests
    {
        private readonly VolunteerController _controller;

        public VolunteerControllerTests()
        {
            _controller = new VolunteerController();
        }

        [Fact]
        public void Opportunities_Get_ReturnsView()
        {
            // Act
            var result = _controller.Opportunities();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Opportunities_Post_ValidModel_RedirectsToDashboard()
        {
            // Arrange
            var model = new VolunteerViewModel
            {
                FullName = "Test User",
                Email = "test@test.com",
                Phone = "1234567890",
                Skills = "First Aid"
            };

            // Act
            var result = _controller.Opportunities(model);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Dashboard", redirectResult.ActionName);
            Assert.Equal("Home", redirectResult.ControllerName);
        }

        [Fact]
        public void Opportunities_Post_InvalidModel_ReturnsViewWithModel()
        {
            // Arrange
            var model = new VolunteerViewModel();
            _controller.ModelState.AddModelError("FullName", "Full name is required");

            // Act
            var result = _controller.Opportunities(model);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(model, viewResult.Model);
        }

        [Fact]
        public void Controller_HasAuthorizeAttribute()
        {
            // Arrange & Act
            var attributes = _controller.GetType().GetCustomAttributes(typeof(AuthorizeAttribute), true);

            // Assert
            Assert.NotEmpty(attributes);
        }
    }
}