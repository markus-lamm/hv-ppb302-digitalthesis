using Hv.Ppb302.DigitalThesis.WebClient.Models;
using Microsoft.EntityFrameworkCore;

namespace Hv.Ppb302.DigitalThesis.WebClient.Data;

public class MolecularMosaicRepository(DigitalThesisDbContext dbContext) : IRepository<MolecularMosaic>
{
    public MolecularMosaic? Get(Guid id)
    {
        try
        {
            return dbContext.MolecularMosaics
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

    public List<MolecularMosaic>? GetAll()
    {
        try
        {
            return dbContext.MolecularMosaics
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
            var existingMolecularMosaic = dbContext.MolecularMosaics.FirstOrDefault(m => m.Title == molecularMosaic.Title);
            if (existingMolecularMosaic != null)
            {
                throw new Exception("A molecular mosaic with the same title already exists");
            }
            dbContext.MolecularMosaics.Add(molecularMosaic);
            dbContext.SaveChanges();
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
            var existingMolecularMosaic = dbContext.MolecularMosaics.Find(molecularMosaic.Id);
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
            existingMolecularMosaic.IsVisible = molecularMosaic.IsVisible;
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
            var existingMolecularMosaic = dbContext.MolecularMosaics.Find(id);
            if (existingMolecularMosaic == null)
            {
                throw new Exception("The molecular mosaic does not exist");
            }
            dbContext.MolecularMosaics.Remove(existingMolecularMosaic);
            dbContext.SaveChanges();
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }
}
