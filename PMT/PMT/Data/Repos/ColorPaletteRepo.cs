using Microsoft.EntityFrameworkCore;
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
    _db.ColorPalettes.Add(colorPalette);
    return Save();
  }

  public bool Delete(ColorPalette colorPalette)
  {
    _db.ColorPalettes.Remove(colorPalette);
    return Save();
  }

  public async Task<ColorPalette> GetByProjectIdAsync(int projId)
  {
    // There is always one per project, so the element at index 0 will always be the only element
    var colorPaletteAsList = await _db.ColorPalettes.Where(c => c.ProjId == projId).ToListAsync();
    return (colorPaletteAsList.Count > 0) ? colorPaletteAsList[0] : null;
  }

  public bool Save()
  {
    int numSaved = _db.SaveChanges(); // returns the number of entries written to the database
    return numSaved > 0;
  }

  public bool Update(ColorPalette colorPalette)
  {
    _db.ColorPalettes.Update(colorPalette);
    return Save();
  }
}
