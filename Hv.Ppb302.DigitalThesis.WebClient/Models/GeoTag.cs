namespace Hv.Ppb302.DigitalThesis.WebClient.Models;

public class GeoTag
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Title { get; set; }
    public string? Content { get; set; }
    public string? PdfFilePath { get; set; }
    public string? AudioFilePath { get; set; }
    public bool? IsVisible { get; set; }
}
