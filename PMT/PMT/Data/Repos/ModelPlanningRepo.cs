using PMT.Data.RepoInterfaces;

namespace PMT.Data.Models;

public class ModelPlanningRepo : IModelPlanningRepo
{
  private readonly AppDbContext _db;

  public ModelPlanningRepo(AppDbContext db)
  {
    _db = db;
  }

  public bool Add(ModelPlanning modelPlanning)
  {
    throw new NotImplementedException();
  }

  public bool Delete(ModelPlanning modelPlanning)
  {
    throw new NotImplementedException();
  }

  public Task<ModelPlanning> GetByProjectIdAsync(int projId)
  {
    throw new NotImplementedException();
  }

  public bool Save()
  {
    throw new NotImplementedException();
  }

  public bool Update(ModelPlanning modelPlanning)
  {
    throw new NotImplementedException();
  }
}
