using DisasterAlleviationFoundationn.Controllers;
using DisasterAlleviationFoundationn.Data;
using DisasterAlleviationFoundationn.Models;
using DisasterAlleviationFoundationn.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;
using Xunit;

namespace DisasterAlleviationFoundationn.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<ApplicationDbContext> _mockContext;
        private readonly Mock<ILogger<AuthController>> _mockLogger;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mockContext = new Mock<ApplicationDbContext>();
            _mockLogger = new Mock<ILogger<AuthController>>();
            _controller = new AuthController(_mockContext.Object, _mockLogger.Object);

            // Setup controller context for HttpContext
            var httpContext = new DefaultHttpContext();
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
        }

        [Fact]
        public void Login_Get_ReturnsView()
        {
            // Act
            var result = _controller.Login();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Login_Post_InvalidModel_ReturnsViewWithModel()
        {
            // Arrange
            var model = new LoginViewModel { Email = "test@test.com", Password = "pass" };
            _controller.ModelState.AddModelError("Error", "Model error");

            // Act
            var result = await _controller.Login(model);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(model, viewResult.Model);
        }

        [Fact]
        public async Task Logout_Post_ReturnsRedirectToHome()
        {
            // Arrange
            var mockAuthService = new Mock<IAuthenticationService>();
            mockAuthService
                .Setup(_ => _.SignOutAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<AuthenticationProperties>()))
                .Returns(Task.CompletedTask);

            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider
                .Setup(_ => _.GetService(typeof(IAuthenticationService)))
                .Returns(mockAuthService.Object);

            _controller.ControllerContext.HttpContext.RequestServices = serviceProvider.Object;

            // Act
            var result = await _controller.Logout();

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("Home", redirectResult.ControllerName);
        }

        [Fact]
        public void AccessDenied_Get_ReturnsView()
        {
            // Act
            var result = _controller.AccessDenied();

            // Assert
            Assert.IsType<ViewResult>(result);
        }
    }
}
