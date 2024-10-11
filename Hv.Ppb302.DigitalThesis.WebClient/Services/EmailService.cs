using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Auth.OAuth2;
using Hv.Ppb302.DigitalThesis.WebClient.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Hv.Ppb302.DigitalThesis.WebClient.Services;

public class EmailService
{
    private static string _clientId = "536688781308-ps46hcq3u6up5bmhbaqmt37bnl8bvh09.apps.googleusercontent.com";
    private static string _refreshToken { get; set; } = "1//04o247UgBtqFnCgYIARAAGAQSNwF-L9Ireu053MXBjX4uhlreIY4NUjONwK6xdvGMZfARyuM8rdZp5GGThV_sWkGnPYjpRq7W-dA";
    private static string _clientSecret { get; set; } = "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";

    public async Task SendMail(Email email)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Digital Thesis", "digitalavhandling@gmail.com"));
        message.To.Add(new MailboxAddress("", email.Receiver));
        message.Subject = email.Subject;

        message.Body = new TextPart("plain")
        {
            Text = email.Body
        };
        var token = await GetNewAccessTokenAsync();
        using var client = new SmtpClient();
        await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

        await client.AuthenticateAsync(new SaslMechanismOAuth2("digitalavhandling@gmail.com", token));

        await client.SendAsync(message);

        await client.DisconnectAsync(true);
    }

    private static async Task<string> GetNewAccessTokenAsync()
    {
        var credential = new UserCredential(
            new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = _clientId,
                    ClientSecret = _clientSecret
                },
                Scopes = ["https://mail.google.com/"]
            }),
            null, 
            new TokenResponse { RefreshToken = _refreshToken }
        );

        await credential.RefreshTokenAsync(CancellationToken.None);
        

        return credential.Token.AccessToken;
    }
}

 