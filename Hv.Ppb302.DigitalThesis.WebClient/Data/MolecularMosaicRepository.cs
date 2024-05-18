﻿using Hv.Ppb302.DigitalThesis.WebClient.Models;
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
            .Include(g => g.ConnectorTags)
            .FirstOrDefault(g => g.Id == id);
    }

    public List<MolecularMosaic>? GetAll()
    {
        return _dbContext.MolecularMosaics
            .Include(g => g.ConnectorTags)
            .ToList();
    }

    public void Create(MolecularMosaic molecularMosaic)
    {
        var existingMolecularMosaic = _dbContext.MolarMosaics.FirstOrDefault(m => m.Title == molecularMosaic.Title);
        if (existingMolecularMosaic != null)
        {
            throw new Exception("A molecular mosaic with the same title already exists");
        }
        _dbContext.MolecularMosaics.Add(molecularMosaic);
        _dbContext.SaveChanges();
    }

    public void Update(MolecularMosaic molecularMosaic)
    {
        var existingMolecularMosaic = _dbContext.MolecularMosaics.Find(molecularMosaic.Id);
        if (existingMolecularMosaic == null)
        {
            throw new Exception("The molecular mosaic does not exist");
        }
        existingMolecularMosaic.Title = molecularMosaic.Title;
        existingMolecularMosaic.Content = molecularMosaic.Content;
        existingMolecularMosaic.PdfFilePath = molecularMosaic.PdfFilePath;
        existingMolecularMosaic.HasAudio = molecularMosaic.HasAudio;
        existingMolecularMosaic.AudioFilePath = molecularMosaic.AudioFilePath;
        existingMolecularMosaic.Becomings = molecularMosaic.Becomings;
        existingMolecularMosaic.AssemblageTag = molecularMosaic.AssemblageTag;
        _dbContext.SaveChanges();
    }

    public void Delete(Guid id)
    {
        var existingMolecularMosaic = _dbContext.MolecularMosaics.Find(id);
        if (existingMolecularMosaic == null)
        {
            throw new Exception("The molecular mosaic does not exist");
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

    public void AddConnectorTag(Guid molecularMosaicId, Guid connectorTagId)
    {
        var molecularMosaic = _dbContext.MolecularMosaics.Find(molecularMosaicId);
        if (molecularMosaic == null)
        {
            throw new Exception("The molecular mosaic does not exist");
        }

        var connectorTag = _dbContext.ConnectorTags.Find(connectorTagId);
        if (connectorTag == null)
        {
            throw new Exception("The connector tag does not exist");
        }

        molecularMosaic.ConnectorTags.Add(connectorTag);
        connectorTag.MolecularMosaics.Add(molecularMosaic);
        _dbContext.SaveChanges();
    }

    public void AddKaleidoscopeTag(Guid molecularMosaicId, Guid kaleidoscopeTagId)
    {
        var molecularMosaic = _dbContext.MolecularMosaics.Find(molecularMosaicId);
        if (molecularMosaic == null)
        {
            throw new Exception("The molecular mosaic does not exist");
        }

        var kaleidoscopeTag = _dbContext.KaleidoscopeTags.Find(kaleidoscopeTagId);
        if (kaleidoscopeTag == null)
        {
            throw new Exception("The kaleidoscope tag does not exist");
        }

        molecularMosaic.KaleidoscopeTags.Add(kaleidoscopeTag);
        kaleidoscopeTag.MolecularMosaics.Add(molecularMosaic);
        _dbContext.SaveChanges();
    }

    public void RemoveConnectorTag(Guid molecularMosaicId, Guid connectorTagId)
    {
        var molecularMosaic = _dbContext.MolecularMosaics.Find(molecularMosaicId);
        if (molecularMosaic == null)
        {
            throw new Exception("The molecular mosaic does not exist");
        }

        var connectorTag = _dbContext.ConnectorTags.Find(connectorTagId);
        if (connectorTag == null)
        {
            throw new Exception("The connector tag does not exist");
        }

        molecularMosaic.ConnectorTags.Remove(connectorTag);
        connectorTag.MolecularMosaics.Remove(molecularMosaic);
        _dbContext.SaveChanges();
    }
}
