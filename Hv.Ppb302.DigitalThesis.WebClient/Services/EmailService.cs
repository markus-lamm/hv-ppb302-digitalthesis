using Hv.Ppb302.DigitalThesis.WebClient.Models;
using System.Net.Mail;
using System.Net;

namespace Hv.Ppb302.DigitalThesis.WebClient.Services
{
    public class EmailService
    {
        private readonly SmtpClient _smtpClient = new()
        {
            Host = "smtp-mail.outlook.com",
            Port = 587,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential("DigitalAvhandlingen@outlook.com", "H0gskolanVastDigitalAvhandling")
        };

        private readonly MailMessage _mailMessage;

        public EmailService(Email email)
        {
            using var message = new MailMessage("DigitalAvhandlingen@outlook.com", email.Receiver);
            message.Subject = email.Subject;
            message.Body = email.Body;

            _mailMessage = message;
        }

        public void SendMail()
        { 
            _smtpClient.Send(_mailMessage);
        }
    }
}
