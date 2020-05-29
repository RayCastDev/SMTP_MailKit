using System;
using System.IO;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using NotificationService.Configuration;
using NotificationService.Model;

namespace NotificationService.Service
{
    public class SmtpProvider:ISmtpProvider
    {
        private readonly ISmtpSettings _smtpSettings;

        public SmtpProvider(ISmtpSettings smtpSettings)
        {
            _smtpSettings = smtpSettings;
        }

        public async Task<bool> SendEmailAsync(EmailMessage emailMessage)
        {
            try
            {
                var message = CreateMimeMessage(emailMessage);

                using (var smtpClient = new SmtpClient())
                {
                    if (!smtpClient.IsConnected)
                    {
                        await smtpClient.ConnectAsync(_smtpSettings.SmtpServer, _smtpSettings.SmtpPort, _smtpSettings.SmtpUseSsl);
                    }
                    if (!smtpClient.IsAuthenticated)
                    {
                        await smtpClient.AuthenticateAsync(_smtpSettings.SmtpUsername, _smtpSettings.SmtpPassword);
                    }
                    await smtpClient.SendAsync(message);
                    await smtpClient.DisconnectAsync(true);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

            return true;
        }

        private MimeMessage CreateMimeMessage(EmailMessage emailMessage)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.To.Add(new MailboxAddress(emailMessage.RecipientAddress));
            mimeMessage.From.Add(new MailboxAddress(emailMessage.SenderName, _smtpSettings.SmtpUsername));
            mimeMessage.Subject = emailMessage.Subject;

            var builder = new BodyBuilder() { HtmlBody = emailMessage.Content };

            if (emailMessage.Attachments != null)
            {
                foreach (var file in emailMessage.Attachments)
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        file.CopyTo(memoryStream);
                        builder.Attachments.Add(file.FileName, memoryStream.ToArray());
                    }
                } 
            }

            mimeMessage.Body = builder.ToMessageBody();
            return mimeMessage;
        }
    }
}
