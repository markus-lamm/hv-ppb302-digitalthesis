using Hv.Ppb302.DigitalThesis.WebClient.Models;
using Microsoft.EntityFrameworkCore;

namespace Hv.Ppb302.DigitalThesis.WebClient.Data;

public class MolarMosaicRepository(DigitalThesisDbContext dbContext) : IRepository<MolarMosaic>
{
    public MolarMosaic? Get(Guid id)
    {
        try
        {
            return dbContext.MolarMosaics
                .Include(g => g.ConnectorTags)
                .Include(m => m.KaleidoscopeTags)
                .Include(q => q.AssemblageTag)
                .FirstOrDefault(g => g.Id == id);
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }

    public List<MolarMosaic>? GetAll()
    {
        try
        {
            return dbContext.MolarMosaics
                .Include(g => g.ConnectorTags)
                .Include(m => m.KaleidoscopeTags)
                .ToList();
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }

    public void Create(MolarMosaic molarMosaic)
    {
        try
        {
            var existingMolarMosaic = dbContext.MolarMosaics.FirstOrDefault(m => m.Title == molarMosaic.Title);
            if (existingMolarMosaic != null)
            {
                throw new Exception("A molar mosaic with the same title already exists");
            }
            dbContext.MolarMosaics.Add(molarMosaic);
            dbContext.SaveChanges();
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }

    public void Update(MolarMosaic molarMosaic)
    {
        try
        {
            var existingMolarMosaic = dbContext.MolarMosaics.Find(molarMosaic.Id);
            if (existingMolarMosaic == null)
            {
                throw new Exception("The molar mosaic does not exist");
            }
            existingMolarMosaic.Title = molarMosaic.Title;
            existingMolarMosaic.Content = molarMosaic.Content;
            existingMolarMosaic.PdfFilePath = molarMosaic.PdfFilePath;
            existingMolarMosaic.AudioFilePath = molarMosaic.AudioFilePath;
            existingMolarMosaic.Becomings = molarMosaic.Becomings;
            existingMolarMosaic.AssemblageTag = molarMosaic.AssemblageTag;
            existingMolarMosaic.ConnectorTags = molarMosaic.ConnectorTags;
            existingMolarMosaic.KaleidoscopeTags = molarMosaic.KaleidoscopeTags;
            existingMolarMosaic.AssemblageTagId = molarMosaic.AssemblageTagId;
            existingMolarMosaic.IsVisible = molarMosaic.IsVisible;
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
            var existingMolarMosaic = dbContext.MolarMosaics.Find(id);
            if (existingMolarMosaic == null)
            {
                throw new Exception("The molar mosaic does not exist");
            }
            dbContext.MolarMosaics.Remove(existingMolarMosaic);
            dbContext.SaveChanges();
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }
}
