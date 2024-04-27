namespace Hv.Ppb302.DigitalThesis.WebClient.Models;

public class DetailViewModel
{
    public Guid? ObjectId { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public List<GroupTag> GroupTags { get; set; } = [];
    public string? PdfFilePath { get; set; }
    public bool? HasAudio { get; set; }
    public string? AudioFilePath { get; set; }
}
