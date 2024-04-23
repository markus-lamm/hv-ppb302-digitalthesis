using Microsoft.EntityFrameworkCore;

namespace Hv.Ppb302.DigitalThesis.WebClient.Models
{
    public class DigitalThesisContext : DbContext
    {
        public DigitalThesisContext(DbContextOptions<DigitalThesisContext> options) : base (options) { }
        
        public DbSet<Tag> Tags { get; set; }
        public DbSet<MolarMosaics> MolarMosaics { get; set; }
        public DbSet<MolecularMosaic> MolecularMosaics { get; set; }    
        public DbSet<GeoTag> GeoTag { get; set; }
    }
}
