using PMT.Data.Models;

namespace PMT.Data.RepoInterfaces;

public interface IModelPlanningRepo
{
  Task<ModelPlanning> GetByProjectIdAsync(int projId);
  bool Add(ModelPlanning modelPlanning);
  bool Update(ModelPlanning modelPlanning);
  bool Delete(ModelPlanning modelPlanning);
  bool Save();
}
