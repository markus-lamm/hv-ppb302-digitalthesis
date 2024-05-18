using Hv.Ppb302.DigitalThesis.WebClient.Models;
using Microsoft.EntityFrameworkCore;

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
        return _dbContext.Pages.Find(id);
    }

    public List<Page>? GetAll()
    {
        return _dbContext.Pages.ToList();
    }

    public Page? GetByName(string name)
    {
        return _dbContext.Pages.FirstOrDefault(g => g.Name == name);
    }

    public void Create(Page page)
    {
        var existingPage = _dbContext.Pages.FirstOrDefault(g => g.Name == page.Name);
        if (existingPage != null)
        {
            throw new Exception("A page with the same name already exists");
        }
        _dbContext.Pages.Add(page);
        _dbContext.SaveChanges();
    }

    public void Update(Page page)
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

    public void Delete(Guid id)
    {
        var existingPage = _dbContext.Pages.Find(id);
        if (existingPage == null)
        {
            throw new Exception("The page does not exist");
        }
        _dbContext.Pages.Remove(existingPage);
        _dbContext.SaveChanges();
    }
}
