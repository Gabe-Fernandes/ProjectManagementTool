using PMT.Data.RepoInterfaces;

namespace PMT.Data.Models;

public class FileStructureRepo : IFileStructureRepo
{
  private readonly AppDbContext _db;

  public FileStructureRepo(AppDbContext db)
  {
    _db = db;
  }

  public bool Add(FileStructure fileStructure)
  {
    throw new NotImplementedException();
  }

  public bool Delete(FileStructure fileStructure)
  {
    throw new NotImplementedException();
  }

  public Task<FileStructure> GetByProjectIdAsync(int projId)
  {
    throw new NotImplementedException();
  }

  public bool Save()
  {
    throw new NotImplementedException();
  }

  public bool Update(FileStructure fileStructure)
  {
    throw new NotImplementedException();
  }
}
