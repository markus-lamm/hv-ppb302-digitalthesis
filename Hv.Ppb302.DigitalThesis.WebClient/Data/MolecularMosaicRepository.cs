using Hv.Ppb302.DigitalThesis.WebClient.Models;
using Microsoft.EntityFrameworkCore;

namespace Hv.Ppb302.DigitalThesis.WebClient.Data;

public class MolecularMosaicRepository : IRepository<MolecularMosaic>
{
    private readonly DigitalThesisDbContext _dbContext;

    public MolecularMosaicRepository(DigitalThesisDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public MolecularMosaic? Get(Guid id)
    {
        try
        {
            return _dbContext.MolecularMosaics
                .Include(g => g.ConnectorTags)
                .Include(m => m.KaleidoscopeTags)
                .FirstOrDefault(g => g.Id == id);
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }

    public List<MolecularMosaic>? GetAll()
    {
        try
        {
            return _dbContext.MolecularMosaics
                .Include(g => g.ConnectorTags)
                .Include(m => m.KaleidoscopeTags)
                .ToList();
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }

    public void Create(MolecularMosaic molecularMosaic)
    {
        try
        {
            var existingMolecularMosaic = _dbContext.MolarMosaics.FirstOrDefault(m => m.Title == molecularMosaic.Title);
            if (existingMolecularMosaic != null)
            {
                throw new Exception("A molecular mosaic with the same title already exists");
            }
            _dbContext.MolecularMosaics.Add(molecularMosaic);
            _dbContext.SaveChanges();
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }

    public void Update(MolecularMosaic molecularMosaic)
    {
        try
        {
            var existingMolecularMosaic = _dbContext.MolecularMosaics.Find(molecularMosaic.Id);
            if (existingMolecularMosaic == null)
            {
                throw new Exception("The molecular mosaic does not exist");
            }
            existingMolecularMosaic.Title = molecularMosaic.Title;
            existingMolecularMosaic.Content = molecularMosaic.Content;
            existingMolecularMosaic.PdfFilePath = molecularMosaic.PdfFilePath;
            existingMolecularMosaic.AudioFilePath = molecularMosaic.AudioFilePath;
            existingMolecularMosaic.Becomings = molecularMosaic.Becomings;
            existingMolecularMosaic.AssemblageTag = molecularMosaic.AssemblageTag;
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
            var existingMolecularMosaic = _dbContext.MolecularMosaics.Find(id);
            if (existingMolecularMosaic == null)
            {
                throw new Exception("The molecular mosaic does not exist");
            }
            _dbContext.MolecularMosaics.Remove(existingMolecularMosaic);
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
            var existingMolecularMosaics = _dbContext.MolecularMosaics
                .Where(g => g.Title!.Contains(title))
                .ToList();
            if (existingMolecularMosaics.Count == 0)
            {
                return;
            }
            _dbContext.MolecularMosaics.RemoveRange(existingMolecularMosaics);
            _dbContext.SaveChanges();
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }

    public void AddConnectorTag(Guid molecularMosaicId, Guid connectorTagId)
    {
        try
        {
            var molecularMosaic = _dbContext.MolecularMosaics.Find(molecularMosaicId);
            if (molecularMosaic == null)
            {
                throw new Exception("The molecular mosaic does not exist");
            }

            var connectorTag = _dbContext.ConnectorTags.Find(connectorTagId);
            if (connectorTag == null)
            {
                throw new Exception("The connector tag does not exist");
            }

            molecularMosaic.ConnectorTags!.Add(connectorTag);
            connectorTag.MolecularMosaics.Add(molecularMosaic);
            _dbContext.SaveChanges();
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }

    public void AddKaleidoscopeTag(Guid molecularMosaicId, Guid kaleidoscopeTagId)
    {
        try
        {
            var molecularMosaic = _dbContext.MolecularMosaics.Find(molecularMosaicId);
            if (molecularMosaic == null)
            {
                throw new Exception("The molecular mosaic does not exist");
            }

            var kaleidoscopeTag = _dbContext.KaleidoscopeTags.Find(kaleidoscopeTagId);
            if (kaleidoscopeTag == null)
            {
                throw new Exception("The kaleidoscope tag does not exist");
            }

            molecularMosaic.KaleidoscopeTags!.Add(kaleidoscopeTag);
            kaleidoscopeTag.MolecularMosaics.Add(molecularMosaic);
            _dbContext.SaveChanges();
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }

    public void RemoveConnectorTag(Guid molecularMosaicId, Guid connectorTagId)
    {
        try
        {
            var molecularMosaic = _dbContext.MolecularMosaics.Find(molecularMosaicId);
            if (molecularMosaic == null)
            {
                throw new Exception("The molecular mosaic does not exist");
            }

            var connectorTag = _dbContext.ConnectorTags.Find(connectorTagId);
            if (connectorTag == null)
            {
                throw new Exception("The connector tag does not exist");
            }

            molecularMosaic.ConnectorTags!.Remove(connectorTag);
            connectorTag.MolecularMosaics.Remove(molecularMosaic);
            _dbContext.SaveChanges();
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }
}
