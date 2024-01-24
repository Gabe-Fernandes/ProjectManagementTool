using Microsoft.EntityFrameworkCore;
using PMT.Data.RepoInterfaces;

namespace PMT.Data.Models;

public class ProjectRepo : IProjectRepo
{
  private readonly AppDbContext _db;

  public ProjectRepo(AppDbContext db)
  {
    _db = db;
  }

  public async Task<Project> GetDuplicateProject(string projCode)
  {
    return await _db.Projects.Where(p => p.JoinCode == projCode).FirstOrDefaultAsync();
  }

  public bool Add(Project project)
  {
    _db.Projects.Add(project);
    return Save();
  }

  public bool Delete(Project project)
  {
    _db.Projects.Remove(project);
    return Save();
  }

  public async Task<IEnumerable<Project>> GetAllFromUserAsync(string appUserId)
  {
    // many to many relationship needs to be established first
    // return all for now
    return await _db.Projects.ToListAsync();
  }

  public async Task<Project> GetByIdAsync(int id)
  {
    return await _db.Projects.FindAsync(id);
  }

  public bool Save()
  {
    int numSaved = _db.SaveChanges(); // returns the number of entries written to the database
    return numSaved > 0;
  }

  public bool Update(Project project)
  {
    _db.Projects.Update(project);
    return Save();
  }
}
