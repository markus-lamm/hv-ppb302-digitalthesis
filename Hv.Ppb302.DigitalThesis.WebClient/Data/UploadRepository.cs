using Hv.Ppb302.DigitalThesis.WebClient.Models;

namespace Hv.Ppb302.DigitalThesis.WebClient.Data
{
    public class UploadRepository(DigitalThesisDbContext dbContext)
    {
        public void Create(Upload file)
        {
            try
            {
                dbContext.Uploads.Add(file);
                dbContext.SaveChanges();
            }
            catch (Exception)
            {
                throw new Exception("Internal Server Error");
            }
        }

        public void Delete(string filename)
        {
            var existingFile = dbContext.Uploads.FirstOrDefault(m => m.Name == filename);

            if (existingFile != null)
            {
                dbContext.Uploads.Remove(existingFile);
                dbContext.SaveChanges();
            }

        }

        public List<Upload>? GetAllMaterials()
        {
            try
            {
                return dbContext.Uploads.Where(u => u.IsMaterial == true).ToList();
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
                return dbContext.Uploads.ToList();
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
                var existingUpload = dbContext.Uploads.FirstOrDefault(m => m.Name == upload.Name);
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

            dbContext.SaveChanges();
        }
    }
}
