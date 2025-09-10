using DavidBekeris.Models;
using DavidBekeris.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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

        [HttpPost("Kontakt")]
        public async Task<IActionResult> Kontakt(ContactFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // No keys fetched anymore — IAM role is used
                var emailService = new EmailService("eu-central-1");

                await emailService.SendEmailAsync(
                    from: "noreply@davidbekeris.se",
                    to: "davidbekeris@hotmail.com",
                    subject: model.Subject,
                    body: $"Message from {model.Email}:\n\n{model.Message}",
                    replyTo: model.Email
                );

                ViewBag.EmailSent = true;
                return View(new ContactFormModel());
            }
            catch (Exception ex)
            {
                Console.WriteLine("SES Error: " + ex.ToString());
                ModelState.AddModelError("", "Oops! Something went wrong sending your message.");
                return View(model);
            }
        }
    } 
}
