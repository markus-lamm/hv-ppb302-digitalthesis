namespace Hv.Ppb302.DigitalThesis.WebClient.Models;

public class Page
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Name { get; set; }
    public string? Content { get; set; }
}
