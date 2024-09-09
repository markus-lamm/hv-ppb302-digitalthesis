namespace Hv.Ppb302.DigitalThesis.WebClient.Models
{
    public class Email
    {
        public string Receiver { get; set; }
        public string Subject { get; } = "DigitalThesis Website Link";
        public string Body { get; } = "This is the link to the DigitalThesis website {templink}";

        public Email(string receiver)
        {
            Receiver = receiver;
        }
    }
}
