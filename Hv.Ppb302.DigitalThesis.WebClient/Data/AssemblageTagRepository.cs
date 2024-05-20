using Hv.Ppb302.DigitalThesis.WebClient.Models;
using Microsoft.EntityFrameworkCore;

namespace Hv.Ppb302.DigitalThesis.WebClient.Data;

public class AssemblageTagRepository : IRepository<AssemblageTag>
{
    private readonly DigitalThesisDbContext _dbContext;

    public AssemblageTagRepository(DigitalThesisDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public AssemblageTag? Get(Guid id)
    {
        return _dbContext.AssemblageTags
            .Include(g => g.MolarMosaics)
            .Include(g => g.MolecularMosaics)
            .FirstOrDefault(g => g.Id == id);
    }

    public List<AssemblageTag>? GetAll()
    {
        return _dbContext.AssemblageTags
            .Include(g => g.MolarMosaics)
            .Include(g => g.MolecularMosaics)
            .ToList();
    }

    public void Create(AssemblageTag assemblageTag)
    {
        var existingAssemblageTag = _dbContext.AssemblageTags.FirstOrDefault(g => g.Name == assemblageTag.Name);
        if (existingAssemblageTag != null)
        {
            throw new Exception("A assemblage tag with the same name already exists");
        }
        _dbContext.AssemblageTags.Add(assemblageTag);
        _dbContext.SaveChanges();
    }

    public void Update(AssemblageTag assemblageTag)
    {
        var existingAssemblageTag = _dbContext.AssemblageTags.Find(assemblageTag.Id);
        if (existingAssemblageTag == null)
        {
            throw new Exception("The assemblage tag does not exist");
        }
        existingAssemblageTag.Name = assemblageTag.Name;
        existingAssemblageTag.MolarMosaics = assemblageTag.MolarMosaics;
        existingAssemblageTag.MolecularMosaics = assemblageTag.MolecularMosaics;
        _dbContext.SaveChanges();
    }

    public void Delete(Guid id)
    {
        var existingAssemblageTag = _dbContext.AssemblageTags.Find(id);
        if (existingAssemblageTag == null)
        {
            throw new Exception("The assemblage tag does not exist");
        }
        _dbContext.AssemblageTags.Remove(existingAssemblageTag);
        _dbContext.SaveChanges();
    }

    public void DeleteAllByName(string name)
    {
        var existingAssemblageTag = _dbContext.AssemblageTags
            .Where(g => g.Name!.Contains(name))
            .ToList();
        if (existingAssemblageTag.Count == 0)
        {
            return;
        }
        _dbContext.AssemblageTags.RemoveRange(existingAssemblageTag);
        _dbContext.SaveChanges();
    }
}
