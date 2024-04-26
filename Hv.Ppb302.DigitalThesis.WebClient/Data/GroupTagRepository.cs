using Hv.Ppb302.DigitalThesis.WebClient.Models;

namespace Hv.Ppb302.DigitalThesis.WebClient.Data;

public class GroupTagRepository : IRepository<GroupTag>
{
    private readonly DigitalThesisDbContext _dbContext;

    public GroupTagRepository(DigitalThesisDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public GroupTag? Get(Guid id) => _dbContext.GroupTags.Find(id);

    public List<GroupTag>? GetAll() => _dbContext.GroupTags.ToList();

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
}
