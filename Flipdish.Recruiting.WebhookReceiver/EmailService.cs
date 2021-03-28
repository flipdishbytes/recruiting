using Flipdish.Recruiting.WebhookReceiver.Models;
using Flipdish.Recruiting.WebhookReceiver.Service;
using Flipdish.Recruiting.WebhookReceiver.Strategy.LiquidTemplates;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Flipdish.Recruiting.WebhookReceiver
{
    public class EmailService : IEmailService
    {
		private readonly IFileService _fileService;

		public EmailService(IFileService fileService)
		{
			_fileService = fileService;
		}

        public string GetEmailTemplateFor(ILiquidTemplateStrategy liquidStrategy)
        {
            var template = _fileService.GetFileContents(liquidStrategy.Directory, liquidStrategy.TemplateName);
            return liquidStrategy.GetTemplate(template);
        }

        public async Task SendAsync(EmailMessage emailMessage)
        {
            await SendAsync(emailMessage.From, emailMessage.To, emailMessage.Subject, emailMessage.Body, emailMessage.Attachements, emailMessage.CC);
        }

        private static async Task SendAsync(string from, IEnumerable<string> to, string subject, string body, Dictionary<string, Stream> attachements, IEnumerable<string> cc = null)
        {
            using MailMessage mailMessage = new MailMessage
            {
                IsBodyHtml = true,
                From = new MailAddress(from),
                Subject = subject,
                Body = body
            };
            foreach (var t in to)
            {
                mailMessage.To.Add(t);
            }

            if (cc != null)
            {
                foreach (var t in cc)
                {
                    mailMessage.To.Add(t);
                }
            }
            foreach (var nameAndStreamPair in attachements)
            {
                var attachment = new Attachment(nameAndStreamPair.Value, nameAndStreamPair.Key);
                attachment.ContentId = nameAndStreamPair.Key;
                mailMessage.Attachments.Add(attachment);
            }

            using SmtpClient mailer = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential("", "") //Note: Temporarily Allow less secure apps: ON
            };
            await mailer.SendMailAsync(mailMessage);
        }
    }
}
