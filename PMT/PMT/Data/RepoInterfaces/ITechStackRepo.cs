using PMT.Data.Models;

namespace PMT.Data.RepoInterfaces;

public interface ITechStackRepo
{
  Task<TechStack> GetByProjectIdAsync(int projId);
  bool Add(TechStack techStack);
  bool Update(TechStack techStack);
  bool Delete(TechStack techStack);
  bool Save();
}
