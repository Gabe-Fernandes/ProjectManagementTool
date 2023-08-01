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
    throw new NotImplementedException();
  }

  public bool Delete(TechStack techStack)
  {
    throw new NotImplementedException();
  }

  public Task<TechStack> GetByProjectIdAsync(int projId)
  {
    throw new NotImplementedException();
  }

  public bool Save()
  {
    throw new NotImplementedException();
  }

  public bool Update(TechStack techStack)
  {
    throw new NotImplementedException();
  }
}
