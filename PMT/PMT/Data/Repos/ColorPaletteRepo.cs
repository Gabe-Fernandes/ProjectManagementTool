using PMT.Data.RepoInterfaces;

namespace PMT.Data.Models;

public class ColorPaletteRepo : IColorPaletteRepo
{
  private readonly AppDbContext _db;

  public ColorPaletteRepo(AppDbContext db)
  {
    _db = db;
  }

  public bool Add(ColorPalette colorPalette)
  {
    throw new NotImplementedException();
  }

  public bool Delete(ColorPalette colorPalette)
  {
    throw new NotImplementedException();
  }

  public Task<ColorPalette> GetByProjectIdAsync(int projId)
  {
    throw new NotImplementedException();
  }

  public bool Save()
  {
    throw new NotImplementedException();
  }

  public bool Update(ColorPalette colorPalette)
  {
    throw new NotImplementedException();
  }
}
