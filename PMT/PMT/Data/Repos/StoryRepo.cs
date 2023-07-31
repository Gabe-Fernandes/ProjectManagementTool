namespace PMT.Data.Models;

public class StoryRepo
{
  private readonly AppDbContext _db;

  public StoryRepo(AppDbContext db)
  {
    _db = db;
  }
}
