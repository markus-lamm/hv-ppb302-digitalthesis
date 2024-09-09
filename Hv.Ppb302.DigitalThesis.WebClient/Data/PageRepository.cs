using Hv.Ppb302.DigitalThesis.WebClient.Models;

namespace Hv.Ppb302.DigitalThesis.WebClient.Data;

public class PageRepository(DigitalThesisDbContext dbContext) : IRepository<Page>
{
    public Page? Get(Guid id)
    {
        try
        {
            return dbContext.Pages.Find(id);
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }

    public List<Page>? GetAll()
    {
        try
        {
            return dbContext.Pages.ToList();
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }

    public Page? GetByName(string name)
    {
        try
        {
            return dbContext.Pages.FirstOrDefault(g => g.Name == name);
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }

    public void Create(Page page)
    {
        try
        {
            var existingPage = dbContext.Pages.FirstOrDefault(g => g.Name == page.Name);
            if (existingPage != null)
            {
                throw new Exception("A page with the same name already exists");
            }
            dbContext.Pages.Add(page);
            dbContext.SaveChanges();
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }

    public void Update(Page page)
    {
        try
        {
            var existingPage = dbContext.Pages.Find(page.Id);
            if (existingPage == null)
            {
                throw new Exception("The page does not exist");
            }
            existingPage.Name = page.Name;
            existingPage.Content = page.Content;
            dbContext.SaveChanges();
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
            var existingPage = dbContext.Pages.Find(id);
            if (existingPage == null)
            {
                throw new Exception("The page does not exist");
            }
            dbContext.Pages.Remove(existingPage);
            dbContext.SaveChanges();
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }
}
