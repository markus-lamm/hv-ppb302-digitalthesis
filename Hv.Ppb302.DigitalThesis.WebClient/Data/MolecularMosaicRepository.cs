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
        return _dbContext.MolecularMosaics
            .Include(g => g.GroupTags)
            .FirstOrDefault(g => g.Id == id);
    }

    public List<MolecularMosaic>? GetAll()
    {
        return _dbContext.MolecularMosaics
            .Include(g => g.GroupTags)
            .ToList();
    }

    public void Create(MolecularMosaic molecularMosaic)
    {
        var existingMolecularMosaic = _dbContext.MolarMosaics.FirstOrDefault(m => m.Title == molecularMosaic.Title);
        if (existingMolecularMosaic != null)
        {
            return;
        }
        _dbContext.MolecularMosaics.Add(molecularMosaic);
        _dbContext.SaveChanges();
    }

    public void Update(MolecularMosaic molecularMosaic)
    {
        var existingMolecularMosaic = _dbContext.MolarMosaics.Find(molecularMosaic.Id);
        if (existingMolecularMosaic == null)
        {
            throw new Exception("The molar mosaic does not exist");
        }
        existingMolecularMosaic.Title = molecularMosaic.Title;
        existingMolecularMosaic.Content = molecularMosaic.Content;
        existingMolecularMosaic.PdfFilePath = molecularMosaic.PdfFilePath;
        existingMolecularMosaic.HasAudio = molecularMosaic.HasAudio;
        existingMolecularMosaic.AudioFilePath = molecularMosaic.AudioFilePath;
        _dbContext.SaveChanges();
    }

    public void Delete(Guid id)
    {
        var existingMolecularMosaic = _dbContext.MolecularMosaics.Find(id);
        if (existingMolecularMosaic == null)
        {
            throw new Exception("The molar mosaic does not exist");
        }
        _dbContext.MolecularMosaics.Remove(existingMolecularMosaic);
        _dbContext.SaveChanges();
    }

    public void DeleteAllByTitle(string title)
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

    public void AddGroupTag(Guid molecularMosaicId, Guid groupTagId)
    {
        var molecularMosaic = _dbContext.MolecularMosaics.Find(molecularMosaicId);
        if (molecularMosaic == null)
        {
            throw new Exception("The molecular mosaic does not exist");
        }

        var groupTag = _dbContext.GroupTags.Find(groupTagId);
        if (groupTag == null)
        {
            throw new Exception("The group tag does not exist");
        }

        molecularMosaic.GroupTags.Add(groupTag);
        groupTag.MolecularMosaics.Add(molecularMosaic);

        _dbContext.SaveChanges();
    }
}
