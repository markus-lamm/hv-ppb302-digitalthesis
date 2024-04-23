using Hv.Ppb302.DigitalThesis.WebClient.Models;

namespace Hv.Ppb302.DigitalThesis.WebClient.Data;

public class GeoTagCrud
{
    private readonly DigitalThesisDbContext _dbContext;

    public GeoTagCrud(DigitalThesisDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public GeoTag? GetGeoTag(Guid id) => _dbContext.GeoTags.Find(id);

    public List<GeoTag>? GetGeoTags() => _dbContext.GeoTags.ToList();

    public void CreateGeoTag(GeoTag geoTag)
    {
        var existingGeoTag = _dbContext.GeoTags.FirstOrDefault(g => g.Title == geoTag.Title);
        if (existingGeoTag != null)
        {
            throw new Exception("A geotag with the same title already exists");
        }
        _dbContext.GeoTags.Add(geoTag);
        _dbContext.SaveChanges();
    }

    public void UpdateGeoTag(GeoTag geoTag)
    {
        var existingGeoTag = _dbContext.GeoTags.Find(geoTag.Id);
        if (existingGeoTag == null)
        {
            throw new Exception("The geotag does not exist");
        }
        existingGeoTag.Title = geoTag.Title;
        existingGeoTag.Content = geoTag.Content;
        existingGeoTag.PdfFilePath = geoTag.PdfFilePath;
        existingGeoTag.HasAudio = geoTag.HasAudio;
        existingGeoTag.AudioFilePath = geoTag.AudioFilePath;
        _dbContext.SaveChanges();
    }

    public void DeleteGeoTag(Guid id)
    {
        var existingGeoTag = _dbContext.GeoTags.Find(id);
        if (existingGeoTag == null)
        {
            throw new Exception("The geotag does not exist");
        }
        _dbContext.GeoTags.Remove(existingGeoTag);
        _dbContext.SaveChanges();
    }

    public void CreatePreDefinedGeoTags()
    {
        var geoTags = new List<GeoTag>
        {
            new GeoTag
            {
                Title = "Problems",
                Content = "Test",
                PdfFilePath = "Test",
                HasAudio = true,
                AudioFilePath = "Test"
            },
            new GeoTag
            {
                Title = "A Solution",
                Content = "Test",
                PdfFilePath = "Test",
                HasAudio = true,
                AudioFilePath = "Test"
            },
            new GeoTag
            {
                Title = "Deleuzian Ontology in Education",
                Content = "Test",
                PdfFilePath = "Test",
                HasAudio = true,
                AudioFilePath = "Test"
            },
            new GeoTag
            {
                Title = "An Inquiry-Machine",
                Content = "Test",
                PdfFilePath = "Test",
                HasAudio = true,
                AudioFilePath = "Test"
            },
            new GeoTag
            {
                Title = "Ignore This",
                Content = "Test",
                PdfFilePath = "Test",
                HasAudio = true,
                AudioFilePath = "Test"
            },
            new GeoTag
            {
                Title = "Kaleido-scoping",
                Content = "Test",
                PdfFilePath = "Test",
                HasAudio = true,
                AudioFilePath = "Test"
            },
        };
        foreach (var geoTag in geoTags)
        {
            var existingGeoTag = _dbContext.GeoTags.FirstOrDefault(g => g.Title == geoTag.Title);
            if (existingGeoTag == null)
            {
                CreateGeoTag(geoTag);
            }
        }
    }

}
