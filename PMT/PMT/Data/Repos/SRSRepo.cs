using PMT.Data.RepoInterfaces;

namespace PMT.Data.Models;

public class SRSRepo : ISRSRepo
{
  private readonly AppDbContext _db;

  public SRSRepo(AppDbContext db)
  {
    _db = db;
  }

  public bool Add(SRS SRS)
  {
    throw new NotImplementedException();
  }

  public bool Delete(SRS SRS)
  {
    throw new NotImplementedException();
  }

  public Task<SRS> GetByProjectIdAsync(int projId)
  {
    throw new NotImplementedException();
  }

  public bool Save()
  {
    throw new NotImplementedException();
  }

  public bool Update(SRS SRS)
  {
    throw new NotImplementedException();
  }
}
