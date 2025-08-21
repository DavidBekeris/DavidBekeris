using DavidBekeris.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using DavidBekeris.Services;

namespace DavidBekeris.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Projekt")]
        public IActionResult Projekt()
        {
            return View();
        }

        [HttpGet("Kontakt")]
        public IActionResult Kontakt()
        {
            return View();
        }

        [HttpGet("CV")]
        public IActionResult CV()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> Contact(ContactFormModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var emailService = new EmailService(
                Environment.GetEnvironmentVariable("REMOVED"),
                Environment.GetEnvironmentVariable("REMOVED"),
                "eu-central-1" // or your SES region
            );

            await emailService.SendEmailAsync(
                model.Email,               // user’s email
                "david-_-bkrs@hotmail.com",  // where you want to receive it
                model.Subject,
                model.Message
            );

            return RedirectToAction("ThankYou");
        }
    }
}
