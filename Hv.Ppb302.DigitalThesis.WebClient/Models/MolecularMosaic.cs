namespace Hv.Ppb302.DigitalThesis.WebClient.Models
{
    public class MolecularMosaic
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public List<Tag> Tags { get; set; } = [];
        public string? PdfFilePath { get; set; }
        public string? AudioFilePath { get; set; }

    }
}
