using Hv.Ppb302.DigitalThesis.WebClient.Models;

namespace Hv.Ppb302.DigitalThesis.WebClient.Data;

public class MolarMosaicRepository : IRepository<MolarMosaic>
{
    private readonly DigitalThesisDbContext _dbContext;

    public MolarMosaicRepository(DigitalThesisDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public MolarMosaic? Get(Guid id) => _dbContext.MolarMosaics.Find(id);

    public List<MolarMosaic>? GetAll() => _dbContext.MolarMosaics.ToList();

    public void Create(MolarMosaic molarMosaic)
    {
        var existingMolarMosaic = _dbContext.MolarMosaics.FirstOrDefault(m => m.Title == molarMosaic.Title);
        if (existingMolarMosaic != null)
        {
            throw new Exception("A molar mosaic with the same title already exists");
        }
        _dbContext.MolarMosaics.Add(molarMosaic);
        _dbContext.SaveChanges();
    }

    public void Update(MolarMosaic molarMosaic)
    {
        var existingMolarMosaic = _dbContext.MolarMosaics.Find(molarMosaic.Id);
        if (existingMolarMosaic == null)
        {
            throw new Exception("The molar mosaic does not exist");
        }
        existingMolarMosaic.Title = molarMosaic.Title;
        existingMolarMosaic.Content = molarMosaic.Content;
        existingMolarMosaic.PdfFilePath = molarMosaic.PdfFilePath;
        existingMolarMosaic.HasAudio = molarMosaic.HasAudio;
        existingMolarMosaic.AudioFilePath = molarMosaic.AudioFilePath;
        _dbContext.SaveChanges();
    }

    public void Delete(Guid id)
    {
        var existingMolarMosaic = _dbContext.MolarMosaics.Find(id);
        if (existingMolarMosaic == null)
        {
            throw new Exception("The molar mosaic does not exist");
        }
        _dbContext.MolarMosaics.Remove(existingMolarMosaic);
        _dbContext.SaveChanges();
    }
}
