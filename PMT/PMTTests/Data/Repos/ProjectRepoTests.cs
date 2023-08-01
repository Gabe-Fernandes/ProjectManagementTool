using Microsoft.EntityFrameworkCore;
using PMT.Data.Models;
using PMT.Data;

namespace PMTTests.Data.Repos;

public class ProjectRepoTests
{
  private readonly AppDbContext _dbContext;
  private readonly ProjectRepo _projectRepo;

  public ProjectRepoTests()
  {
    // Dependencies
    _dbContext = GetDbContext();
    // SUT
    _projectRepo = new ProjectRepo(_dbContext);
  }

  private AppDbContext GetDbContext()
  {
    var options = new DbContextOptionsBuilder<AppDbContext>()
      .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
      .Options;
    var dbContext = new AppDbContext(options);
    dbContext.Database.EnsureCreated();
    if (dbContext.Projects.Count() < 0)
    {
      for (int i = 0; i < 10; i++)
      {
        dbContext.Projects.Add(new Project()
        {
          Id = (i + 1),
          StartDate = DateTime.Now,
          DueDate = DateTime.Now,
          JoinCode = "test string",
          Name = "test string"
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
    var project = new Project()
    {
      Id = 2,
      StartDate = DateTime.Now,
      DueDate = DateTime.Now,
      JoinCode = "test string",
      Name = "test string"
    };
    // Act
    var result = _projectRepo.Add(project);
    // Assert
    Assert.True(result);
  }

  [Fact]
  public void Delete_ReturnsTrue()
  {
    // Arrange
    var project = new Project()
    {
      Id = 2,
      StartDate = DateTime.Now,
      DueDate = DateTime.Now,
      JoinCode = "test string",
      Name = "test string"
    };
    _projectRepo.Add(project);
    // Act
    var result = _projectRepo.Delete(project);
    // Assert
    Assert.True(result);
  }

  [Fact]
  public void Update_ReturnsTrue()
  {
    // Arrange
    var project = new Project()
    {
      Id = 2,
      StartDate = DateTime.Now,
      DueDate = DateTime.Now,
      JoinCode = "test string",
      Name = "test string"
    };
    _projectRepo.Add(project);
    // Act
    var result = _projectRepo.Update(project);
    // Assert
    Assert.True(result);
  }

  [Fact]
  public void Save_ReturnsBool()
  {
    // Arrange (empty)
    // Act
    var result = _projectRepo.Save();
    // Assert
    Assert.IsType<bool>(result);
  }

  [Fact]
  public async void GetByIdAsync_ReturnsProjectTask()
  {
    // Arrange
    var id = 2;
    // Act
    var result = _projectRepo.GetByIdAsync(id);
    // Assert
    await Assert.IsType<Task<Project>>(result);
  }

  [Fact]
  public async void GetAllFromUserAsync_ReturnsIEnumerableProjectTask()
  {
    // Arrange
    string appUserId = "test";
    // Act
    var result = _projectRepo.GetAllFromUserAsync(appUserId);
    // Assert
    await Assert.IsType<Task<IEnumerable<Project>>>(result);
  }
}
