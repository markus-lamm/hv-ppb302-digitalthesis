using Hv.Ppb302.DigitalThesis.WebClient.Models;
using Microsoft.EntityFrameworkCore;

namespace Hv.Ppb302.DigitalThesis.WebClient.Data;

public class DigitalThesisDbContext(DbContextOptions<DigitalThesisDbContext> options) : DbContext(options)
{
    public DbSet<GeoTag> GeoTags { get; set; }
    public DbSet<ConnectorTag> ConnectorTags { get; set; }
    public DbSet<MolarMosaic> MolarMosaics { get; set; }
    public DbSet<MolecularMosaic> MolecularMosaics { get; set; }
    public DbSet<KaleidoscopeTag> KaleidoscopeTags { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<AssemblageTag> AssemblageTags { get; set; }
    public DbSet<Page> Pages { get; set; }
    public DbSet<Upload> Uploads { get; set; }
    public DbSet<YearlyVisit> YearlyVisits { get; set; }
    public DbSet<MonthlyVisit> MonthlyVisits { get; set; }
}