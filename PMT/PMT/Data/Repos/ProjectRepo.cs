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

  public async Task<IEnumerable<Project>> GetAllFromUserAsync(string appUserId)
  {
    List<Project_AppUser> paRecords = await _db.Project_AppUsers.Where(pa => pa.AppUserId == appUserId).ToListAsync();
    List<Project> projectsFromUser = [];

    for (int i = 0; i < paRecords.Count; i++)
    {
      projectsFromUser.Add(await GetByIdAsync(paRecords[i].ProjId));
    }

    return projectsFromUser;
  }

  public async Task<Project> GetByIdAsync(int id)
  {
    return await _db.Projects.FindAsync(id);
  }

  public async Task<Project> GetDuplicateProject(string joinCode)
  {
    return await _db.Projects.Where(p => p.JoinCode == joinCode).FirstOrDefaultAsync();
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
