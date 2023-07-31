namespace PMT.Data.Models;

public class FileStructureRepo
{
  private readonly AppDbContext _db;

  public FileStructureRepo(AppDbContext db)
  {
    _db = db;
  }
}
