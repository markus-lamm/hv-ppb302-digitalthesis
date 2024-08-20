namespace Hv.Ppb302.DigitalThesis.WebClient.Models;

public class FileViewViewModel
{
    public MimeDetective.Storage.Category Category { get; set; }
    public string? Name { get; set; }
    public string? FileUrl { get; set; }
    public bool? IsMaterial { get; set; }
}
