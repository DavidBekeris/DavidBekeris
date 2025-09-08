using Amazon;
using Amazon.Runtime.CredentialManagement;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using System.Text.Json;

namespace DavidBekeris.Services;


public class AwsSecretsHelper
{
    private readonly IAmazonSecretsManager _secretsManager;

    public AwsSecretsHelper(string region)
    {
        // If running in production on AWS with an IAM role, the SDK automatically picks up credentials
        _secretsManager = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(region));
    }

    public async Task<(string accessKey, string secretKey)> GetSesKeysAsync()
    {
        var request = new GetSecretValueRequest
        {
            SecretId = "/SES/Credentials-Main" // name of your secret in Secrets Manager
        };

        var response = await _secretsManager.GetSecretValueAsync(request);

        if (string.IsNullOrEmpty(response.SecretString))
            throw new Exception("Secret string is empty.");

        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(response.SecretString);

        return (dict["AWS_ACCESS_KEY_ID"], dict["AWS_SECRET_ACCESS_KEY"]);
    }
}


public class EmailService
{
    private readonly IAmazonSimpleEmailService _ses;

    public EmailService(string? awsAccessKey, string? awsSecretKey, string region)
    {
        var config = new AmazonSimpleEmailServiceConfig
        {
            RegionEndpoint = RegionEndpoint.GetBySystemName(region)
        };

        // Use provided keys if available; otherwise rely on IAM role in production
        if (!string.IsNullOrEmpty(awsAccessKey) && !string.IsNullOrEmpty(awsSecretKey))
        {
            _ses = new AmazonSimpleEmailServiceClient(awsAccessKey, awsSecretKey, config);
        }
        else
        {
            _ses = new AmazonSimpleEmailServiceClient(config);
        }
    }

    public async Task SendEmailAsync(
        string from,
        string to,
        string subject,
        string body,
        string? replyTo = null)
    {
        var sendRequest = new SendEmailRequest
        {
            Source = from, // must be a verified SES email
            Destination = new Destination
            {
                ToAddresses = new List<string> { to }
            },
            Message = new Message
            {
                Subject = new Content
                {
                    Data = subject,
                    Charset = "UTF-8"
                },
                Body = new Body
                {
                    Text = new Content(body)
                }
            },
            ConfigurationSetName = "EmailTrackingSet" // optional, logs delivery/bounce
        };

        if (!string.IsNullOrEmpty(replyTo))
        {
            sendRequest.ReplyToAddresses = new List<string> { replyTo };
        }

        try
        {
            await _ses.SendEmailAsync(sendRequest);
            Console.WriteLine("Email sent successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("SES send failed: " + ex.Message);
            throw;
        }
    }
}