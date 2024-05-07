using Hv.Ppb302.DigitalThesis.WebClient.Models;
using Microsoft.EntityFrameworkCore;

namespace Hv.Ppb302.DigitalThesis.WebClient.Data
{
    public class KaleidoscopeMosaicRepository : IRepository<KaleidoscopeMosaic>
    {
        private readonly DigitalThesisDbContext _dbContext;

        public KaleidoscopeMosaicRepository(DigitalThesisDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public KaleidoscopeMosaic? Get(Guid id)
        {
            return _dbContext.KaleidoscopeMosaics
                .Include(g => g.ConnectorTags)
                .FirstOrDefault(g => g.Id == id);
        }

        public List<KaleidoscopeMosaic>? GetAll()
        {
            return _dbContext.KaleidoscopeMosaics
                .Include(g => g.ConnectorTags)
                .ToList();
        }

        public void Create(KaleidoscopeMosaic KaleidoscopeMosaic)
        {
            var existingKaleidoscopeMosaic = _dbContext.KaleidoscopeMosaics.FirstOrDefault(m => m.Title == KaleidoscopeMosaic.Title);
            if (existingKaleidoscopeMosaic != null)
            {
                throw new Exception("A kaleidoscope mosaic with the same title already exists");
            }
            _dbContext.KaleidoscopeMosaics.Add(KaleidoscopeMosaic);
            _dbContext.SaveChanges();
        }

        public void Update(KaleidoscopeMosaic KaleidoscopeMosaic)
        {
            var existingKaleidoscopeMosaic = _dbContext.KaleidoscopeMosaics.Find(KaleidoscopeMosaic.Id);
            if (existingKaleidoscopeMosaic == null)
            {
                throw new Exception("The kaleidoscope mosaic does not exist");
            }
            existingKaleidoscopeMosaic.Title = KaleidoscopeMosaic.Title;
            existingKaleidoscopeMosaic.Content = KaleidoscopeMosaic.Content;
            existingKaleidoscopeMosaic.PdfFilePath = KaleidoscopeMosaic.PdfFilePath;
            existingKaleidoscopeMosaic.HasAudio = KaleidoscopeMosaic.HasAudio;
            existingKaleidoscopeMosaic.AudioFilePath = KaleidoscopeMosaic.AudioFilePath;
            _dbContext.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var existingKaleidoscopeMosaic = _dbContext.KaleidoscopeMosaics.Find(id);
            if (existingKaleidoscopeMosaic == null)
            {
                throw new Exception("The kaleidoscope mosaic does not exist");
            }
            _dbContext.KaleidoscopeMosaics.Remove(existingKaleidoscopeMosaic);
            _dbContext.SaveChanges();
        }

        public void DeleteAllByTitle(string title)
        {
            var existingKaleidoscopeMosaic = _dbContext.KaleidoscopeMosaics
                .Where(g => g.Title!.Contains(title))
                .ToList();
            if (existingKaleidoscopeMosaic.Count == 0)
            {
                return;
            }
            _dbContext.KaleidoscopeMosaics.RemoveRange(existingKaleidoscopeMosaic);
            _dbContext.SaveChanges();
        }

        public void AddConnectorTag(Guid KaleidoscopeMosaicId, Guid connectorTagId)
        {
            var KaleidoscopeMosaic = _dbContext.KaleidoscopeMosaics.Find(KaleidoscopeMosaicId);
            if (KaleidoscopeMosaic == null)
            {
                throw new Exception("The kaleidoscope mosaic does not exist");
            }

            var connectorTag = _dbContext.ConnectorTags.Find(connectorTagId);
            if (connectorTag == null)
            {
                throw new Exception("The connector tag does not exist");
            }

            KaleidoscopeMosaic.ConnectorTags.Add(connectorTag);
            connectorTag.KaleidoscopeMosaics.Add(KaleidoscopeMosaic);
            _dbContext.SaveChanges();
        }

        public void RemoveConnectorTag(Guid KaleidoscopeMosaicId, Guid connectorTagId)
        {
            var KaleidoscopeMosaic = _dbContext.KaleidoscopeMosaics.Find(KaleidoscopeMosaicId);
            if (KaleidoscopeMosaic == null)
            {
                throw new Exception("The kaleidoscope mosaic does not exist");
            }

            var connectorTag = _dbContext.ConnectorTags.Find(connectorTagId);
            if (connectorTag == null)
            {
                throw new Exception("The connector tag does not exist");
            }

            KaleidoscopeMosaic.ConnectorTags.Remove(connectorTag);
            connectorTag.KaleidoscopeMosaics.Remove(KaleidoscopeMosaic);
            _dbContext.SaveChanges();
        }
    }
}
