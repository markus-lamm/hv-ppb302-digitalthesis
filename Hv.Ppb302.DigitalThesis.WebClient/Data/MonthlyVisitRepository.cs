using Hv.Ppb302.DigitalThesis.WebClient.Models;

namespace Hv.Ppb302.DigitalThesis.WebClient.Data;

public class MonthlyVisitRepository : IRepository<MonthlyVisit>
{
    private readonly DigitalThesisDbContext _dbContext;

    public MonthlyVisitRepository(DigitalThesisDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public MonthlyVisit? Get(Guid id)
    {
        try
        {
            return _dbContext.MonthlyVisits.FirstOrDefault(g => g.Id == id);
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }

    public MonthlyVisit? GetByMonthAndYear(int month, int year)
    {
        try
        {
            return _dbContext.MonthlyVisits.FirstOrDefault(g => g.Month == month && g.YearlyVisit.Year == year);
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }

    public List<MonthlyVisit> GetAll()
    {
        try
        {
            return _dbContext.MonthlyVisits.ToList();
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }

    public void Create(MonthlyVisit monthlyVisit)
    {
        try
        {
            var existingEntry = _dbContext.MonthlyVisits.FirstOrDefault(m => m.Id == monthlyVisit.Id);
            if (existingEntry != null)
            {
                throw new Exception("An entry with the same Id already exists");
            }
            existingEntry = _dbContext.MonthlyVisits.FirstOrDefault(m => m.Month == monthlyVisit.Month);
            if (existingEntry != null)
            {
                throw new Exception("An entry with the same Month already exists");
            }
            _dbContext.MonthlyVisits.Add(monthlyVisit);
            _dbContext.SaveChanges();
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }

    public void Update(MonthlyVisit monthlyVisit)
    {
        try
        {
            var existingEntry = _dbContext.MonthlyVisits.Find(monthlyVisit.Id);
            if (existingEntry == null)
            {
                throw new Exception("The entry does not exist");
            }
            existingEntry.Visits = monthlyVisit.Visits;
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
            var existingEntry = _dbContext.MonthlyVisits.Find(id);
            if (existingEntry == null)
            {
                throw new Exception("The entry does not exist");
            }
            _dbContext.MonthlyVisits.Remove(existingEntry);
            _dbContext.SaveChanges();
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }
}
