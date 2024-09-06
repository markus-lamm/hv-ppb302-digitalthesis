using Hv.Ppb302.DigitalThesis.WebClient.Models;
using Microsoft.EntityFrameworkCore;

namespace Hv.Ppb302.DigitalThesis.WebClient.Data;

public class YearlyVisitRepository : IRepository<YearlyVisit>
{
    private readonly DigitalThesisDbContext _dbContext;

    public YearlyVisitRepository(DigitalThesisDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public YearlyVisit? Get(Guid id)
    {
        try
        {
            return _dbContext.YearlyVisits.FirstOrDefault(g => g.Id == id);
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }

    public YearlyVisit? GetByYear(int year)
    {
        try
        {
            return _dbContext.YearlyVisits.FirstOrDefault(g => g.Year == year);
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }

    public List<YearlyVisit> GetAll()
    {
        try
        {
            return _dbContext.YearlyVisits.Include(y => y.MonthlyVisits).ToList();
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }

    public void Create(YearlyVisit yearlyVisit)
    {
        try
        {
            var existingEntry = _dbContext.YearlyVisits.FirstOrDefault(m => m.Id == yearlyVisit.Id);
            if (existingEntry != null)
            {
                throw new Exception("An entry with the same Id already exists");
            }
            existingEntry = _dbContext.YearlyVisits.FirstOrDefault(m => m.Year == yearlyVisit.Year);
            if (existingEntry != null)
            {
                throw new Exception("An entry with the same Year already exists");
            }
            _dbContext.YearlyVisits.Add(yearlyVisit);
            _dbContext.SaveChanges();
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }

    public void Update(YearlyVisit yearlyVisit)
    {
        try
        {
            var existingEntry = _dbContext.YearlyVisits.Find(yearlyVisit.Id);
            if (existingEntry == null)
            {
                throw new Exception("The entry does not exist");
            }
            existingEntry.Visits = yearlyVisit.Visits;
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
            var existingEntry = _dbContext.YearlyVisits.Find(id);
            if (existingEntry == null)
            {
                throw new Exception("The entry does not exist");
            }
            _dbContext.YearlyVisits.Remove(existingEntry);
            _dbContext.SaveChanges();
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }
}
