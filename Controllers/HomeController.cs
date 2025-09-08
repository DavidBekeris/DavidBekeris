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

        [HttpPost("Kontakt")]
        public async Task<IActionResult> Kontakt(ContactFormModel model)
        {
            if (!ModelState.IsValid)
            {
                // Return the view with validation errors
                return View(model);
            }

            try
            {
                // 1. Get AWS keys from Secrets Manager
                var secretsHelper = new AwsSecretsHelper("eu-central-1");
                var (accessKey, secretKey) = await secretsHelper.GetSesKeysAsync();

                // 2. Use EmailService with those keys
                var emailService = new EmailService(accessKey, secretKey, "eu-central-1");

                await emailService.SendEmailAsync(
                    from: "noreply@davidbekeris.se",       // your verified sender
                    to: "david-_-bkrs@hotmail.com",       // your inbox
                    subject: model.Subject,
                    body: $"Message from {model.Email}:\n\n{model.Message}",
                    replyTo: model.Email
                );

                // 3. Set a flag to display success message
                ViewBag.EmailSent = true;

                // Return the same view with empty form or original model
                return View(new ContactFormModel());
            }
            catch (Exception ex)
            {
                // Log the error if you want
                ModelState.AddModelError("", "Oops! Something went wrong sending your message.");
                return View(model);
            }
        }

        public IActionResult ThankYou()
        {
            return View("Views/Home/Kontakt.cshtml");
        }
    }
}
