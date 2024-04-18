using Amazon;
using Microsoft.AspNetCore.Mvc;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;

namespace DavidBekeris.Controllers
{
    public class KontaktController : Controller
    {
        private readonly IAmazonSimpleEmailService _amazonSimpleEmailService;
        public IActionResult Kontakt()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult SendWithSDK(string to, string subject, string message)
        //    Amazon.SimpleEmail.AmazonSimpleEmailServiceClient sesClient =
        //    new Amazon.SimpleEmail.AmazonSimpleEmailServiceClient(RegionEndpoint.EUCentral1);
        //Amazon.SimpleEmail.Model.SendEmailRequest sendRequest
        //    = new Amazon.SimpleEmail.Model.SendEmailRequest()
        //    {
        //        Destination = new Amazon.SimpleEmail.Model.Destination()
        //        {
        //            ToAddresses = new List<string>{ to }
        //        },
        //        Message = new Amazon.SimpleEmail.Model.Message
        //        {
        //            Body = new Amazon.SimpleEmail.Model.Body
        //            {
        //                Html = new Amazon.SimpleEmail.Model.Body
        //                {
        //                    Charset = "UTF-8",
        //                    Data = message
        //                },
        //                Source =
        //            }
        //        }
        //    }

        public async Task<string> SendEmailAsync(List<string> toAddresses,
                List<string> ccAddresses, List<string> bccAddresses,
                string bodyHtml, string bodyText, string subject, string senderAddress)
        {
            var messageId = "";
            try
            {
                var response = await _amazonSimpleEmailService.SendEmailAsync(
                    new SendEmailRequest
                    {
                        Destination = new Destination
                        {
                            BccAddresses = bccAddresses,
                            CcAddresses = ccAddresses,
                            ToAddresses = toAddresses
                        },
                        Message = new Message
                        {
                            Body = new Body
                            {
                                Html = new Content
                                {
                                    Charset = "UTF-8",
                                    Data = bodyHtml
                                },
                                Text = new Content
                                {
                                    Charset = "UTF-8",
                                    Data = bodyText
                                }
                            },
                            Subject = new Content
                            {
                                Charset = "UTF-8",
                                Data = subject
                            }
                        },
                        Source = senderAddress
                    });
                messageId = response.MessageId;
            }
            catch (Exception ex)
            {
                Console.WriteLine("SendEmailAsync failed with exception: " + ex.Message);
            }

            return messageId;
        }
    }
}
