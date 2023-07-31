namespace PMT.Data.Models;

public class SRSRepo
{
  private readonly AppDbContext _db;

  public SRSRepo(AppDbContext db)
  {
    _db = db;
  }
}
