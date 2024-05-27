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
        try
        {
            return _dbContext.KaleidoscopeTags
                .Include(g => g.MolarMosaics)
                .Include(g => g.MolecularMosaics)
                .FirstOrDefault(g => g.Id == id);
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }

    public List<KaleidoscopeTag>? GetAll()
    {
        try
        {
            return _dbContext.KaleidoscopeTags
                .Include(g => g.MolarMosaics)
                .Include(g => g.MolecularMosaics)
                .ToList();
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }

    public void Create(KaleidoscopeTag kaleidoscopeTag)
    {
        try
        {
            var existingKaleidoscopeTag = _dbContext.KaleidoscopeTags.FirstOrDefault(g => g.Name == kaleidoscopeTag.Name);
            if (existingKaleidoscopeTag != null)
            {
                throw new Exception("A kaleidoscope tag with the same name already exists");
            }
            _dbContext.KaleidoscopeTags.Add(kaleidoscopeTag);
            _dbContext.SaveChanges();
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }

    public void Update(KaleidoscopeTag kaleidoscopeTag)
    {
        try
        {
        var existingKaleidoscopeTag = _dbContext.KaleidoscopeTags.Find(kaleidoscopeTag.Id);
        if (existingKaleidoscopeTag == null)
        {
            throw new Exception("The kaleidoscope tag does not exist");
        }
        existingKaleidoscopeTag.Name = kaleidoscopeTag.Name;
        existingKaleidoscopeTag.MolarMosaics = kaleidoscopeTag.MolarMosaics;
        existingKaleidoscopeTag.MolecularMosaics = kaleidoscopeTag.MolecularMosaics;
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
            var existingKaleidoscopeTag = _dbContext.KaleidoscopeTags.Find(id);
            if (existingKaleidoscopeTag == null)
            {
                throw new Exception("The kaleidoscope tag does not exist");
            }
            _dbContext.KaleidoscopeTags.Remove(existingKaleidoscopeTag);
            _dbContext.SaveChanges();
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }
}
