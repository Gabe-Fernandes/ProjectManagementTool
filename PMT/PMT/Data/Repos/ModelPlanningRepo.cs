using Microsoft.EntityFrameworkCore;
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
    _db.ModelPlans.Add(modelPlanning);
    return Save();
  }

  public bool Delete(ModelPlanning modelPlanning)
  {
    _db.ModelPlans.Remove(modelPlanning);
    return Save();
  }

  public async Task<ModelPlanning> GetByProjectIdAsync(int projId)
  {
    // There is always one per project, so the element at index 0 will always be the only element
    var modelPlansAsList = await _db.ModelPlans.Where(m => m.ProjId == projId).ToListAsync();
    return (modelPlansAsList.Count > 0) ? modelPlansAsList[0] : null;
  }

  public bool Save()
  {
    int numSaved = _db.SaveChanges(); // returns the number of entries written to the database
    return numSaved > 0;
  }

  public bool Update(ModelPlanning modelPlanning)
  {
    _db.ModelPlans.Update(modelPlanning);
    return Save();
  }
}
