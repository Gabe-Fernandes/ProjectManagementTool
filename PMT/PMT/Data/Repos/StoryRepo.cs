using PMT.Data.RepoInterfaces;

namespace PMT.Data.Models;

public class StoryRepo : IStoryRepo
{
  private readonly AppDbContext _db;

  public StoryRepo(AppDbContext db)
  {
    _db = db;
  }

  public bool Add(Story story)
  {
    throw new NotImplementedException();
  }

  public bool Delete(Story story)
  {
    throw new NotImplementedException();
  }

  public Task<IEnumerable<Story>> GetAllFromUserAsync(int projId, string appUserId)
  {
    throw new NotImplementedException();
  }

  public Task<IEnumerable<Story>> GetAllUnresolvedStoriesFromUserAsync(int projId, string appUserId)
  {
    throw new NotImplementedException();
  }

  public Task<IEnumerable<Story>> GetAllUnresolvedStoriesWithSearchFilterAsync(int projId, string filterString)
  {
    throw new NotImplementedException();
  }

  public Task<IEnumerable<Story>> GetAllWithSearchFilterAsync(int projId, string filterString)
  {
    throw new NotImplementedException();
  }

  public Task<Story> GetByIdAsync(int id)
  {
    throw new NotImplementedException();
  }

  public bool Save()
  {
    throw new NotImplementedException();
  }

  public bool Update(Story story)
  {
    throw new NotImplementedException();
  }
}
