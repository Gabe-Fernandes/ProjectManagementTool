using PMT.Data.Models;

namespace PMT.Data.RepoInterfaces;

public interface IProject_AppUserRepo
{
  Task<Project_AppUser> GetByIdAsync(int id);

  bool Add(Project_AppUser Project_AppUser);

  bool Update(Project_AppUser Project_AppUser);

  bool Delete(Project_AppUser Project_AppUser);

  bool Save();
}
