using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using DisasterAlleviationFoundationn.Models;
using DisasterAlleviationFoundationn.Data;
using DisasterAlleviationFoundationn.ViewModels;

namespace DisasterAlleviationFoundationn.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Dashboard()
        {
            try
            {
                // FIXED: Safe user ID parsing with proper error handling
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
                {
                    TempData["ErrorMessage"] = "Unable to identify user. Please login again.";
                    return RedirectToAction("Login", "Auth");
                }

                var dashboardStats = new DashboardViewModel
                {
                    TotalIncidents = await _context.DisasterIncidents.CountAsync(),
                    TotalVolunteers = await _context.Volunteers.CountAsync(),
                    TotalDonations = await _context.Donations.CountAsync(),
                    UserIncidents = await _context.DisasterIncidents
                        .Where(i => i.ReportedByUserId == userId)
                        .CountAsync(),
                    RecentIncidents = await _context.DisasterIncidents
                        .Include(i => i.ReportedByUser)
                        .OrderByDescending(i => i.ReportedOn)
                        .Take(5)
                        .ToListAsync(),

                    // FIXED: Use IsActive instead of IsEmailConfirmed
                    ActiveUsers = await _context.Users.CountAsync(u => u.IsActive),

                    // FIXED: Check if these properties exist in your models
                    // If Donations doesn't have Status, remove this line
                    // PendingDonations = await _context.Donations.CountAsync(d => d.Status == "Pending"),

                    // If DisasterIncidents doesn't have Status, remove this line
                    // ResolvedIncidents = await _context.DisasterIncidents.CountAsync(i => i.Status == "Resolved")
                };

                return View(dashboardStats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading dashboard");

                // Return a safe default view with basic stats
                return View(new DashboardViewModel
                {
                    TotalIncidents = 0,
                    TotalVolunteers = 0,
                    TotalDonations = 0,
                    UserIncidents = 0,
                    RecentIncidents = new List<DisasterIncident>(),
                    ActiveUsers = 0
                    // Remove the properties that don't exist in your model
                });
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}