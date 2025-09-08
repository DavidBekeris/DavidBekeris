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
                // Return the view with validation errors
                return View(model);
            }

            try
            {
                // 1. Get AWS keys from Secrets Manager (optional for IAM role)
                var secretsHelper = new AwsSecretsHelper("eu-central-1");
                var (accessKey, secretKey) = await secretsHelper.GetSesKeysAsync();

                // 2. Initialize EmailService (keys optional if running with IAM role)
                var emailService = new EmailService(accessKey, secretKey, "eu-central-1");

                // 3. Send the email
                await emailService.SendEmailAsync(
                    from: "noreply@davidbekeris.se",        // Verified SES sender
                    to: "david-_-bkrs@hotmail.com",        // Your inbox
                    subject: model.Subject,
                    body: $"Message from {model.Email}:\n\n{model.Message}",
                    replyTo: model.Email                     // User's email for reply
                );

                // 4. Set a flag to display a success message on the view
                ViewBag.EmailSent = true;

                // 5. Return a fresh form (or you could return model if you prefer to keep data)
                return View(new ContactFormModel());
            }
            catch (Exception ex)
            {
                // Log the full error (useful in production logs or CloudWatch)
                Console.WriteLine("SES Error: " + ex.ToString());

                // Add a user-friendly error message for the form
                ModelState.AddModelError("", "Oops! Something went wrong sending your message.");
                return View(model);
            }
        }
    } 
}
