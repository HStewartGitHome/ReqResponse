
using Microsoft.Extensions.Logging;
using MimeKit;
using ReqResponse.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ReqResponse.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger = null;
        private readonly EmailConfiguration _emailConfiguration = null;

        #region Constructor

        public EmailService(EmailConfiguration emailConfiguration,
                            ILogger<EmailService> logger)
        {
            _emailConfiguration = emailConfiguration;
            _logger = logger;
        }

        #endregion Constructor

        #region EmailErrorReportStrings

        public async Task EmailErrorReportStrings(List<string> strs)
        {
            await Task.Delay(0);

            string emailData = "";
            foreach (string str in strs)
            {
                _logger.LogInformation("-->" + str);
                emailData += str;
                emailData += "\r\n";
            }

            SendEmail(_emailConfiguration, emailData);
        }

        #endregion EmailErrorReportStrings

        #region private SendEmail

        private static bool SendEmail(EmailConfiguration config,
                                       string emailData)
        {
            bool result = false;
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(config.To, config.From));
                message.To.Add(new MailboxAddress(config.To, config.To));
                message.Subject = "ReqResponse Error";
                message.Body = new TextPart("plain")
                {
                    Text = emailData
                };

                using var client = new MailKit.Net.Smtp.SmtpClient();
                client.Connect(config.SmtpServer, config.Port, false);

                //SMTP server authentication if needed
                client.Authenticate(config.UserName, config.Password);

                client.Send(message);

                client.Disconnect(true);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
            }

            return result;
        }

        #endregion private SendEmail
    }
}