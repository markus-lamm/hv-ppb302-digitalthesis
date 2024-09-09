using Hv.Ppb302.DigitalThesis.WebClient.Models;

namespace Hv.Ppb302.DigitalThesis.WebClient.Data;

public class GeoTagRepository(DigitalThesisDbContext dbContext) : IRepository<GeoTag>
{
    public GeoTag? Get(Guid id)
    {
        try
        {
            return dbContext.GeoTags.FirstOrDefault(g => g.Id == id);
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }

    public List<GeoTag>? GetAll()
    {
        try
        {
            return dbContext.GeoTags.ToList();
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }

    public void Create(GeoTag geoTag)
    {
        try
        {
            var existingGeoTag = dbContext.GeoTags.FirstOrDefault(g => g.Title == geoTag.Title);
            if (existingGeoTag != null)
            {
                throw new Exception("A geotag with the same title already exists");
            }
            dbContext.GeoTags.Add(geoTag);
            dbContext.SaveChanges();
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }

    public void Update(GeoTag geoTag)
    {
        try
        {
            var existingGeoTag = dbContext.GeoTags.Find(geoTag.Id);
            if (existingGeoTag == null)
            {
                throw new Exception("The geotag does not exist");
            }
            existingGeoTag.Title = geoTag.Title;
            existingGeoTag.Content = geoTag.Content;
            existingGeoTag.PdfFilePath = geoTag.PdfFilePath;
            existingGeoTag.AudioFilePath = geoTag.AudioFilePath;
            existingGeoTag.IsVisible = geoTag.IsVisible;
            dbContext.SaveChanges();
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }

    public void Delete(Guid id)
    {
        try
        {
            var existingGeoTag = dbContext.GeoTags.Find(id);
            if (existingGeoTag == null)
            {
                throw new Exception("The geotag does not exist");
            }
            dbContext.GeoTags.Remove(existingGeoTag);
            dbContext.SaveChanges();
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }
}
