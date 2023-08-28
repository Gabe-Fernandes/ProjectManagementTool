using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PMT.Data.Models;

namespace PMT.Data;

public class AppDbContext : IdentityDbContext<AppUser>
{
  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
  
  public DbSet<BugReport> BugReports { get; set; }
  public DbSet<Story> Stories { get; set; }
  public DbSet<ColorPalette> ColorPalettes { get; set; }
  public DbSet<FileStructure> FileStructures { get; set; }
  public DbSet<TechStack> TechStacks { get; set; }
  public DbSet<ModelPlanning> ModelPlans { get; set; }
  public DbSet<SRS> SRSs { get; set; }
  public DbSet<Project> Projects { get; set; }

  protected override void OnModelCreating(ModelBuilder builder)
  {
    base.OnModelCreating(builder);

    builder.Entity<AppUser>()
    .Ignore(x => x.TwoFactorEnabled)
    .Ignore(x => x.AccessFailedCount)
    .Ignore(x => x.LockoutEnabled)
    .Ignore(x => x.LockoutEnd)
    .Ignore(x => x.PhoneNumber)
    .Ignore(x => x.PhoneNumberConfirmed);
  }
}
