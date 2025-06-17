using Microsoft.AspNetCore.Mvc;

namespace DavidBekeris.Controllers
{
    [Route("download-resume")]
    public class ResumeController : Controller
    {
        
            private static Dictionary<string, List<DateTime>> _ipDownloadLog = new();

            [HttpGet]
            public IActionResult Download()
            {
                var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                // Track downloads per IP
                if (!_ipDownloadLog.ContainsKey(ip))
                    _ipDownloadLog[ip] = new List<DateTime>();

                // Remove old entries > 1 hour
                _ipDownloadLog[ip] = _ipDownloadLog[ip].Where(t => t > DateTime.UtcNow.AddHours(-1)).ToList();

                if (_ipDownloadLog[ip].Count >= 5)
                {
                    return StatusCode(429, "Too many downloads. Please try again later.");
                }

                _ipDownloadLog[ip].Add(DateTime.UtcNow);

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files", "David Bekeris CV - Webbutvecklare.pdf");
                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                return File(fileBytes, "application/pdf", "David Bekeris CV - Webbutvecklare.pdf");
            }
    }
}
