using Microsoft.AspNetCore.Mvc;
using DisasterAlleviationFoundationn.ViewModels;

namespace DisasterAlleviationFoundationn.Controllers
{
    public class DonationController : Controller
    {
        [HttpGet]
        public IActionResult Donate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Donate(DonationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Process donation logic here
            return RedirectToAction("Dashboard", "Home");
        }
    }
}