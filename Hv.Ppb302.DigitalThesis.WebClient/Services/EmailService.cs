using Hv.Ppb302.DigitalThesis.WebClient.Models;
using System.Net.Mail;
using System.Net;

namespace Hv.Ppb302.DigitalThesis.WebClient.Services;

public class EmailService
{
    private readonly SmtpClient _smtpClient;

    public EmailService()
    {
        _smtpClient = new SmtpClient
        {
            Host = "smtp-mail.outlook.com",
            Port = 587,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential("DigitalAvhandlingen@outlook.com", "brtglmcuwpcsubty")
        };
    }

    public void SendMail(Email email)
    {
        using var message = new MailMessage("DigitalAvhandlingen@outlook.com", email.Receiver)
        {
            Subject = email.Subject,
            Body = email.Body
        };

        try
        {
            _smtpClient.Send(message);
        }
        catch (SmtpException ex)
        {
            throw new InvalidOperationException("Failed to send email", ex);
        }
    }
}
