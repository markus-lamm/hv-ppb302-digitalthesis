using Hv.Ppb302.DigitalThesis.WebClient.Models;

namespace Hv.Ppb302.DigitalThesis.WebClient.Data;

public class PageRepository : IRepository<Page>
{
    private readonly DigitalThesisDbContext _dbContext;

    public PageRepository(DigitalThesisDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Page? Get(Guid id)
    {
        try
        {
            return _dbContext.Pages.Find(id);
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
            return _dbContext.Pages.ToList();
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
            return _dbContext.Pages.FirstOrDefault(g => g.Name == name);
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
            var existingPage = _dbContext.Pages.FirstOrDefault(g => g.Name == page.Name);
            if (existingPage != null)
            {
                throw new Exception("A page with the same name already exists");
            }
            _dbContext.Pages.Add(page);
            _dbContext.SaveChanges();
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
            var existingPage = _dbContext.Pages.Find(page.Id);
            if (existingPage == null)
            {
                throw new Exception("The page does not exist");
            }
            existingPage.Name = page.Name;
            existingPage.Content = page.Content;
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
            var existingPage = _dbContext.Pages.Find(id);
            if (existingPage == null)
            {
                throw new Exception("The page does not exist");
            }
            _dbContext.Pages.Remove(existingPage);
            _dbContext.SaveChanges();
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }
}
