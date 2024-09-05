namespace Hv.Ppb302.DigitalThesis.WebClient.Models;

public class Upload
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Name { get; set; }
    public bool? IsMaterial { get; set; }
}