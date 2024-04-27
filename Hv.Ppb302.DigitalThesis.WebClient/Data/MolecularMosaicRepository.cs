using Hv.Ppb302.DigitalThesis.WebClient.Models;

namespace Hv.Ppb302.DigitalThesis.WebClient.Data;

public class MolecularMosaicRepository : IRepository<MolecularMosaic>
{
    private readonly DigitalThesisDbContext _dbContext;

    public MolecularMosaicRepository(DigitalThesisDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public MolecularMosaic? Get(Guid id) => _dbContext.MolecularMosaics.Find(id);

    public List<MolecularMosaic>? GetAll() => _dbContext.MolecularMosaics.ToList();

    public void Create(MolecularMosaic molecularMosaic)
    {
        var existingMolecularMosaic = _dbContext.MolarMosaics.FirstOrDefault(m => m.Title == molecularMosaic.Title);
        if (existingMolecularMosaic != null)
        {
            throw new Exception("A molar mosaic with the same title already exists");
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
}
