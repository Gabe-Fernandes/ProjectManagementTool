using PMT.Data.Models;

namespace PMT.Data.RepoInterfaces;

public interface IFileStructureRepo
{
  Task<FileStructure> GetByProjectIdAsync(int projId);
  bool Add(FileStructure fileStructure);
  bool Update(FileStructure fileStructure);
  bool Delete(FileStructure fileStructure);
  bool Save();
}
