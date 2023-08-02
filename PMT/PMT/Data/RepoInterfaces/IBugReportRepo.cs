﻿using PMT.Data.Models;

namespace PMT.Data.RepoInterfaces;

public interface IBugReportRepo
{
  Task<IEnumerable<BugReport>> GetAllAsync(int projId);
  Task<IEnumerable<BugReport>> GetAllUnresolvedReportsAsync(int projId);
  Task<IEnumerable<BugReport>> GetAllFromUserAsync(int projId, string appUserId);
  Task<IEnumerable<BugReport>> GetAllUnresolvedReportsFromUserAsync(int projId, string appUserId);
  Task<BugReport> GetByIdAsync(int id);
  bool Add(BugReport appUser);
  bool Update(BugReport appUser);
  bool Delete(BugReport appUser);
  bool Save();
}
