namespace PMT.Data.Models;

public class ColorPaletteRepo
{
  private readonly AppDbContext _db;

  public ColorPaletteRepo(AppDbContext db)
  {
    _db = db;
  }
}
