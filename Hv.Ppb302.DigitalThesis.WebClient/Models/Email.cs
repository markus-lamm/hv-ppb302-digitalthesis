namespace Hv.Ppb302.DigitalThesis.WebClient.Models;

public class Email(string receiver)
{
    public string Receiver { get; set; } = receiver;
    public string Subject { get; } = "DigitalThesis Website Link";
    public string Body { get; } = $"This is the link to the DigitalThesis website https://da.ios.hv.se/DigitalThesis" +
                                  $"\n\n\n" +
                                  $"Please do not reply to this mail\n" +
                                  $"Svara inte på detta mejl";
}
