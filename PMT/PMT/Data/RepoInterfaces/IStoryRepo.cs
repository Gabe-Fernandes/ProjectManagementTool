using PMT.Data.Models;

namespace PMT.Data.RepoInterfaces;

public interface IStoryRepo
{
  Task<IEnumerable<Story>> GetAllWithSearchFilterAsync(int projId, string filterString);
  Task<IEnumerable<Story>> GetAllUnresolvedStoriesWithSearchFilterAsync(int projId, string filterString);
  Task<IEnumerable<Story>> GetAllFromUserAsync(int projId, string appUserId);
  Task<IEnumerable<Story>> GetAllUnresolvedStoriesFromUserAsync(int projId, string appUserId);
  Task<IEnumerable<Story>> GetAllResolved(int projId);
  Task<Story> GetByIdAsync(int id);
  bool Add(Story story);
  bool Update(Story story);
  bool Delete(Story story);
  bool Save();
}
