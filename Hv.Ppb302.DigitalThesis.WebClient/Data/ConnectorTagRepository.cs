using Hv.Ppb302.DigitalThesis.WebClient.Models;
using Microsoft.EntityFrameworkCore;

namespace Hv.Ppb302.DigitalThesis.WebClient.Data;

public class ConnectorTagRepository : IRepository<ConnectorTag>
{
    private readonly DigitalThesisDbContext _dbContext;

    public ConnectorTagRepository(DigitalThesisDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public ConnectorTag? Get(Guid id)
    {
        return _dbContext.ConnectorTags
            .Include(g => g.GeoTags)
            .Include(g => g.MolarMosaics)
            .Include(g => g.MolecularMosaics)
            .FirstOrDefault(g => g.Id == id);
    }

    public List<ConnectorTag>? GetAll()
    {
        return _dbContext.ConnectorTags
            .Include(g => g.GeoTags)
            .Include(g => g.MolarMosaics)
            .Include(g => g.MolecularMosaics)
            .ToList();
    }

    public void Create(ConnectorTag connectorTag)
    {
        var existingConnectorTag = _dbContext.ConnectorTags.FirstOrDefault(g => g.Name == connectorTag.Name);
        if (existingConnectorTag != null)
        {
            throw new Exception("A connector tag with the same name already exists");
        }
        _dbContext.ConnectorTags.Add(connectorTag);
        _dbContext.SaveChanges();
    }

    public void Update(ConnectorTag connectorTag)
    {
        var existingConnectorTag = _dbContext.ConnectorTags.Find(connectorTag.Id);
        if (existingConnectorTag == null)
        {
            throw new Exception("The connector tag does not exist");
        }
        existingConnectorTag.Name = connectorTag.Name;
        existingConnectorTag.MolarMosaics = connectorTag.MolarMosaics;
        existingConnectorTag.MolecularMosaics = connectorTag.MolecularMosaics;
        _dbContext.SaveChanges();
    }

    public void Delete(Guid id)
    {
        var existingConnectorTag = _dbContext.ConnectorTags.Find(id);
        if (existingConnectorTag == null)
        {
            throw new Exception("The connector tag does not exist");
        }
        _dbContext.ConnectorTags.Remove(existingConnectorTag);
        _dbContext.SaveChanges();
    }

    public void DeleteAllByName(string name)
    {
        var existingConnectorTags = _dbContext.ConnectorTags
            .Where(g => g.Name!.Contains(name))
            .ToList();
        if (existingConnectorTags.Count == 0)
        {
            return;
        }
        _dbContext.ConnectorTags.RemoveRange(existingConnectorTags);
        _dbContext.SaveChanges();
    }
}
