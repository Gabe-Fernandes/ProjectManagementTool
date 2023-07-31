namespace PMT.Data.Models;

public class ProjectRepo
{
  private readonly AppDbContext _db;

  public ProjectRepo(AppDbContext db)
  {
    _db = db;
  }
}
