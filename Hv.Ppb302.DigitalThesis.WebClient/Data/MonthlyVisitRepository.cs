using Hv.Ppb302.DigitalThesis.WebClient.Models;

namespace Hv.Ppb302.DigitalThesis.WebClient.Data;

public class MonthlyVisitRepository(DigitalThesisDbContext dbContext) : IRepository<MonthlyVisit>
{
    public MonthlyVisit? Get(Guid id)
    {
        try
        {
            return dbContext.MonthlyVisits.FirstOrDefault(g => g.Id == id);
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
            return dbContext.MonthlyVisits.FirstOrDefault(g => g.Month == month && g.YearlyVisit.Year == year);
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
            return dbContext.MonthlyVisits.ToList();
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
            var existingEntry = dbContext.MonthlyVisits.FirstOrDefault(m => m.Id == monthlyVisit.Id);
            if (existingEntry != null)
            {
                throw new Exception("An entry with the same Id already exists");
            }
            existingEntry = dbContext.MonthlyVisits.FirstOrDefault(m => m.Month == monthlyVisit.Month);
            if (existingEntry != null)
            {
                throw new Exception("An entry with the same Month already exists");
            }
            dbContext.MonthlyVisits.Add(monthlyVisit);
            dbContext.SaveChanges();
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
            var existingEntry = dbContext.MonthlyVisits.Find(monthlyVisit.Id);
            if (existingEntry == null)
            {
                throw new Exception("The entry does not exist");
            }
            existingEntry.Visits = monthlyVisit.Visits;
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
            var existingEntry = dbContext.MonthlyVisits.Find(id);
            if (existingEntry == null)
            {
                throw new Exception("The entry does not exist");
            }
            dbContext.MonthlyVisits.Remove(existingEntry);
            dbContext.SaveChanges();
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }
}
