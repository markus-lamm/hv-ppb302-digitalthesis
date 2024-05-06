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
                .Include(g => g.GroupTags)
                .FirstOrDefault(g => g.Id == id);
        }

        public List<KaleidoscopeMosaic>? GetAll()
        {
            return _dbContext.KaleidoscopeMosaics
                .Include(g => g.GroupTags)
                .ToList();
        }

        public void Create(KaleidoscopeMosaic KaleidoscopeMosaic)
        {
            var existingKaleidoscopeMosaic = _dbContext.KaleidoscopeMosaics.FirstOrDefault(m => m.Title == KaleidoscopeMosaic.Title);
            if (existingKaleidoscopeMosaic != null)
            {
                throw new Exception("A molar mosaic with the same title already exists");
            }
            _dbContext.KaleidoscopeMosaics.Add(KaleidoscopeMosaic);
            _dbContext.SaveChanges();
        }

        public void Update(KaleidoscopeMosaic KaleidoscopeMosaic)
        {
            var existingKaleidoscopeMosaic = _dbContext.KaleidoscopeMosaics.Find(KaleidoscopeMosaic.Id);
            if (existingKaleidoscopeMosaic == null)
            {
                throw new Exception("The molar mosaic does not exist");
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
                throw new Exception("The molar mosaic does not exist");
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

        public void AddGroupTag(Guid KaleidoscopeMosaicId, Guid groupTagId)
        {
            var KaleidoscopeMosaic = _dbContext.KaleidoscopeMosaics.Find(KaleidoscopeMosaicId);
            if (KaleidoscopeMosaic == null)
            {
                throw new Exception("The molar mosaic does not exist");
            }

            var groupTag = _dbContext.GroupTags.Find(groupTagId);
            if (groupTag == null)
            {
                throw new Exception("The group tag does not exist");
            }

            KaleidoscopeMosaic.GroupTags.Add(groupTag);
            groupTag.KaleidoscopeMosaics.Add(KaleidoscopeMosaic);
            _dbContext.SaveChanges();
        }

        public void RemoveGroupTag(Guid KaleidoscopeMosaicId, Guid groupTagId)
        {
            var KaleidoscopeMosaic = _dbContext.KaleidoscopeMosaics.Find(KaleidoscopeMosaicId);
            if (KaleidoscopeMosaic == null)
            {
                throw new Exception("The molar mosaic does not exist");
            }

            var groupTag = _dbContext.GroupTags.Find(groupTagId);
            if (groupTag == null)
            {
                throw new Exception("The group tag does not exist");
            }

            KaleidoscopeMosaic.GroupTags.Remove(groupTag);
            groupTag.KaleidoscopeMosaics.Remove(KaleidoscopeMosaic);
            _dbContext.SaveChanges();
        }
    }
}
