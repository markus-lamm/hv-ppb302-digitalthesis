using Hv.Ppb302.DigitalThesis.WebClient.Models;
using Microsoft.EntityFrameworkCore;

namespace Hv.Ppb302.DigitalThesis.WebClient.Data;

public class KaleidoscopeTagRepository : IRepository<KaleidoscopeTag>
{
    private readonly DigitalThesisDbContext _dbContext;

    public KaleidoscopeTagRepository(DigitalThesisDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public KaleidoscopeTag? Get(Guid id)
    {
        return _dbContext.KaleidoscopeTags
            .Include(g => g.MolarMosaics)
            .Include(g => g.MolecularMosaics)
            .FirstOrDefault(g => g.Id == id);
    }

    public List<KaleidoscopeTag>? GetAll()
    {
        return _dbContext.KaleidoscopeTags
            .Include(g => g.MolarMosaics)
            .Include(g => g.MolecularMosaics)
            .ToList();
    }

    public void Create(KaleidoscopeTag kaleidoscopeTag)
    {
        var existingKaleidoscopeTag = _dbContext.ConnectorTags.FirstOrDefault(g => g.Name == kaleidoscopeTag.Name);
        if (existingKaleidoscopeTag != null)
        {
            throw new Exception("A kaleidoscope tag with the same name already exists");
        }
        _dbContext.KaleidoscopeTags.Add(kaleidoscopeTag);
        _dbContext.SaveChanges();
    }

    public void Update(KaleidoscopeTag kaleidoscopeTag)
    {
        var existingKaleidoscopeTag = _dbContext.ConnectorTags.Find(kaleidoscopeTag.Id);
        if (existingKaleidoscopeTag == null)
        {
            throw new Exception("The kaleidoscope tag does not exist");
        }
        existingKaleidoscopeTag.Name = kaleidoscopeTag.Name;
        existingKaleidoscopeTag.MolarMosaics = kaleidoscopeTag.MolarMosaics;
        existingKaleidoscopeTag.MolecularMosaics = kaleidoscopeTag.MolecularMosaics;
        _dbContext.SaveChanges();
    }

    public void Delete(Guid id)
    {
        var existingKaleidoscopeTag = _dbContext.KaleidoscopeTags.Find(id);
        if (existingKaleidoscopeTag == null)
        {
            throw new Exception("The kaleidoscope tag does not exist");
        }
        _dbContext.KaleidoscopeTags.Remove(existingKaleidoscopeTag);
        _dbContext.SaveChanges();
    }

    public void DeleteAllByName(string name)
    {
        var existingKaleidoscopeTags = _dbContext.KaleidoscopeTags
            .Where(g => g.Name!.Contains(name))
            .ToList();
        if (existingKaleidoscopeTags.Count == 0)
        {
            return;
        }
        _dbContext.KaleidoscopeTags.RemoveRange(existingKaleidoscopeTags);
        _dbContext.SaveChanges();
    }
}
