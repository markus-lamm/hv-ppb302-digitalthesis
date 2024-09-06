using Hv.Ppb302.DigitalThesis.WebClient.Models;
using Microsoft.EntityFrameworkCore;

namespace Hv.Ppb302.DigitalThesis.WebClient.Data
{
    public class UploadRepository
    {
        private readonly DigitalThesisDbContext _dbContext;

        public UploadRepository(DigitalThesisDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Create(Upload file)
        {
            try
            {
                _dbContext.Uploads.Add(file);
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {
                throw new Exception("Internal Server Error");
            }
        }

        public void Delete(string filename)
        {
            var existingFile = _dbContext.Uploads.FirstOrDefault(m => m.Name == filename);

            if (existingFile != null)
            {
                _dbContext.Uploads.Remove(existingFile);
                _dbContext.SaveChanges();
            }

        }

        public List<Upload>? GetAllMaterials()
        {
            try
            {
                return _dbContext.Uploads.Where(u => u.IsMaterial == true).ToList();
            }
            catch (Exception)
            {
                throw new Exception("Internal Server Error");
            }
        }

        public List<Upload>? GetAll()
        {
            try
            {
                return _dbContext.Uploads.ToList();
            }
            catch (Exception)
            {
                throw new Exception("Internal Server Error");
            }
        }

        public void Update(List<Upload> uploadsToUpdate)
        {
            foreach (var upload in uploadsToUpdate)
            {
                var existingUpload = _dbContext.Uploads.FirstOrDefault(m => m.Name == upload.Name);
                if (existingUpload != null)
                {
                    existingUpload.IsMaterial = upload.IsMaterial ?? existingUpload.IsMaterial;
                    existingUpload.MaterialOrder = upload.MaterialOrder ?? existingUpload.MaterialOrder;
                }
                else
                {
                    //Vid ändring av gammal fil som inte existerar i databasen
                    Create(upload);
                }
            }

            _dbContext.SaveChanges();
        }
    }
}
