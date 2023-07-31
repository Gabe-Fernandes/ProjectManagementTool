namespace PMT.Data.Models;

public class ModelPlanningRepo
{
  private readonly AppDbContext _db;

  public ModelPlanningRepo(AppDbContext db)
  {
    _db = db;
  }
}
