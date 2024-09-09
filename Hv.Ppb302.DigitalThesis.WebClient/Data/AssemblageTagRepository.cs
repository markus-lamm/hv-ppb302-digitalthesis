using Hv.Ppb302.DigitalThesis.WebClient.Models;
using Microsoft.EntityFrameworkCore;

namespace Hv.Ppb302.DigitalThesis.WebClient.Data;

public class AssemblageTagRepository(DigitalThesisDbContext dbContext) : IRepository<AssemblageTag>
{
    public AssemblageTag? Get(Guid id)
    {
        try
        {
            return dbContext.AssemblageTags
                .Include(g => g.MolarMosaics)
                .Include(g => g.MolecularMosaics)
                .FirstOrDefault(g => g.Id == id);
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }

    public List<AssemblageTag>? GetAll()
    {
        try
        {
            return dbContext.AssemblageTags
                .Include(g => g.MolarMosaics)
                .Include(g => g.MolecularMosaics)
                .ToList();
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }

    public void Create(AssemblageTag assemblageTag)
    {
        try
        {
            var existingAssemblageTag = dbContext.AssemblageTags.FirstOrDefault(g => g.Name == assemblageTag.Name);
            if (existingAssemblageTag != null)
            {
                throw new Exception("A assemblage tag with the same name already exists");
            }
            dbContext.AssemblageTags.Add(assemblageTag);
            dbContext.SaveChanges();
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }

    public void Update(AssemblageTag assemblageTag)
    {
        try
        {
            var existingAssemblageTag = dbContext.AssemblageTags.Find(assemblageTag.Id);
            if (existingAssemblageTag == null)
            {
                throw new Exception("The assemblage tag does not exist");
            }
            existingAssemblageTag.Name = assemblageTag.Name;
            existingAssemblageTag.MolarMosaics = assemblageTag.MolarMosaics;
            existingAssemblageTag.MolecularMosaics = assemblageTag.MolecularMosaics;
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
            var existingAssemblageTag = dbContext.AssemblageTags.Find(id);
            if (existingAssemblageTag == null)
            {
                throw new Exception("The assemblage tag does not exist");
            }
            dbContext.AssemblageTags.Remove(existingAssemblageTag);
            dbContext.SaveChanges();
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }
}
