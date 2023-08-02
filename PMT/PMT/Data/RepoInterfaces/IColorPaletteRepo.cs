using PMT.Data.Models;

namespace PMT.Data.RepoInterfaces;

public interface IColorPaletteRepo
{
  Task<ColorPalette> GetByProjectIdAsync(int projId);
  bool Add(ColorPalette colorPalette);
  bool Update(ColorPalette colorPalette);
  bool Delete(ColorPalette colorPalette);
  bool Save();
}
