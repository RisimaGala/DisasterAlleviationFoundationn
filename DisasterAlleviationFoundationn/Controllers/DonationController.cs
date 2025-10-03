using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DisasterAlleviationFoundationn.ViewModels;

namespace DisasterAlleviationFoundationn.Controllers
{
    [Authorize]
    public class DonationController : Controller
    {
        [HttpGet]
        public IActionResult Donate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Donate(DonationViewModel model)
        {
            if (ModelState.IsValid)
            {
                TempData["SuccessMessage"] = "Thank you for your donation! We'll contact you shortly.";
                return RedirectToAction("Dashboard", "Home");
            }
            return View(model);
        }
    }
}