using PMT.Data.RepoInterfaces;

namespace PMT.Data.Models;

public class AppUserRepo : IAppUserRepo
{
  private readonly AppDbContext _db;

  public AppUserRepo(AppDbContext db)
  {
      _db = db;
  }

  public bool Add(AppUser appUser)
  {
    throw new NotImplementedException();
  }

  public bool Delete(AppUser appUser)
  {
    throw new NotImplementedException();
  }

  public Task<IEnumerable<AppUser>> GetAllWithSearchFilterAsync(string filter)
  {
    throw new NotImplementedException();
  }

  public AppUser GetById(string id)
  {
    throw new NotImplementedException();
  }

  public Task<AppUser> GetByIdAsync(string id)
  {
    throw new NotImplementedException();
  }

  public bool Save()
  {
    throw new NotImplementedException();
  }

  public bool Update(AppUser appUser)
  {
    throw new NotImplementedException();
  }
}
