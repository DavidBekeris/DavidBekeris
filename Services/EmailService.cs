using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;

namespace DavidBekeris.Services;


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