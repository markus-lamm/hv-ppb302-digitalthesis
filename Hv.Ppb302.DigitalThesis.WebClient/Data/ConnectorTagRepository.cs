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
        try
        {
            return _dbContext.ConnectorTags
                .Include(g => g.GeoTags)
                .Include(g => g.MolarMosaics)
                .Include(g => g.MolecularMosaics)
                .FirstOrDefault(g => g.Id == id);
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }

    public List<ConnectorTag>? GetAll()
    {
        try
        {
            return _dbContext.ConnectorTags
                .Include(g => g.GeoTags)
                .Include(g => g.MolarMosaics)
                .Include(g => g.MolecularMosaics)
                .ToList();
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }

    public void Create(ConnectorTag connectorTag)
    {
        try
        {
            var existingConnectorTag = _dbContext.ConnectorTags.FirstOrDefault(g => g.Name == connectorTag.Name);
            if (existingConnectorTag != null)
            {
                throw new Exception("A connector tag with the same name already exists");
            }
            _dbContext.ConnectorTags.Add(connectorTag);
            _dbContext.SaveChanges();
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }

    public void Update(ConnectorTag connectorTag)
    {
        try
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
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }

    public void Delete(Guid id)
    {
        try
        {
            var existingConnectorTag = _dbContext.ConnectorTags.Find(id);
            if (existingConnectorTag == null)
            {
                throw new Exception("The connector tag does not exist");
            }
            _dbContext.ConnectorTags.Remove(existingConnectorTag);
            _dbContext.SaveChanges();
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }
}
