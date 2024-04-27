using Hv.Ppb302.DigitalThesis.WebClient.Models;

namespace Hv.Ppb302.DigitalThesis.WebClient.Data;

public class GeoTagRepository : IRepository<GeoTag>
{
    private readonly DigitalThesisDbContext _dbContext;

    public GeoTagRepository(DigitalThesisDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public GeoTag? Get(Guid id) => _dbContext.GeoTags.Find(id);

    public List<GeoTag>? GetAll() => _dbContext.GeoTags.ToList();

    public void Create(GeoTag geoTag)
    {
        var existingGeoTag = _dbContext.GeoTags.FirstOrDefault(g => g.Title == geoTag.Title);
        if (existingGeoTag != null)
        {
            throw new Exception("A geotag with the same title already exists");
        }
        _dbContext.GeoTags.Add(geoTag);
        _dbContext.SaveChanges();
    }

    public void Update(GeoTag geoTag)
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

    public void Delete(Guid id)
    {
        var existingGeoTag = _dbContext.GeoTags.Find(id);
        if (existingGeoTag == null)
        {
            throw new Exception("The geotag does not exist");
        }
        _dbContext.GeoTags.Remove(existingGeoTag);
        _dbContext.SaveChanges();
    }
}
