using DisasterAlleviationFoundationn.Data;
using DisasterAlleviationFoundationn.Models;
using DisasterAlleviationFoundationn.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DisasterAlleviationFoundationn.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AuthController> _logger
            ;

        public AuthController(ApplicationDbContext context, ILogger<AuthController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

                    if (user != null && VerifyPassword(model.Password, user.PasswordHash))
                    {
                        if (!user.IsActive)
                        {
                            ModelState.AddModelError(string.Empty, "This account has been deactivated.");
                            return View(model);
                        }

                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                            new Claim(ClaimTypes.Email, user.Email),
                            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                            new Claim(ClaimTypes.Role, user.Role ?? "User")
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = model.RememberMe,
                            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30)
                        };

                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);

                        TempData["SuccessMessage"] = $"Welcome back, {user.FirstName}!";
                        return RedirectToAction("Index", "Home");
                    }

                    ModelState.AddModelError(string.Empty, "Invalid email or password.");
                }
                catch (Exception ex)
                {
                    // Log the actual error for debugging
                    _logger.LogError(ex, "Error during login for email: {Email}", model.Email);
                    ModelState.AddModelError(string.Empty, "An error occurred during login. Please try again.");
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            _logger.LogInformation("Registration attempt for email: {Email}", model.Email);

            if (ModelState.IsValid)
            {
                try
                {
                    _logger.LogInformation("Model state is valid");

                    // Check if email already exists
                    var emailExists = await _context.Users.AnyAsync(u => u.Email == model.Email);
                    _logger.LogInformation("Email exists check: {EmailExists}", emailExists);

                    if (emailExists)
                    {
                        ModelState.AddModelError("Email", "Email is already registered.");
                        return View(model);
                    }

                    // Create new user
                    var user = new User
                    {
                        Email = model.Email.Trim(),
                        PasswordHash = HashPassword(model.Password),
                        FirstName = model.FirstName.Trim(),
                        LastName = model.LastName.Trim(),
                        PhoneNumber = string.IsNullOrEmpty(model.PhoneNumber) ? string.Empty : model.PhoneNumber.Trim(),
                        CreatedAt = DateTime.UtcNow,
                        IsActive = true,
                        IsEmailConfirmed = true,
                        Role = "User"
                    };

                    _logger.LogInformation("User object created, about to save to database");

                    _context.Users.Add(user);
                    var saveResult = await _context.SaveChangesAsync();
                    _logger.LogInformation("Save changes result: {SaveResult}, User ID: {UserId}", saveResult, user.Id);

                    if (saveResult > 0 && user.Id > 0)
                    {
                        _logger.LogInformation("New user registered successfully: {Email} (ID: {UserId})", user.Email, user.Id);
                        TempData["SuccessMessage"] = "Registration successful! Please login with your credentials.";
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        _logger.LogError("Failed to save user to database");
                        ModelState.AddModelError(string.Empty, "Failed to create user account. Please try again.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during registration for email: {Email}", model.Email);
                    ModelState.AddModelError(string.Empty, "An error occurred during registration. Please try again.");
                }
            }
            else
            {
                _logger.LogWarning("Model state invalid. Errors: {Errors}",
                    string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                _logger.LogInformation("User logged out: {Email}", userEmail);
                TempData["SuccessMessage"] = "You have been logged out successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during logout");
                TempData["ErrorMessage"] = "An error occurred during logout.";
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            _logger.LogWarning("Access denied for user: {User} at {Path}", User.Identity?.Name, HttpContext.Request.Path);
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> TestCreateUser()
        {
            try
            {
                var testUser = new User
                {
                    FirstName = "Test",
                    LastName = "User",
                    Email = "test@example.com",
                    PasswordHash = HashPassword("password123"),
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true,
                    IsEmailConfirmed = true,
                    Role = "User"
                };

                _context.Users.Add(testUser);
                await _context.SaveChangesAsync();

                return Content($"Test user created with ID: {testUser.Id}");
            }
            catch (Exception ex)
            {
                return Content($"Error: {ex.Message}");
            }
        }

        // =============================================
        // DEBUG AUTH METHOD ADDED HERE
        // =============================================
        [HttpGet]
        public async Task<IActionResult> DebugAuth()
        {
            var result = new List<string>();

            try
            {
                // 1. Check database connection
                result.Add("1. Testing database connection...");
                var userCount = await _context.Users.CountAsync();
                result.Add($"   ✅ Database connected - {userCount} users found");

                // 2. Check if we can create a test user
                result.Add("2. Testing user creation...");
                var testUser = new User
                {
                    FirstName = "Test",
                    LastName = "User",
                    Email = $"test{Guid.NewGuid()}@test.com",
                    PasswordHash = HashPassword("test123"),
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true,
                    IsEmailConfirmed = true,
                    Role = "User"
                };

                _context.Users.Add(testUser);
                var saveResult = await _context.SaveChangesAsync();
                result.Add($"   ✅ User creation test - Saved: {saveResult}, ID: {testUser.Id}");

                // 3. Test password verification
                result.Add("3. Testing password verification...");
                var verifyResult = VerifyPassword("test123", testUser.PasswordHash);
                result.Add($"   ✅ Password verification: {verifyResult}");

                // 4. Clean up test user
                _context.Users.Remove(testUser);
                await _context.SaveChangesAsync();
                result.Add("   ✅ Test user cleaned up");

                return Content(string.Join("<br/>", result));
            }
            catch (Exception ex)
            {
                result.Add($"❌ ERROR: {ex.Message}");
                result.Add($"Stack: {ex.StackTrace}");
                return Content(string.Join("<br/>", result));
            }
        }

        // =============================================
        // TEST SIMPLE METHOD ADDED RIGHT HERE
        // =============================================
        [HttpGet]
        public async Task<IActionResult> TestSimple()
        {
            try
            {
                var result = new List<string>();

                // Test 1: Basic connection
                result.Add("1. Testing database connection...");
                var canConnect = await _context.Database.CanConnectAsync();
                result.Add($"   Can connect: {canConnect}");

                if (canConnect)
                {
                    // Test 2: Create a simple user directly
                    result.Add("2. Creating test user...");
                    var testUser = new User
                    {
                        FirstName = "Test",
                        LastName = "User",
                        Email = "test@test.com",
                        PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword("password123"),
                        CreatedAt = DateTime.UtcNow,
                        IsActive = true,
                        IsEmailConfirmed = true,
                        Role = "User"
                    };

                    _context.Users.Add(testUser);
                    var saveResult = await _context.SaveChangesAsync();
                    result.Add($"   Save result: {saveResult}, User ID: {testUser.Id}");

                    // Test 3: Try to retrieve the user
                    result.Add("3. Retrieving test user...");
                    var foundUser = await _context.Users.FindAsync(testUser.Id);
                    result.Add($"   User found: {foundUser != null}, Email: {foundUser?.Email}");

                    // Test 4: Verify password
                    result.Add("4. Testing password verification...");
                    var passwordValid = BCrypt.Net.BCrypt.EnhancedVerify("password123", foundUser.PasswordHash);
                    result.Add($"   Password valid: {passwordValid}");

                    // Clean up
                    _context.Users.Remove(testUser);
                    await _context.SaveChangesAsync();
                    result.Add("5. Test user cleaned up");
                }

                return Content(string.Join("<br/>", result));
            }
            catch (Exception ex)
            {
                return Content($"❌ ERROR: {ex.Message}<br/>Stack: {ex.StackTrace}");
            }
        }

        // =============================================
        // UPDATED BCRYPT METHODS WITH BETTER ERROR HANDLING
        // =============================================
        private string HashPassword(string password)
        {
            try
            {
                if (string.IsNullOrEmpty(password))
                {
                    throw new ArgumentException("Password cannot be null or empty");
                }

                // Use enhanced hash for better compatibility
                var hashed = BCrypt.Net.BCrypt.EnhancedHashPassword(password, 13);

                // Verify the hash can be verified
                if (!BCrypt.Net.BCrypt.EnhancedVerify(password, hashed))
                {
                    throw new Exception("Generated hash cannot be verified");
                }

                return hashed;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Password hashing failed");
                throw new Exception("Failed to hash password", ex);
            }
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            try
            {
                if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hashedPassword))
                {
                    _logger.LogWarning("Empty password or hash during verification");
                    return false;
                }

                // Use enhanced verify for compatibility 
                var result = BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);

                if (!result)
                {
                    _logger.LogWarning("Password verification failed");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Password verification error");
                return false;
            }
        }
    }
}