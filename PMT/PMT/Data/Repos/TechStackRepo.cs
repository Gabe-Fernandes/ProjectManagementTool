namespace PMT.Data.Models;

public class TechStackRepo
{
  private readonly AppDbContext _db;

  public TechStackRepo(AppDbContext db)
  {
    _db = db;
  }
}
