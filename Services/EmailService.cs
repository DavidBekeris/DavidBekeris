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
        // Load credentials from the default AWS CLI profile
        var chain = new CredentialProfileStoreChain();
        if (!chain.TryGetAWSCredentials("default", out var awsCredentials))
        {
            throw new Exception("Could not load AWS credentials from AWS CLI profile.");
        }

        _secretsManager = new AmazonSecretsManagerClient(
            awsCredentials,
            RegionEndpoint.GetBySystemName(region)
        );
    }

    public async Task<(string accessKey, string secretKey)> GetSesKeysAsync()
    {
        var request = new GetSecretValueRequest
        {
            SecretId = "/SES/Credentials-Main"
        };

        var response = await _secretsManager.GetSecretValueAsync(request);

        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(response.SecretString);

        return (dict["AWS_ACCESS_KEY_ID"], dict["AWS_SECRET_ACCESS_KEY"]);
    }
}

public class EmailService
{
    private readonly IAmazonSimpleEmailService _ses;

    public EmailService(string awsAccessKey, string awsSecretKey, string region)
    {
        var config = new AmazonSimpleEmailServiceConfig
        {
            RegionEndpoint = RegionEndpoint.GetBySystemName(region)
        };

        _ses = new AmazonSimpleEmailServiceClient(awsAccessKey, awsSecretKey, config);
    }

    public async Task SendEmailAsync(string from, string to, string subject, string body)
    {
        var sendRequest = new SendEmailRequest
        {
            Source = from,
            Destination = new Destination
            {
                ToAddresses = new List<string> { to }
            },
            Message = new Message
            {
                Subject = new Content(subject),
                Body = new Body
                {
                    Text = new Content(body)
                }
            }
        };
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