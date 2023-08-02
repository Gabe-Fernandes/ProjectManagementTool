using Microsoft.EntityFrameworkCore;
using PMT.Data.RepoInterfaces;

namespace PMT.Data.Models;

public class FileStructureRepo : IFileStructureRepo
{
  private readonly AppDbContext _db;

  public FileStructureRepo(AppDbContext db)
  {
    _db = db;
  }

  public bool Add(FileStructure fileStructure)
  {
    _db.FileStructures.Add(fileStructure);
    return Save();
  }

  public bool Delete(FileStructure fileStructure)
  {
    _db.FileStructures.Remove(fileStructure);
    return Save();
  }

  public async Task<FileStructure> GetByProjectIdAsync(int projId)
  {
    // There is always one per project, so the element at index 0 will always be the only element
    var fileStructureAsList = await _db.FileStructures.Where(f => f.ProjId == projId).ToListAsync();
    return (fileStructureAsList.Count > 0) ? fileStructureAsList[0] : null;
  }

  public bool Save()
  {
    int numSaved = _db.SaveChanges(); // returns the number of entries written to the database
    return numSaved > 0;
  }

  public bool Update(FileStructure fileStructure)
  {
    _db.FileStructures.Update(fileStructure);
    return Save();
  }
}
