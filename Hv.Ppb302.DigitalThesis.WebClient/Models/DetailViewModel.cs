namespace Hv.Ppb302.DigitalThesis.WebClient.Models;

public class DetailViewModel
{
    public Guid? Id { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public List<ConnectorTag> ConnectorTags { get; set; } = [];
    public List<string> Becomings { get; set; } = [];
    public AssemblageTag? AssemblageTag { get; set; }
    public string? PdfFilePath { get; set; }
    public string? AudioFilePath { get; set; }
}
