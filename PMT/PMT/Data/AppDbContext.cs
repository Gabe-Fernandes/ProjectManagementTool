using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PMT.Data.Models;

namespace PMT.Data;

public class AppDbContext : IdentityDbContext<AppUser>
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
	
	// PMT Landmark
	public DbSet<TimeInterval> TimeIntervals { get; set; }
	public DbSet<TimeSet> TimeSets { get; set; }
	public DbSet<Stopwatch> Stopwatches { get; set; }
	public DbSet<BugReport> BugReports { get; set; }
	public DbSet<Story> Stories { get; set; }
	public DbSet<ColorPalette> ColorPalettes { get; set; }
	public DbSet<FileStructure> FileStructures { get; set; }
	public DbSet<TechStack> TechStacks { get; set; }
	public DbSet<ModelPlanning> ModelPlans { get; set; }
	public DbSet<SRS> SRSs { get; set; }
	public DbSet<Project> Projects { get; set; }
	public DbSet<Project_AppUser> Project_AppUsers { get; set; }

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);

		builder.Entity<AppUser>()
		.Ignore(x => x.TwoFactorEnabled)
		.Ignore(x => x.AccessFailedCount)
		.Ignore(x => x.LockoutEnabled)
		.Ignore(x => x.LockoutEnd)
		.Ignore(x => x.PhoneNumberConfirmed);
	}
} 
