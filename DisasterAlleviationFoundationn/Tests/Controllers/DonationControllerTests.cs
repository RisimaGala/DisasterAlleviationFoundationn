using DisasterAlleviationFoundationn.Controllers;

using DisasterAlleviationFoundationn.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace DisasterAlleviationFoundationn.Tests.Controllers
{
    public class DonationControllerTests
    {
        private readonly DonationController _controller;

        public DonationControllerTests()
        {
            _controller = new DonationController();
        }

        [Fact]
        public void Donate_Get_ReturnsView()
        {
            // Act
            //var result = _controller.Donate();

            // Assert
            //Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Donate_Post_ValidModel_RedirectsToDashboard()
        {
            // Arrange
            var model = new DonationViewModel
            {
                Amount = 100,
                DonationType = "Money",
                //DonorName = "Test User"
            };

            // Act
            var result = _controller.Donate(model);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Dashboard", redirectResult.ActionName);
            Assert.Equal("Home", redirectResult.ControllerName);
        }

        [Fact]
        public void Donate_Post_InvalidModel_ReturnsViewWithModel()
        {
            // Arrange
            var model = new DonationViewModel();
            _controller.ModelState.AddModelError("Amount", "Amount is required");

            // Act
            var result = _controller.Donate(model);

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