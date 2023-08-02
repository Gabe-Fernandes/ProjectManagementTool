using Microsoft.EntityFrameworkCore;
using PMT.Data.Models;
using PMT.Data;

namespace PMTTests.Data.Repos;

public class TechStackRepoTests
{
  private readonly AppDbContext _dbContext;
  private readonly TechStackRepo _techStackRepo;

  public TechStackRepoTests()
  {
    // Dependencies
    _dbContext = GetDbContext();
    // SUT
    _techStackRepo = new TechStackRepo(_dbContext);
  }

  private AppDbContext GetDbContext()
  {
    var options = new DbContextOptionsBuilder<AppDbContext>()
      .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
      .Options;
    var dbContext = new AppDbContext(options);
    dbContext.Database.EnsureCreated();
    if (dbContext.TechStacks.Count() < 0)
    {
      for (int i = 0; i < 10; i++)
      {
        dbContext.TechStacks.Add(new TechStack()
        {
          Id = (i + 1),
          ProjId = (i + 2),
          SourceControl = "test string",
          BackendFramework = "test string",
          BackendLanguage = "test string",
          FrontendFramework = "test string",
          FrontendLanguage = "test string",
          Styling = "test string",
          Database = "test string",
          Deployment = "test string",
          ORM = "test string",
          UIDesign = "test string"
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
    var techStack = new TechStack()
    {
      Id = 2,
      ProjId = 3,
      SourceControl = "test string",
      BackendFramework = "test string",
      BackendLanguage = "test string",
      FrontendFramework = "test string",
      FrontendLanguage = "test string",
      Styling = "test string",
      Database = "test string",
      Deployment = "test string",
      ORM = "test string",
      UIDesign = "test string"
    };
    // Act
    var result = _techStackRepo.Add(techStack);
    // Assert
    Assert.True(result);
  }

  [Fact]
  public void Delete_ReturnsTrue()
  {
    // Arrange
    var techStack = new TechStack()
    {
      Id = 2,
      ProjId = 3,
      SourceControl = "test string",
      BackendFramework = "test string",
      BackendLanguage = "test string",
      FrontendFramework = "test string",
      FrontendLanguage = "test string",
      Styling = "test string",
      Database = "test string",
      Deployment = "test string",
      ORM = "test string",
      UIDesign = "test string"
    };
    _techStackRepo.Add(techStack);
    // Act
    var result = _techStackRepo.Delete(techStack);
    // Assert
    Assert.True(result);
  }

  [Fact]
  public void Update_ReturnsTrue()
  {
    // Arrange
    var techStack = new TechStack()
    {
      Id = 2,
      ProjId = 3,
      SourceControl = "test string",
      BackendFramework = "test string",
      BackendLanguage = "test string",
      FrontendFramework = "test string",
      FrontendLanguage = "test string",
      Styling = "test string",
      Database = "test string",
      Deployment = "test string",
      ORM = "test string",
      UIDesign = "test string"
    };
    _techStackRepo.Add(techStack);
    // Act
    var result = _techStackRepo.Update(techStack);
    // Assert
    Assert.True(result);
  }

  [Fact]
  public void Save_ReturnsBool()
  {
    // Arrange (empty)
    // Act
    var result = _techStackRepo.Save();
    // Assert
    Assert.IsType<bool>(result);
  }

  [Fact]
  public async void GetByProjectIdAsync_ReturnsTechStackTask()
  {
    // Arrange
    int projId = 3;
    // Act
    var result = _techStackRepo.GetByProjectIdAsync(projId);
    // Assert
    await Assert.IsType<Task<TechStack>>(result);
  }
}
