using Microsoft.EntityFrameworkCore;
using PMT.Data.RepoInterfaces;
using PMT.Services;

namespace PMT.Data.Models;

public class BugReportRepo : IBugReportRepo
{
  private readonly AppDbContext _db;

  public BugReportRepo(AppDbContext db)
  {
    _db = db;
  }

  public bool Add(BugReport bugReport)
  {
    _db.BugReports.Add(bugReport);
    return Save();
  }

  public bool Delete(BugReport bugReport)
  {
    _db.BugReports.Remove(bugReport);
    return Save();
  }

  public async Task<IEnumerable<BugReport>> GetAllAsync(int projId)
  {
    return await _db.BugReports.Where(b => b.ProjId == projId).ToListAsync();
  }

  public async Task<IEnumerable<BugReport>> GetAllFromUserAsync(int projId, string appUserId)
  {
    // implement appUserId condition when assignment properties are added to the issue model
    return await _db.BugReports.Where(b => b.ProjId == projId).ToListAsync();
    //return await _db.BugReports.Where(b => b.ProjId == projId && b.AssignedTo == appUserId).ToListAsync();
  }

  public async Task<IEnumerable<BugReport>> GetAllUnresolvedReportsAsync(int projId)
  {
    var bugReportsFromProj = await _db.BugReports.Where(b => b.ProjId == projId).ToListAsync();
    return bugReportsFromProj.Where(b => b.Status != Str.Resolved);
  }

  public async Task<IEnumerable<BugReport>> GetAllUnresolvedReportsFromUserAsync(int projId, string appUserId)
  {
    var unresolvedBugReports = await GetAllUnresolvedReportsAsync(projId);
    // implement appUserId condition when assignment properties are added to the issue model
    return unresolvedBugReports;
    //return unresolvedBugReports.Where(b => b.AssignedTo == appUserId);
  }

  public async Task<BugReport> GetByIdAsync(int id)
  {
    return await _db.BugReports.FindAsync(id);
  }

  public bool Save()
  {
    int numSaved = _db.SaveChanges(); // returns the number of entries written to the database
    return numSaved > 0;
  }

  public bool Update(BugReport bugReport)
  {
    _db.BugReports.Update(bugReport);
    return Save();
  }
}
