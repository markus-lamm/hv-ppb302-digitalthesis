using Hv.Ppb302.DigitalThesis.WebClient.Models;
using Microsoft.EntityFrameworkCore;

namespace Hv.Ppb302.DigitalThesis.WebClient.Data;

public class YearlyVisitRepository(DigitalThesisDbContext dbContext) : IRepository<YearlyVisit>
{
    public YearlyVisit? Get(Guid id)
    {
        try
        {
            return dbContext.YearlyVisits.FirstOrDefault(g => g.Id == id);
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
            return dbContext.YearlyVisits.FirstOrDefault(g => g.Year == year);
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
            return dbContext.YearlyVisits.Include(y => y.MonthlyVisits).ToList();
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
            var existingEntry = dbContext.YearlyVisits.FirstOrDefault(m => m.Id == yearlyVisit.Id);
            if (existingEntry != null)
            {
                throw new Exception("An entry with the same Id already exists");
            }
            existingEntry = dbContext.YearlyVisits.FirstOrDefault(m => m.Year == yearlyVisit.Year);
            if (existingEntry != null)
            {
                throw new Exception("An entry with the same Year already exists");
            }
            dbContext.YearlyVisits.Add(yearlyVisit);
            dbContext.SaveChanges();
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
            var existingEntry = dbContext.YearlyVisits.Find(yearlyVisit.Id);
            if (existingEntry == null)
            {
                throw new Exception("The entry does not exist");
            }
            existingEntry.Visits = yearlyVisit.Visits;
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
            var existingEntry = dbContext.YearlyVisits.Find(id);
            if (existingEntry == null)
            {
                throw new Exception("The entry does not exist");
            }
            dbContext.YearlyVisits.Remove(existingEntry);
            dbContext.SaveChanges();
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }
}
