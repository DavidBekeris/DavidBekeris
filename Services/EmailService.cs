using Amazon;
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
        _secretsManager = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(region));
    }

    public async Task<(string accessKey, string secretKey)> GetSesKeysAsync()
    {
        var request = new GetSecretValueRequest
        {
            SecretId = "MyApp/SES/Credentials" // same name you used in AWS
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

        await _ses.SendEmailAsync(sendRequest);
    }
}