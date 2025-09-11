namespace DavidBekeris.Services
{
    using Amazon.SecretsManager;
    using Amazon.SecretsManager.Model;
    using Amazon;

    public class AwsSecretsHelper
    {
        private readonly IAmazonSecretsManager _client;

        public AwsSecretsHelper(string region)
        {
            _client = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(region));
        }

        // 🔹 Fetch arbitrary secret string (e.g. Google reCAPTCHA key)
        public async Task<string> GetSecretValueAsync(string secretName)
        {
            try
            {
                var request = new GetSecretValueRequest
                {
                    SecretId = secretName
                };

                var response = await _client.GetSecretValueAsync(request);

                if (!string.IsNullOrEmpty(response.SecretString))
                {
                    return response.SecretString;
                }

                throw new Exception($"Secret {secretName} is empty.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching secret {secretName}: {ex.Message}");
                throw;
            }
        }
    }
}
