using Microsoft.EntityFrameworkCore;
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
    _db.Users.Add(appUser);
    return Save();
  }

  public bool Delete(AppUser appUser)
  {
    _db.Users.Remove(appUser);
    return Save();
  }

  public async Task<IEnumerable<AppUser>> GetAllWithSearchFilterAsync(string filter)
  {
    filter ??= string.Empty;
    filter = filter.ToUpper();
    var appUsers = await _db.Users.Where(a => (a.Firstname + " " + a.Lastname).ToUpper().Contains(filter)).ToListAsync();
    return appUsers;
  }

  public AppUser GetById(string id)
  {
    return _db.Users.Find(id);
  }

  public async Task<AppUser> GetByIdAsync(string id)
  {
    return await _db.Users.FindAsync(id);
  }

  public bool Save()
  {
    int numSaved = _db.SaveChanges(); // returns the number of entries written to the database
    return numSaved > 0;
  }

  public bool Update(AppUser appUser)
  {
    _db.Users.Update(appUser);
    return Save();
  }
}
