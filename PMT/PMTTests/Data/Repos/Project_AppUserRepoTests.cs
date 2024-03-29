using Microsoft.EntityFrameworkCore;
using PMT.Data.Models;
using PMT.Data;

namespace PMTTests.Data.Repos;

public class Project_AppUserRepoTests
{
  private readonly AppDbContext _dbContext;
  private readonly Project_AppUserRepo _Project_AppUserRepo;

  public Project_AppUserRepoTests()
  {
    //Dependencies
    _dbContext = GetDbContext();
    // SUT
    _Project_AppUserRepo = new Project_AppUserRepo(_dbContext);
  }
  
  private AppDbContext GetDbContext()
  {
    var options = new DbContextOptionsBuilder<AppDbContext>()
      .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
    var dbContext = new AppDbContext(options);
    dbContext.Database.EnsureCreated();
    if (dbContext.Project_AppUsers.Count() < 0)
    {
      for (int i = 0; i < 10; i++)
      {
        dbContext.Project_AppUsers.Add(new Project_AppUser()
        {
          Id = (i + 1),
          Approved = true,
          AppUserId = (i + 1).ToString(),
          ProjId = i,
          Role = "test"
        }); ;
        dbContext.SaveChangesAsync();
      }
    }
    return dbContext;
  }

  [Fact]
  public void Add_ReturnsTrue()
  {
    // Arrange
    var Project_AppUser = new Project_AppUser()
    {
      Id = 2,
      Approved = true,
      AppUserId = "test",
      ProjId = 2,
      Role = "test"
    };
    // Act
    var result = _Project_AppUserRepo.Add(Project_AppUser);
    // Assert
    Assert.True(result);
  }

  [Fact]
  public void Delete_ReturnsTrue()
  {
    // Arrange
    var Project_AppUser = new Project_AppUser()
    {
      Id = 2,
      Approved = true,
      AppUserId = "test",
      ProjId = 2,
      Role = "test"
    };
    _Project_AppUserRepo.Add(Project_AppUser);
    // Act
    var result = _Project_AppUserRepo.Delete(Project_AppUser);
    // Assert
    Assert.True(result);
  }
  
  [Fact]
  public void Update_ReturnsTrue()
  {
    // Arrange
    var Project_AppUser = new Project_AppUser()
    {
      Id = 2,
      Approved = true,
      AppUserId = "test",
      ProjId = 2,
      Role = "test"
    };
    _Project_AppUserRepo.Add(Project_AppUser);
    // Act
    var result = _Project_AppUserRepo.Update(Project_AppUser);
    // Assert
    Assert.True(result);
  }
  
  [Fact]
  public void Save_ReturnsBool()
  {
    // Arrange (empty)
    // Act
    var result = _Project_AppUserRepo.Save();
    // Assert
    Assert.IsType<bool>(result);
  }
  
  [Fact]
  public async void GetByIdAsync_ReturnsProject_AppUserTask()
  {
    // Arrange
    var id = 2;
    // Act
    var result = _Project_AppUserRepo.GetByIdAsync(id);
    // Assert
    await Assert.IsType<Task<Project_AppUser>>(result);
  }
}
