using Amazon;
using Amazon.Runtime.CredentialManagement;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using System.Text.Json;

namespace DavidBekeris.Services;


public class EmailService
{
    private readonly IAmazonSimpleEmailService _ses;

    public EmailService(string region)
    {
        var config = new AmazonSimpleEmailServiceConfig
        {
            RegionEndpoint = RegionEndpoint.GetBySystemName(region)
        };

        // Use IAM role credentials automatically (no access keys needed)
        _ses = new AmazonSimpleEmailServiceClient(config);
    }

    public async Task SendEmailAsync(string from, string to, string subject, string body, string replyTo = null)
    {
        var sendRequest = new SendEmailRequest
        {
            Source = "noreply@davidbekeris.se",
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
            ConfigurationSetName = "EmailTrackingSet"
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