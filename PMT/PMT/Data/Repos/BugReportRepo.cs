namespace PMT.Data.Models;

public class BugReportRepo
{
  private readonly AppDbContext _db;

  public BugReportRepo(AppDbContext db)
  {
    _db = db;
  }
}
