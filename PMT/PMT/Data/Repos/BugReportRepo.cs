using PMT.Data.RepoInterfaces;

namespace PMT.Data.Models;

public class BugReportRepo : IBugReportRepo
{
  private readonly AppDbContext _db;

  public BugReportRepo(AppDbContext db)
  {
    _db = db;
  }

  public bool Add(BugReport appUser)
  {
    throw new NotImplementedException();
  }

  public bool Delete(BugReport appUser)
  {
    throw new NotImplementedException();
  }

  public Task<IEnumerable<BugReport>> GetAllAsync(int projId)
  {
    throw new NotImplementedException();
  }

  public Task<IEnumerable<BugReport>> GetAllFromUserAsync(int projId, string appUserId)
  {
    throw new NotImplementedException();
  }

  public Task<IEnumerable<BugReport>> GetAllUnresolvedReportsAsync(int projId)
  {
    throw new NotImplementedException();
  }

  public Task<IEnumerable<BugReport>> GetAllUnresolvedReportsFromUserAsync(int projId, string appUserId)
  {
    throw new NotImplementedException();
  }

  public Task<BugReport> GetByIdAsync(int id)
  {
    throw new NotImplementedException();
  }

  public bool Save()
  {
    throw new NotImplementedException();
  }

  public bool Update(BugReport appUser)
  {
    throw new NotImplementedException();
  }
}
