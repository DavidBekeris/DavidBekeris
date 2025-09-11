using System.Text.Json;

namespace DavidBekeris.Services
{
    public class GoogleRecaptchaService
    {
        private readonly GoogleRecaptchaHelper _helper;

        public GoogleRecaptchaService(GoogleRecaptchaHelper helper)
        {
            _helper = helper;
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            var secretKey = await _helper.GetSecretKeyAsync();

            using var client = new HttpClient();
            var response = await client.PostAsync(
                "https://www.google.com/recaptcha/api/siteverify",
                new FormUrlEncodedContent(new Dictionary<string, string>
                {
                { "secret", secretKey },
                { "response", token }
                })
            );

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            // success is a boolean, so extract properly
            return doc.RootElement.GetProperty("success").GetBoolean();
        }
    }
}
