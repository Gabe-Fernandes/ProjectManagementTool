using Microsoft.EntityFrameworkCore;
using PMT.Data.Models;
using PMT.Data;

namespace PMTTests.Data.Repos;

public class FileStructureRepoTests
{
  private readonly AppDbContext _dbContext;
  private readonly FileStructureRepo _fileStructureRepo;

  public FileStructureRepoTests()
  {
    // Dependencies
    _dbContext = GetDbContext();
    // SUT
    _fileStructureRepo = new FileStructureRepo(_dbContext);
  }

  private AppDbContext GetDbContext()
  {
    var options = new DbContextOptionsBuilder<AppDbContext>()
      .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
      .Options;
    var dbContext = new AppDbContext(options);
    dbContext.Database.EnsureCreated();
    if (dbContext.FileStructures.Count() < 0)
    {
      for (int i = 0; i < 10; i++)
      {
        dbContext.FileStructures.Add(new FileStructure()
        {
          Id = (i + 1),
          ProjId = i + 2,
          FileStructureData = "test string"
        });
        dbContext.SaveChangesAsync();
      }
    }
    return dbContext;
  }

  [Fact]
  public void Add_ReturnsTrue()
  {
    // Arrange
    var fileStructure = new FileStructure()
    {
      Id = 2,
      ProjId = 3,
      FileStructureData = "test string"
    };
    // Act
    var result = _fileStructureRepo.Add(fileStructure);
    // Assert
    Assert.True(result);
  }

  [Fact]
  public void Delete_ReturnsTrue()
  {
    // Arrange
    var fileStructure = new FileStructure()
    {
      Id = 2,
      ProjId = 3,
      FileStructureData = "test string"
    };
    _fileStructureRepo.Add(fileStructure);
    // Act
    var result = _fileStructureRepo.Delete(fileStructure);
    // Assert
    Assert.True(result);
  }

  [Fact]
  public void Update_ReturnsTrue()
  {
    // Arrange
    var fileStructure = new FileStructure()
    {
      Id = 2,
      ProjId = 3,
      FileStructureData = "test string"
    };
    _fileStructureRepo.Add(fileStructure);
    // Act
    var result = _fileStructureRepo.Update(fileStructure);
    // Assert
    Assert.True(result);
  }

  [Fact]
  public void Save_ReturnsBool()
  {
    // Arrange (empty)
    // Act
    var result = _fileStructureRepo.Save();
    // Assert
    Assert.IsType<bool>(result);
  }

  [Fact]
  public async void GetByProjectIdAsync_ReturnsFileStructureTask()
  {
    // Arrange
    int projId = 3;
    // Act
    var result = _fileStructureRepo.GetByProjectIdAsync(projId);
    // Assert
    await Assert.IsType<Task<FileStructure>>(result);
  }
}
