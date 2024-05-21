namespace Hv.Ppb302.DigitalThesis.WebClient.Models;

public class MolecularMosaic
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Title { get; set; }
    public string? Content { get; set; }
    public List<ConnectorTag>? ConnectorTags { get; set; } = [];
    public List<string>? Becomings { get; set; } = [];
    public List<KaleidoscopeTag>? KaleidoscopeTags { get; set; } = [];
    public Guid? AssemblageTagId { get; set; }
    public AssemblageTag? AssemblageTag { get; set; }
    public string? PdfFilePath { get; set; }
    public string? AudioFilePath { get; set; }
}