using PMT.Data.Models;

namespace PMT.Data.RepoInterfaces;

public interface IBugReportRepo
{
  Task<IEnumerable<BugReport>> GetAllAsync(int projId, string filterString);
  Task<IEnumerable<BugReport>> GetAllUnresolvedReportsAsync(int projId, string filterString);
  Task<IEnumerable<BugReport>> GetAllFromUserAsync(int projId, string appUserId, string filterString);
  Task<IEnumerable<BugReport>> GetAllUnresolvedReportsFromUserAsync(int projId, string appUserId, string filterString);
  Task<IEnumerable<BugReport>> GetAllResolved(int projId);
  Task<BugReport> GetByIdAsync(int id);
  bool Add(BugReport appUser);
  bool Update(BugReport appUser);
  bool Delete(BugReport appUser);
  bool Save();
}
