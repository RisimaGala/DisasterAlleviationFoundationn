using DisasterAlleviationFoundationn.Controllers;
using DisasterAlleviationFoundationn.Data;
using DisasterAlleviationFoundationn.Models;
using DisasterAlleviationFoundationn.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;
using Xunit;

namespace DisasterAlleviationFoundationn.Tests.Controllers
{
    public class HomeControllerTests
    {
        private readonly Mock<ApplicationDbContext> _mockContext;
        private readonly Mock<ILogger<HomeController>> _mockLogger;
        private readonly HomeController _controller;

        public HomeControllerTests()
        {
            _mockContext = new Mock<ApplicationDbContext>();
            _mockLogger = new Mock<ILogger<HomeController>>();
            _controller = new HomeController(_mockLogger.Object, _mockContext.Object);
        }

        [Fact]
        public void Index_ReturnsView()
        {
            // Act
            var result = _controller.Index();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Privacy_ReturnsView()
        {
            // Act
            var result = _controller.Privacy();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Error_ReturnsViewWithModel()
        {
            // Act
            var result = _controller.Error();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<ErrorViewModel>(viewResult.Model);
        }

        [Fact]
        public async Task Dashboard_UserNotAuthenticated_RedirectsToLogin()
        {
            // Arrange
            var controller = new HomeController(_mockLogger.Object, _mockContext.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity())
                    }
                }
            };

            // Act
            var result = await controller.Dashboard();

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Login", redirectResult.ActionName);
            Assert.Equal("Auth", redirectResult.ControllerName);
        }
    }
}