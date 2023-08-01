using PMT.Data.RepoInterfaces;

namespace PMT.Data.Models;

public class ProjectRepo : IProjectRepo
{
  private readonly AppDbContext _db;

  public ProjectRepo(AppDbContext db)
  {
    _db = db;
  }

  public bool Add(Project project)
  {
    throw new NotImplementedException();
  }

  public bool Delete(Project project)
  {
    throw new NotImplementedException();
  }

  public Task<IEnumerable<Project>> GetAllFromUserAsync(string appUserId)
  {
    throw new NotImplementedException();
  }

  public Task<Project> GetByIdAsync(int id)
  {
    throw new NotImplementedException();
  }

  public bool Save()
  {
    throw new NotImplementedException();
  }

  public bool Update(Project project)
  {
    throw new NotImplementedException();
  }
}
