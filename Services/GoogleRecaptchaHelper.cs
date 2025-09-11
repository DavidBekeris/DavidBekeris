using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using System.Text.Json;

namespace DavidBekeris.Services
{
    public class GoogleRecaptchaHelper
    {
        private readonly IAmazonSecretsManager _secretsManager;

        public GoogleRecaptchaHelper(string region)
        {
            _secretsManager = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(region));
        }

        public async Task<string> GetSecretKeyAsync()
        {
            var request = new GetSecretValueRequest
            {
                SecretId = "GoogleReCaptcha-Secret"
            };

            var response = await _secretsManager.GetSecretValueAsync(request);
            using var doc = JsonDocument.Parse(response.SecretString);
            return doc.RootElement.GetProperty("RECAPTCHA_SECRET").GetString();
        }
    }
}
