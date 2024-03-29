using Microsoft.EntityFrameworkCore;
using PMT.Data.RepoInterfaces;

namespace PMT.Data.Models;

public class Project_AppUserRepo(AppDbContext db) : IProject_AppUserRepo
{
  private readonly AppDbContext _db = db;

  public async Task<List<Project_AppUser>> GetAllWithAppUserId(string appUserId)
  {
    return await _db.Project_AppUsers.Where(pa => pa.AppUserId == appUserId).ToListAsync();
  }

  public async Task<List<Project_AppUser>> GetAllWithProjId(int projId)
  {
    return await _db.Project_AppUsers.Where(pa => pa.ProjId == projId).ToListAsync();
  }

  public async Task<Project_AppUser> GetByForeignKeysAsync(int projId, string appUserId)
  {
    return await _db.Project_AppUsers.Where(pa => pa.ProjId == projId && pa.AppUserId == appUserId).FirstOrDefaultAsync();
  }

  public async Task<Project_AppUser> GetByIdAsync(int id)
  {
    return await _db.Project_AppUsers.FindAsync(id);
  }

  public bool Add(Project_AppUser Project_AppUser)
  {
    _db.Project_AppUsers.Add(Project_AppUser);
    return Save();
  }

  public bool Delete(Project_AppUser Project_AppUser)
  {
    _db.Project_AppUsers.Remove(Project_AppUser);
    return Save();
  }

  public bool Update(Project_AppUser Project_AppUser)
  {
    _db.Project_AppUsers.Update(Project_AppUser);
    return Save();
  }

  public bool Save()
  {
    int numSaved = _db.SaveChanges(); // returns the number of entries written to the database
    return numSaved > 0;
  }
}
