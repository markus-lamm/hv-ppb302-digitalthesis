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
        return _dbContext.MolarMosaics
            .Include(g => g.ConnectorTags)
            .FirstOrDefault(g => g.Id == id);
    }

    public List<MolarMosaic>? GetAll()
    {
        return _dbContext.MolarMosaics
            .Include(g => g.ConnectorTags)
            .ToList();
    }

    public void Create(MolarMosaic molarMosaic)
    {
        var existingMolarMosaic = _dbContext.MolarMosaics.FirstOrDefault(m => m.Title == molarMosaic.Title);
        if (existingMolarMosaic != null)
        {
            throw new Exception("A molar mosaic with the same title already exists");
        }
        _dbContext.MolarMosaics.Add(molarMosaic);
        _dbContext.SaveChanges();
    }

    public void Update(MolarMosaic molarMosaic)
    {
        var existingMolarMosaic = _dbContext.MolarMosaics.Find(molarMosaic.Id);
        if (existingMolarMosaic == null)
        {
            throw new Exception("The molar mosaic does not exist");
        }
        existingMolarMosaic.Title = molarMosaic.Title;
        existingMolarMosaic.Content = molarMosaic.Content;
        existingMolarMosaic.PdfFilePath = molarMosaic.PdfFilePath;
        existingMolarMosaic.HasAudio = molarMosaic.HasAudio;
        existingMolarMosaic.AudioFilePath = molarMosaic.AudioFilePath;
        _dbContext.SaveChanges();
    }

    public void Delete(Guid id)
    {
        var existingMolarMosaic = _dbContext.MolarMosaics.Find(id);
        if (existingMolarMosaic == null)
        {
            throw new Exception("The molar mosaic does not exist");
        }
        _dbContext.MolarMosaics.Remove(existingMolarMosaic);
        _dbContext.SaveChanges();
    }

    public void DeleteAllByTitle(string title)
    {
        var existingMolarMosaics = _dbContext.MolarMosaics
            .Where(g => g.Title!.Contains(title))
            .ToList();
        if (existingMolarMosaics.Count == 0)
        {
            return;
        }
        _dbContext.MolarMosaics.RemoveRange(existingMolarMosaics);
        _dbContext.SaveChanges();
    }

    public void AddConnectorTag(Guid molarMosaicId, Guid connectorTagId)
    {
        var molarMosaic = _dbContext.MolarMosaics.Find(molarMosaicId);
        if (molarMosaic == null)
        {
            throw new Exception("The molar mosaic does not exist");
        }

        var connectorTag = _dbContext.ConnectorTags.Find(connectorTagId);
        if (connectorTag == null)
        {
            throw new Exception("The connector tag does not exist");
        }

        molarMosaic.ConnectorTags.Add(connectorTag);
        connectorTag.MolarMosaics.Add(molarMosaic);
        _dbContext.SaveChanges();
    }

    public void RemoveConnectorTag(Guid molarMosaicId, Guid connectorTagId)
    {
        var molarMosaic = _dbContext.MolarMosaics.Find(molarMosaicId);
        if (molarMosaic == null)
        {
            throw new Exception("The molar mosaic does not exist");
        }

        var connectorTag = _dbContext.ConnectorTags.Find(connectorTagId);
        if (connectorTag == null)
        {
            throw new Exception("The connector tag does not exist");
        }

        molarMosaic.ConnectorTags.Remove(connectorTag);
        connectorTag.MolarMosaics.Remove(molarMosaic);
        _dbContext.SaveChanges();
    }
}
