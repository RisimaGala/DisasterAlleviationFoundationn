using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DisasterAlleviationFoundationn.ViewModels;

namespace DisasterAlleviationFoundationn.Controllers
{
    [Authorize]
    public class VolunteerController : Controller
    {
        [HttpGet]
        public IActionResult Opportunities()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Opportunities(VolunteerViewModel model)
        {
            if (ModelState.IsValid)
            {
                TempData["SuccessMessage"] = "Thank you for your interest in volunteering! We'll contact you shortly.";
                return RedirectToAction("Dashboard", "Home");
            }
            return View(model);
        }
    }
}