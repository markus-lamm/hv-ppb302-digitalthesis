﻿using Hv.Ppb302.DigitalThesis.WebClient.Models;
using Microsoft.EntityFrameworkCore;

namespace Hv.Ppb302.DigitalThesis.WebClient.Data;

public class DigitalThesisDbContext : DbContext
{
    public DigitalThesisDbContext(DbContextOptions<DigitalThesisDbContext> options) : base(options) { }

    public DbSet<GeoTag> GeoTags { get; set; }
    public DbSet<GroupTag> GroupTags { get; set; }
    public DbSet<MolarMosaic> MolarMosaics { get; set; }
    public DbSet<MolecularMosaic> MolecularMosaics { get; set; }
}