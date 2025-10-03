using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using DisasterAlleviationFoundationn.Data;
using DisasterAlleviationFoundationn.Models;
using DisasterAlleviationFoundationn.ViewModels;

namespace DisasterAlleviationFoundationn.Controllers
{
    public class IncidentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public IncidentController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [HttpGet]
        public IActionResult Report()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Report(IncidentReportViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                    if (string.IsNullOrEmpty(userId))
                    {
                        TempData["ErrorMessage"] = "You must be logged in to report an incident.";
                        return RedirectToAction("Login", "Auth");
                    }

                    // Uses nullable string and proper null checking
                    string? imagePath = null;
                    if (model.Image != null && model.Image.Length > 0)
                    {
                        imagePath = await SaveImage(model.Image);
                    }

                    // Uses safe parsing for userId
                    if (!int.TryParse(userId, out int reportedByUserId))
                    {
                        TempData["ErrorMessage"] = "Invalid user information. Please login again.";
                        return RedirectToAction("Login", "Auth");
                    }

                    var incident = new DisasterIncident
                    {
                        Title = model.Title,
                        Description = model.Description,
                        Location = model.Location,
                        DisasterType = model.DisasterType,
                        OccurredOn = model.OccurredOn, // Fixed property name
                        ReportedOn = DateTime.Now,
                        SeverityLevel = model.SeverityLevel,
                        PeopleAffected = model.PeopleAffected, // Fixed property name
                        Status = "Reported",
                        ImageUrl = imagePath,
                        ReportedByUserId = reportedByUserId
                    };

                    _context.DisasterIncidents.Add(incident);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Incident reported successfully! Our team will review it shortly.";
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Error reporting incident. Please try again.";
                }
            }

            return View(model);
        }

        private async Task<string> SaveImage(IFormFile image)
        {
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "incidents");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            return $"/uploads/incidents/{uniqueFileName}";
        }
    }
}