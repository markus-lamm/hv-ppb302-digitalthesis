using Hv.Ppb302.DigitalThesis.WebClient.Models;
using Microsoft.EntityFrameworkCore;

namespace Hv.Ppb302.DigitalThesis.WebClient.Data;

public class GroupTagRepository : IRepository<GroupTag>
{
    private readonly DigitalThesisDbContext _dbContext;

    public GroupTagRepository(DigitalThesisDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public GroupTag? Get(Guid id)
    {
        return _dbContext.GroupTags
            .Include(g => g.GeoTags)
            .Include(g => g.MolarMosaics)
            .Include(g => g.MolecularMosaics)
            .FirstOrDefault(g => g.Id == id);
    }

    public List<GroupTag>? GetAll()
    {
        return _dbContext.GroupTags
            .Include(g => g.GeoTags)
            .Include(g => g.MolarMosaics)
            .Include(g => g.MolecularMosaics)
            .Include(g => g.KaleidoscopeMosaics)
            .ToList();
    }

    public void Create(GroupTag groupTag)
    {
        var existingGroupTag = _dbContext.GroupTags.FirstOrDefault(g => g.Name == groupTag.Name);
        if (existingGroupTag != null)
        {
            throw new Exception("A group tag with the same name already exists");
        }
        _dbContext.GroupTags.Add(groupTag);
        _dbContext.SaveChanges();
    }

    public void Update(GroupTag groupTag)
    {
        var existingGroupTag = _dbContext.GroupTags.Find(groupTag.Id);
        if (existingGroupTag == null)
        {
            throw new Exception("The group tag does not exist");
        }
        existingGroupTag.Name = groupTag.Name;
        existingGroupTag.MolarMosaics = groupTag.MolarMosaics;
        existingGroupTag.MolecularMosaics = groupTag.MolecularMosaics;
        _dbContext.SaveChanges();
    }

    public void Delete(Guid id)
    {
        var existingGroupTag = _dbContext.GroupTags.Find(id);
        if (existingGroupTag == null)
        {
            throw new Exception("The group tag does not exist");
        }
        _dbContext.GroupTags.Remove(existingGroupTag);
        _dbContext.SaveChanges();
    }

    public void DeleteAllByName(string name)
    {
        var existingGroupTags = _dbContext.GroupTags
            .Where(g => g.Name!.Contains(name))
            .ToList();
        if (existingGroupTags.Count == 0)
        {
            return;
        }
        _dbContext.GroupTags.RemoveRange(existingGroupTags);
        _dbContext.SaveChanges();
    }
}
