using Microsoft.EntityFrameworkCore;
using PMT.Data.RepoInterfaces;

namespace PMT.Data.Models;

public class TechStackRepo : ITechStackRepo
{
  private readonly AppDbContext _db;

  public TechStackRepo(AppDbContext db)
  {
    _db = db;
  }

  public bool Add(TechStack techStack)
  {
    _db.TechStacks.Add(techStack);
    return Save();
  }

  public bool Delete(TechStack techStack)
  {
    _db.TechStacks.Remove(techStack);
    return Save();
  }

  public async Task<TechStack> GetByProjectIdAsync(int projId)
  {
    // There is always one per project, so the element at index 0 will always be the only element
    var techStackAsList = await _db.TechStacks.Where(t => t.ProjId == projId).ToListAsync();
    return (techStackAsList.Count > 0) ? techStackAsList[0] : null;
  }

  public bool Save()
  {
    int numSaved = _db.SaveChanges(); // returns the number of entries written to the database
    return numSaved > 0;
  }

  public bool Update(TechStack techStack)
  {
    _db.TechStacks.Update(techStack);
    return Save();
  }
}
