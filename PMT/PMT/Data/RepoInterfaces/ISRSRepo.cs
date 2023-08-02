using PMT.Data.Models;

namespace PMT.Data.RepoInterfaces;

public interface ISRSRepo
{
  Task<SRS> GetByProjectIdAsync(int projId);
  bool Add(SRS SRS);
  bool Update(SRS SRS);
  bool Delete(SRS SRS);
  bool Save();
}
