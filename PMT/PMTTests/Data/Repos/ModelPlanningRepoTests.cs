using Microsoft.EntityFrameworkCore;
using PMT.Data.Models;
using PMT.Data;

namespace PMTTests.Data.Repos;

public class ModelPlanningRepoTests
{
  private readonly AppDbContext _dbContext;
  private readonly ModelPlanningRepo _modelPlanningRepo;

  public ModelPlanningRepoTests()
  {
    // Dependencies
    _dbContext = GetDbContext();
    // SUT
    _modelPlanningRepo = new ModelPlanningRepo(_dbContext);
  }

  private AppDbContext GetDbContext()
  {
    var options = new DbContextOptionsBuilder<AppDbContext>()
      .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
      .Options;
    var dbContext = new AppDbContext(options);
    dbContext.Database.EnsureCreated();
    if (dbContext.ModelPlans.Count() < 0)
    {
      for (int i = 0; i < 10; i++)
      {
        dbContext.ModelPlans.Add(new ModelPlanning()
        {
          Id = (i + 1),
          ProjId = i + 2,
          Models = "test string",
          Properties = "test string",
          Validations = "test string"
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
    var modelPlanning = new ModelPlanning()
    {
      Id = 2,
      ProjId = 3,
      Models = "test string",
      Properties = "test string",
      Validations = "test string"
    };
    // Act
    var result = _modelPlanningRepo.Add(modelPlanning);
    // Assert
    Assert.True(result);
  }

  [Fact]
  public void Delete_ReturnsTrue()
  {
    // Arrange
    var modelPlanning = new ModelPlanning()
    {
      Id = 2,
      ProjId = 3,
      Models = "test string",
      Properties = "test string",
      Validations = "test string"
    };
    _modelPlanningRepo.Add(modelPlanning);
    // Act
    var result = _modelPlanningRepo.Delete(modelPlanning);
    // Assert
    Assert.True(result);
  }

  [Fact]
  public void Update_ReturnsTrue()
  {
    // Arrange
    var modelPlanning = new ModelPlanning()
    {
      Id = 2,
      ProjId = 3,
      Models = "test string",
      Properties = "test string",
      Validations = "test string"
    };
    _modelPlanningRepo.Add(modelPlanning);
    // Act
    var result = _modelPlanningRepo.Update(modelPlanning);
    // Assert
    Assert.True(result);
  }

  [Fact]
  public void Save_ReturnsBool()
  {
    // Arrange (empty)
    // Act
    var result = _modelPlanningRepo.Save();
    // Assert
    Assert.IsType<bool>(result);
  }

  [Fact]
  public async void GetByProjectIdAsync_ReturnsModelPlanningTask()
  {
    // Arrange
    int projId = 3;
    // Act
    var result = _modelPlanningRepo.GetByProjectIdAsync(projId);
    // Assert
    await Assert.IsType<Task<ModelPlanning>>(result);
  }
}
