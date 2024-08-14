using Hv.Ppb302.DigitalThesis.WebClient.Models;
using Microsoft.EntityFrameworkCore;

namespace Hv.Ppb302.DigitalThesis.WebClient.Data;

public class MolarMosaicRepository : IRepository<MolarMosaic>
{
    private readonly DigitalThesisDbContext _dbContext;

    public MolarMosaicRepository(DigitalThesisDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public MolarMosaic? Get(Guid id)
    {
        try
        {
            return _dbContext.MolarMosaics
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
            return _dbContext.MolarMosaics
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
            var existingMolarMosaic = _dbContext.MolarMosaics.FirstOrDefault(m => m.Title == molarMosaic.Title);
            if (existingMolarMosaic != null)
            {
                throw new Exception("A molar mosaic with the same title already exists");
            }
            _dbContext.MolarMosaics.Add(molarMosaic);
            _dbContext.SaveChanges();
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
            var existingMolarMosaic = _dbContext.MolarMosaics.Find(molarMosaic.Id);
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
            _dbContext.SaveChanges();
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
            var existingMolarMosaic = _dbContext.MolarMosaics.Find(id);
            if (existingMolarMosaic == null)
            {
                throw new Exception("The molar mosaic does not exist");
            }
            _dbContext.MolarMosaics.Remove(existingMolarMosaic);
            _dbContext.SaveChanges();
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }
}
