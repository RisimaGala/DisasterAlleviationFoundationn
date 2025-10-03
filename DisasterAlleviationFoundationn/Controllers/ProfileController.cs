using BCrypt.Net;
using DisasterAlleviationFoundationn.Data;
using DisasterAlleviationFoundationn.Models;
using DisasterAlleviationFoundationn.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DisasterAlleviationFoundationn.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ProfileController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: /Profile
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.ReportedIncidents)
                .Include(u => u.Donations)
                .Include(u => u.VolunteerApplications)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound();
            }

            var model = new ProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber ?? string.Empty,
                CurrentProfileImage = user.ProfileImage ?? string.Empty,
                Location = user.Location ?? string.Empty,
                MemberSince = user.CreatedAt,
                TotalIncidentsReported = user.ReportedIncidents?.Count ?? 0,
                TotalDonationsMade = user.Donations?.Count ?? 0,
                TotalVolunteerApplications = user.VolunteerApplications?.Count ?? 0,
                NewLocation = user.Location ?? string.Empty
            };

            return View(model);
        }

        // POST: /Profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
                    {
                        return NotFound();
                    }

                    var user = await _context.Users.FindAsync(userId);

                    if (user == null)
                    {
                        return NotFound();
                    }

                    // Checks if email is already taken by another user
                    if (await _context.Users.AnyAsync(u => u.Email == model.Email && u.Id != userId))
                    {
                        ModelState.AddModelError("Email", "This email is already registered.");
                        return View(model);
                    }

                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Email = model.Email;
                    user.PhoneNumber = model.PhoneNumber;
                    user.Location = model.NewLocation;

                    // Handle profile image upload
                    if (model.ProfileImage != null && model.ProfileImage.Length > 0)
                    {
                        // Deletes old profile image if exists
                        if (!string.IsNullOrEmpty(user.ProfileImage))
                        {
                            await DeleteProfileImage(user.ProfileImage);
                        }
                        user.ProfileImage = await SaveProfileImage(model.ProfileImage);
                    }

                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Profile updated successfully!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Log the actual exception
                    System.Diagnostics.Debug.WriteLine($"Error updating profile: {ex.Message}");
                    ModelState.AddModelError(string.Empty, "An error occurred while updating your profile. Please try again.");
                }
            }

            // Reload statistics for the view
            await LoadProfileStatistics(model);
            return View(model);
        }

        // GET: /Profile/ChangePassword
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        // POST: /Profile/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
                    {
                        return NotFound();
                    }

                    var user = await _context.Users.FindAsync(userId);

                    if (user == null)
                    {
                        return NotFound();
                    }

                    // Verifies current password
                    if (!BCrypt.Net.BCrypt.Verify(model.CurrentPassword, user.PasswordHash))
                    {
                        ModelState.AddModelError("CurrentPassword", "Current password is incorrect.");
                        return View(model);
                    }

                    // Updates password
                    user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Password changed successfully!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Logs the actual exception
                    System.Diagnostics.Debug.WriteLine($"Error changing password: {ex.Message}");
                    ModelState.AddModelError(string.Empty, "An error occurred while changing your password. Please try again.");
                }
            }

            return View(model);
        }

        // GET: /Profile/Activity
        [HttpGet]
        public async Task<IActionResult> Activity()
        {
            try
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
                {
                    return View(new
                    {
                        RecentIncidents = new List<object>(),
                        RecentDonations = new List<object>(),
                        VolunteerApplications = new List<object>()
                    });
                }

                var userActivity = new
                {
                    RecentIncidents = await _context.DisasterIncidents
                        .Where(i => i.ReportedByUserId == userId)
                        .OrderByDescending(i => i.ReportedOn)
                        .Take(10)
                        .ToListAsync(),
                    RecentDonations = await _context.Donations
                        .Where(d => d.DonorUserId == userId)
                        .OrderByDescending(d => d.DonatedOn)
                        .Take(10)
                        .ToListAsync(),
                    VolunteerApplications = await _context.Volunteers
                        .Where(v => v.UserId == userId)
                        .OrderByDescending(v => v.AppliedOn)
                        .Take(10)
                        .ToListAsync()
                };

                return View(userActivity);
            }
            catch (Exception ex)
            {
                // Logs the actual exception
                System.Diagnostics.Debug.WriteLine($"Error loading activity: {ex.Message}");
                return View(new
                {
                    RecentIncidents = new List<object>(),
                    RecentDonations = new List<object>(),
                    VolunteerApplications = new List<object>()
                });
            }
        }

        // GET: /Profile/DeleteAccount
        [HttpGet]
        public IActionResult DeleteAccount()
        {
            return View();
        }

        // POST: /Profile/DeleteAccount
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAccount(DeleteAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
                    {
                        return NotFound();
                    }

                    var user = await _context.Users.FindAsync(userId);

                    if (user == null)
                    {
                        return NotFound();
                    }

                    // Verifies password
                    if (!BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
                    {
                        ModelState.AddModelError("Password", "Password is incorrect.");
                        return View(model);
                    }

                    // Soft deletes user (mark as inactive)
                    user.IsActive = false;
                    user.Email = $"deleted_{user.Id}_{user.Email}";
                    await _context.SaveChangesAsync();

                    // Signs out the user
                    await HttpContext.SignOutAsync();

                    TempData["SuccessMessage"] = "Your account has been deleted successfully.";
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    // Logs the actual exception
                    System.Diagnostics.Debug.WriteLine($"Error deleting account: {ex.Message}");
                    ModelState.AddModelError(string.Empty, "An error occurred while deleting your account. Please try again.");
                }
            }

            return View(model);
        }

        // POST: /Profile/RemoveProfileImage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveProfileImage()
        {
            try
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
                {
                    return NotFound();
                }

                var user = await _context.Users.FindAsync(userId);

                if (user == null)
                {
                    return NotFound();
                }

                if (!string.IsNullOrEmpty(user.ProfileImage))
                {
                    await DeleteProfileImage(user.ProfileImage);
                    user.ProfileImage = string.Empty;
                    await _context.SaveChangesAsync();
                }

                TempData["SuccessMessage"] = "Profile picture removed successfully!";
            }
            catch (Exception ex)
            {
                
                System.Diagnostics.Debug.WriteLine($"Error removing profile image: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while removing the profile picture.";
            }

            return RedirectToAction("Index");
        }

        private async Task<string> SaveProfileImage(IFormFile image)
        {
            try
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "profiles");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }

                return $"/uploads/profiles/{uniqueFileName}";
            }
            catch (Exception ex)
            {
                // Logs the actual exception
                System.Diagnostics.Debug.WriteLine($"Error saving profile image: {ex.Message}");
                throw new Exception("Failed to save profile image");
            }
        }

        private async Task DeleteProfileImage(string imagePath)
        {
            try
            {
                if (!string.IsNullOrEmpty(imagePath))
                {
                    var fullPath = Path.Combine(_environment.WebRootPath, imagePath.TrimStart('/'));
                    if (System.IO.File.Exists(fullPath))
                    {
                        await Task.Run(() => System.IO.File.Delete(fullPath)); 
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the actual exception
                System.Diagnostics.Debug.WriteLine($"Error deleting profile image: {ex.Message}");
               
            }
        }

        private async Task LoadProfileStatistics(ProfileViewModel model)
        {
            try
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
                {
                    return;
                }

                var user = await _context.Users
                    .Include(u => u.ReportedIncidents)
                    .Include(u => u.Donations)
                    .Include(u => u.VolunteerApplications)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user != null)
                {
                    model.TotalIncidentsReported = user.ReportedIncidents?.Count ?? 0;
                    model.TotalDonationsMade = user.Donations?.Count ?? 0;
                    model.TotalVolunteerApplications = user.VolunteerApplications?.Count ?? 0;
                    model.MemberSince = user.CreatedAt;
                }
            }
            catch (Exception ex)
            {
                
                System.Diagnostics.Debug.WriteLine($"Error loading profile statistics: {ex.Message}");
                
                model.TotalIncidentsReported = 0;
                model.TotalDonationsMade = 0;
                model.TotalVolunteerApplications = 0;
            }
        }
    }
}