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
            var existingMolecularMosaic = _dbContext.MolecularMosaics.FirstOrDefault(m => m.Title == molecularMosaic.Title);
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
            existingMolecularMosaic.ConnectorTags = molecularMosaic.ConnectorTags;
            existingMolecularMosaic.KaleidoscopeTags = molecularMosaic.KaleidoscopeTags;
            existingMolecularMosaic.AssemblageTagId = molecularMosaic.AssemblageTagId;
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
}
