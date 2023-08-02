using Microsoft.EntityFrameworkCore;
using PMT.Data.RepoInterfaces;

namespace PMT.Data.Models;

public class SRSRepo : ISRSRepo
{
  private readonly AppDbContext _db;

  public SRSRepo(AppDbContext db)
  {
    _db = db;
  }

  public bool Add(SRS SRS)
  {
    _db.SRSs.Add(SRS);
    return Save();
  }

  public bool Delete(SRS SRS)
  {
    _db.SRSs.Remove(SRS);
    return Save();
  }

  public async Task<SRS> GetByProjectIdAsync(int projId)
  {
    // There is always one per project, so the element at index 0 will always be the only element
    var SRSAsList = await _db.SRSs.Where(s => s.ProjId == projId).ToListAsync();
    return (SRSAsList.Count > 0) ? SRSAsList[0] : null;
  }

  public bool Save()
  {
    int numSaved = _db.SaveChanges(); // returns the number of entries written to the database
    return numSaved > 0;
  }

  public bool Update(SRS SRS)
  {
    _db.SRSs.Update(SRS);
    return Save();
  }
}
