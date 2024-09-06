namespace Hv.Ppb302.DigitalThesis.WebClient.Models
{
    public class Email
    {
        public string Receiver { get; set; }
        public string Subject { get; } = "Avhandlinglänk";
        public string Body { get; } = "Här kommer din nya hemasidan";

        public Email(string receiver)
        {
            Receiver = receiver;
        }

    }
}
