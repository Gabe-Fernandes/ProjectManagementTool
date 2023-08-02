using PMT.Data.Models;

namespace PMT.Data.RepoInterfaces;

public interface IProjectRepo
{
  Task<IEnumerable<Project>> GetAllFromUserAsync(string appUserId);
  Task<Project> GetByIdAsync(int id);
  bool Add(Project project);
  bool Update(Project project);
  bool Delete(Project project);
  bool Save();
}
