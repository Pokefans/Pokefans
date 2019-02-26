using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Configuration;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;

namespace Pokefans.Security
{
    internal class PokefansEmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message) 
        {
            // Hier den E-Mail-Dienst einfügen, um eine E-Mail-Nachricht zu senden.

            var mailconfig = (MailSettingsSection)ConfigurationManager.GetSection("mail");

            var mail = new MimeMessage();
            mail.From.Add(new MailboxAddress("Pokefans", mailconfig.Identity.From));
            mail.To.Add(new MailboxAddress(message.Destination));
            if (!String.IsNullOrEmpty(mailconfig.Identity.ReplyTo))
            {
                mail.ReplyTo.Add(new MailboxAddress(mailconfig.Identity.ReplyTo));
            }
            mail.Subject = message.Subject;

            mail.Body = new TextPart("plain")
            {
                Text = message.Body
            };

            using (var client = new SmtpClient())
            {
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                if (!mailconfig.Server.TrustAllSsl)
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                }

                client.Connect(mailconfig.Server.Host, mailconfig.Server.Port, false);

                // Note: only needed if the SMTP server requires authentication
                if (!String.IsNullOrEmpty(mailconfig.Credentials.User))
                {
                    client.Authenticate(mailconfig.Credentials.User, mailconfig.Credentials.Password);
                }

                client.Send(mail);
                client.Disconnect(true);
            }

            return Task.FromResult(0);
        }
    }
}
