using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Mailer.Interface;
using Mailer.Model;

namespace Mailer.Service
{
    public class MailServices : IMailServices
    {
        private readonly EmailSettings _mailSettings;
        private readonly IWebHostEnvironment _host;

        public MailServices(IOptions<EmailSettings> mailSettings, IWebHostEnvironment host)
        {
            _mailSettings = mailSettings.Value;
            _host = host;
        }
        public async Task SendEmailAsync(string mailto, string subject, string body, IList<IFormFile> attachments = null)
        {
            var email = new MimeMessage()
            {
                Sender = MailboxAddress.Parse(_mailSettings.EmailFrom),
                Subject = subject ?? string.Empty,
            };
            email.To.Add(MailboxAddress.Parse(mailto));
            var builder = new BodyBuilder();
            if (attachments != null)
            {
                byte[] FileBytes;
                foreach (var file in attachments)
                {
                    if (file.Length > 0)
                    {
                        using var ms = new MemoryStream();
                        file.CopyTo(ms);
                        FileBytes = ms.ToArray();
                        builder.Attachments.Add(file.FileName, FileBytes,ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = body;
            email.Body = builder.ToMessageBody();
            email.From.Add(new MailboxAddress("Aarya", _mailSettings.EmailFrom));
            using var smtp = new SmtpClient();
            smtp.Connect("smtp-mail.outlook.com", 587, SecureSocketOptions.StartTls);
            try
            {
                smtp.Authenticate(_mailSettings.EmailFrom, _mailSettings.Password);
            }
            catch (Exception ex)
            {
                 //Log or print the exception details
                Console.WriteLine($"Authentication failed: {ex.Message}");
                throw; // Rethrow the exception or handle it appropriately
            }

            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

    }

}
