namespace Hv.Ppb302.DigitalThesis.WebClient.Models;

public class YearlyVisit
{
    public Guid Id { get; set; } = new();
    public int? Year { get; set; }
    public int? Visits { get; set; }

    // Navigation property
    public ICollection<MonthlyVisit> MonthlyVisits { get; set; } = new List<MonthlyVisit>();
}