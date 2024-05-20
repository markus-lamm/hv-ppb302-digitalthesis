using Hv.Ppb302.DigitalThesis.WebClient.Models;
using Microsoft.EntityFrameworkCore;

namespace Hv.Ppb302.DigitalThesis.WebClient.Data;

public class GeoTagRepository : IRepository<GeoTag>
{
    private readonly DigitalThesisDbContext _dbContext;

    public GeoTagRepository(DigitalThesisDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public GeoTag? Get(Guid id)
    {
        return _dbContext.GeoTags
            .Include(g => g.ConnectorTags)
            .FirstOrDefault(g => g.Id == id);
    }

    public List<GeoTag>? GetAll()
    {
        return _dbContext.GeoTags
            .Include(g => g.ConnectorTags)
            .ToList();
    }

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

    public void DeleteAllByTitle(string title)
    {
        var existingGeoTags = _dbContext.GeoTags
            .Where(g => g.Title!.Contains(title))
            .ToList();

        if (existingGeoTags.Count == 0)
        {
            return;
        }

        _dbContext.GeoTags.RemoveRange(existingGeoTags);
        _dbContext.SaveChanges();
    }

    public void AddConnectorTag(Guid geoTagId, Guid connectorTagId)
    {
        var geoTag = _dbContext.GeoTags.Find(geoTagId);
        if (geoTag == null)
        {
            throw new Exception("The geotag does not exist");
        }

        var connectorTag = _dbContext.ConnectorTags.Find(connectorTagId);
        if (connectorTag == null)
        {
            throw new Exception("The connector tag does not exist");
        }

        geoTag.ConnectorTags.Add(connectorTag);
        connectorTag.GeoTags.Add(geoTag);
        _dbContext.SaveChanges();
    }

    public void RemoveConnectorTag(Guid geoTagId, Guid connectorTagId)
    {
        var geoTag = _dbContext.GeoTags.Find(geoTagId);
        if (geoTag == null)
        {
            throw new Exception("The geotag does not exist");
        }

        var connectorTag = _dbContext.ConnectorTags.Find(connectorTagId);
        if (connectorTag == null)
        {
            throw new Exception("The connector tag does not exist");
        }

        geoTag.ConnectorTags.Remove(connectorTag);
        connectorTag.GeoTags.Remove(geoTag);
        _dbContext.SaveChanges();
    }
}
