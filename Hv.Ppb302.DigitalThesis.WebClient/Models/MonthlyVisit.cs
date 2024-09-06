namespace Hv.Ppb302.DigitalThesis.WebClient.Models;

public class MonthlyVisit
{
    public Guid Id { get; set; } = new();
    public Guid? YearlyVisitId { get; set; }
    public int? Month { get; set; }
    public int? Visits { get; set; }

    // Navigation property
    public YearlyVisit YearlyVisit { get; set; }
}