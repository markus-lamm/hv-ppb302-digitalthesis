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

    public void DeleteAllByTitle(string title)
    {
        try
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
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }

    public void AddConnectorTag(Guid molarMosaicId, Guid connectorTagId)
    {
        try
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

            molarMosaic.ConnectorTags!.Add(connectorTag);
            connectorTag.MolarMosaics.Add(molarMosaic);
            _dbContext.SaveChanges();
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }

    public void AddKaleidoscopeTag(Guid molarMosaicId, Guid kaleidoscopeTagId)
    {
        try
        {
            var molarMosaic = _dbContext.MolarMosaics.Find(molarMosaicId);
            if (molarMosaic == null)
            {
                throw new Exception("The molar mosaic does not exist");
            }

            var kaleidoscopeTag = _dbContext.KaleidoscopeTags.Find(kaleidoscopeTagId);
            if (kaleidoscopeTag == null)
            {
                throw new Exception("The kaleidoscope tag does not exist");
            }

            molarMosaic.KaleidoscopeTags!.Add(kaleidoscopeTag);
            kaleidoscopeTag.MolarMosaics.Add(molarMosaic);
            _dbContext.SaveChanges();
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }

    public void RemoveConnectorTag(Guid molarMosaicId, Guid connectorTagId)
    {
        try
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

            molarMosaic.ConnectorTags!.Remove(connectorTag);
            connectorTag.MolarMosaics.Remove(molarMosaic);
            _dbContext.SaveChanges();
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }
}
